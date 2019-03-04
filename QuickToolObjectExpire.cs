using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// For game object / visual effects
/// Attach to object you wish to expire
/// Set expiration time, after expiration, destroy object
/// </summary>
public class QuickToolObjectExpire : MonoBehaviour {

    public float fl_expirationTime = 2f;
    private float fl_time;
	
	// Update is called once per frame
	void Update () {
        fl_time = fl_time + Time.deltaTime;

        if (fl_time >= fl_expirationTime)
        {
            Destroy(gameObject);
        }
	}
}
