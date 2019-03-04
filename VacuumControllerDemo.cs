using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vacuum Control - alpha version
/// Handles all vacuum controls and actions
/// </summary>
public class VacuumControllerDemo : MonoBehaviour
{

    //---------------------------------------------------------------------------------------
    // Vacuum Raycast
    public float origDistance = 1.5f;
    public float distance = 1.5f;
    public float surfaceMaxDistance = 0.1f;

    public CrosshairController crosshair;
    private Vector2 crosshairPosition;
    public GameObject firepoint;
    private Vector2 firepointPosition;
    public Vector2 contactHit;
    public Vector2 direction;

    public RaycastHit2D vacuumRaycastHit;
    public Rigidbody2D RB_PC;
    public GameObject GO_PC_Controller;

    //Vacuum Physics Collider
    public PolygonCollider2D effectorPolygonCollider;
    public PointEffector2D effector;
    public GameObject suckingPrefab;
    public GameObject blowingPrefab;

    //public float suctionPower = 1;

    public LayerMask platformMask = 0;
    //public LayerMask detectionMask = 0;
    //public LayerMask vacuumMask = 0;

    //Vacuum Rotate Offset
    public int rotationOffset = 90;
    public SpriteRenderer _spriteRenderer;

    //Vacuum Charge for Pressure Jump
    public bool isVacuumCharging = false;
    [SerializeField] private float Charging_time = 0;
    public GameObject AirPressure;
    
    /// <summary>
    /// Handles all raycasting, mainly when it detects wall
    /// </summary>
    void VacuumRaycasting()
    {
        // Vacuum Rotation
        var dir = crosshair.transform.position - transform.position; // get direction to crosshair object
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // use direction to gain angle
        transform.rotation = Quaternion.AngleAxis(angle + rotationOffset, Vector3.forward); // rotate to angle with offset

        // set up Raycasting direction
        crosshairPosition = new Vector2(crosshair.transform.position.x, crosshair.transform.position.y);
        firepointPosition = new Vector2(firepoint.transform.position.x, firepoint.transform.position.y);
        direction = crosshairPosition - firepointPosition;

        // cast ray from firepoint position in Raycasting direction
        vacuumRaycastHit = Physics2D.Raycast(firepointPosition, direction, distance, platformMask);
        Debug.DrawRay(firepointPosition, direction, Color.cyan);

        if (vacuumRaycastHit == true)
        {
            contactHit = vacuumRaycastHit.point;

            Debug.DrawLine(firepointPosition, contactHit, Color.red);
            Debug.Log("We hit Wall:" + vacuumRaycastHit.collider.name);


            if (Input.GetKey(KeyCode.Mouse0)) // left mouse button to charge Vacuum pressure jump
            {
                if (vacuumRaycastHit.distance <= surfaceMaxDistance)
                {
                    isVacuumCharging = true;
                }
                else
                {
                    isVacuumCharging = false;
                    Charging_time = 0;
                }
            }

            if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.E) && vacuumRaycastHit.distance <= 0.01) // right mouse button to charge Vacuum suction cup
            {
                RB_PC.velocity = new Vector3 (0, 0, 0);
                RB_PC.simulated = false;
                crosshair.speed = 0f;
            }
            if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.E))
            {
                RB_PC.simulated = true;
                crosshair.speed = 25f;
            }
        }
        else
        {
            crosshair.speed = 25f;
            isVacuumCharging = false;
            RB_PC.simulated = true;
            distance = origDistance;
        }
    }

    /// <summary>
    /// Handles sucking
    /// </summary>
    void VacuumSuck()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.E)) // right mouse button to Vacuum Suck
        {
            suckingPrefab.SetActive(true);

            effector.gameObject.SetActive(true);
            effector.forceMagnitude = -1F;
            //_vacuumRaycastHits[i].collider.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * suctionPower * (1 / Vector3.Distance(transform.position, _vacuumRaycastHits[i].collider.gameObject.transform.position)), ForceMode2D.Impulse);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.E))
        {
            effector.gameObject.SetActive(false);
            suckingPrefab.SetActive(false);
        }
        //else
        //{
        //    effector.gameObject.SetActive(false);
        //    suckingPrefab.SetActive(false);
        //}
    }

    /// <summary>
    /// Handles blowing
    /// </summary>
    void VacuumBlow()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // left mouse button to Vacuum Blow
        {
            blowingPrefab.SetActive(true);

            effector.gameObject.SetActive(true);
            effector.forceMagnitude = 1F;
            //_vacuumRaycastHits[i].collider.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * suctionPower * (1 / Vector3.Distance(transform.position, _vacuumRaycastHits[i].collider.gameObject.transform.position)), ForceMode2D.Impulse);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            effector.gameObject.SetActive(false);
            blowingPrefab.SetActive(false);
        }
        //else
        //{
        //    effector.gameObject.SetActive(false);
        //    blowingPrefab.SetActive(false);
        //}
    }

    /// <summary>
    /// Handles charging
    /// </summary>
    void VacuumCharge()
    {
        if (isVacuumCharging)
        {
            crosshair.speed = 5f;

            //Charging_time = Charging_time + Time.deltaTime;
            if (Charging_time <= 5)
            {
                Charging_time++;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.W))
            {
                GO_PC_Controller.GetComponent<RoboMovement>().wantJump = true;
                Instantiate(AirPressure, contactHit, Quaternion.identity);
                crosshair.speed = 25f;

                Charging_time = 0;
                isVacuumCharging = false;
            }
        }
    }

    private void Update()
    {
        if (crosshair.crosshairActive) {
            VacuumRaycasting();
            VacuumSuck();
            VacuumBlow();
            VacuumCharge();
            _spriteRenderer.enabled = true;
        }
        else
        {
            distance = origDistance;
            effector.gameObject.SetActive(false);
            blowingPrefab.SetActive(false);
            suckingPrefab.SetActive(false);
            _spriteRenderer.enabled = false;
        }
    }
    //---------------------------------------------------------------------------------------
}//==========
