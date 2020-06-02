using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class funRotation : MonoBehaviour {

    public float rotationspeed;
    public float rotationAmp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler (new Vector3 (0, 0, Mathf.Sin (Time.time * rotationspeed) * rotationAmp));
	}
}
