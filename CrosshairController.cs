using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour {

    // crosshair radius and speed
    public float maxRadius = 2f;
    public float minRadius = 0.5f;
    public float speed = 10f;

    // crosshair active
    public bool crosshairActive;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {

        // crosshair position and speed
        var input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        var vectorFromParent = transform.localPosition + new Vector3(input.x, input.y, 0) * speed * Time.deltaTime;
        transform.localPosition = Vector2.ClampMagnitude(vectorFromParent, maxRadius);

        var minPosition = Vector2.ClampMagnitude(vectorFromParent, minRadius);

        if (transform.localPosition == new Vector3 (minPosition.x, minPosition.y, 0))
        {
            crosshairActive = false;
        }
        else 
        {
            crosshairActive = true;
        }


        ////------------------
        if (crosshairActive)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    crosshairActive = false;
    //}

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //if (collision.gameObject.tag == "Surface")
    //    {
    //        crosshairActive = false;
    //    }
    //}

    //void OnTriggerExit2D(Collider2D collision)
    //{
    //    crosshairActive = true;
    //}
}
