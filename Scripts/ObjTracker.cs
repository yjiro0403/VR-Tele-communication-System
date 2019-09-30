using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTracker : MonoBehaviour {
    public GameObject obj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = obj.transform.localPosition;
	}
}
