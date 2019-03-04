using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For animating Sprites - alpha version
/// Attach to object you wish to invert localscale
/// Based on location and status of crosshair
/// </summary>
public class LocalScaleFlip : MonoBehaviour {

    public GameObject GO_crosshair;
    public bool bl_Invert; //check to invert settings
	
	// Update is called once per frame
	void Update () {

        // get crosshair world position 
        Vector2 crosshairPos = GO_crosshair.transform.position;
        Vector3 newScale = transform.localScale;

        if(!bl_Invert)
        {
            if (crosshairPos.x < transform.position.x && transform.localScale.x > 0) { newScale.x *= -1; transform.localScale = newScale; }
            if (crosshairPos.x > transform.position.x && transform.localScale.x < 0) { newScale.x *= -1; transform.localScale = newScale; }
        }
        else // if settings inverted, do the opposite
        {
            if (crosshairPos.x < transform.position.x && transform.localScale.x < 0) { newScale.x *= -1; transform.localScale = newScale; }
            if (crosshairPos.x > transform.position.x && transform.localScale.x > 0) { newScale.x *= -1; transform.localScale = newScale; }
        }
    }
}
