using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickToolRotateDirection : MonoBehaviour {

    public GameObject crosshair;
    public float rotationOffset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var dir = crosshair.transform.position - transform.position; // get direction to crosshair object
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // use direction to gain angle
        transform.rotation = Quaternion.AngleAxis(angle + rotationOffset, Vector3.forward); // rotate to angle with offset
    }
}
