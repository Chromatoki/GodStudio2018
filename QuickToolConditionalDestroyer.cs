using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickToolConditionalDestroyer : MonoBehaviour {

    public string GO_target_name;
    public float alphaFade;

	// Use this for initialization
	void Start () {
        alphaFade = GameObject.Find(GO_target_name).GetComponent<CanvasGroup>().alpha;

    }
	
	// Update is called once per frame
	void Update () {
        GameObject.Find(GO_target_name).GetComponent<CanvasGroup>().alpha = alphaFade;
        alphaFade = alphaFade - Time.deltaTime;
    }
}
