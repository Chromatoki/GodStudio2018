using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickToolTeleporter : MonoBehaviour {

    public GameObject GO_target;

    // ----------------------------------------------------------------------
    // Detect if something enter the Trigger
    void OnTriggerEnter2D(Collider2D _cl_detected)
    {
        // Is the trigger the PC 
        if (_cl_detected.tag == "Player")
        {
            // Set the new respawn position in the health script
            _cl_detected.gameObject.transform.position = new Vector3 (GO_target.transform.position.x, GO_target.transform.position.y, _cl_detected.gameObject.transform.position.z);
            _cl_detected.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, 0);

        }
    }//-----

    //---------------------------------------------------------------------------------------
    // 2D Checkpoint
    // David Dorrington, UEL Games, 2017 
}//==========

