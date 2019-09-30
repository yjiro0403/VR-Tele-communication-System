using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forearm : MonoBehaviour {
    [SerializeField]
    Transform Hand;
    float distVal = 3.3f;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        float dist = Mathf.Sqrt((Hand.position - transform.position).sqrMagnitude);
        transform.localScale = new Vector3(1.0f, dist*distVal, 1.0f);
        transform.up = Hand.position - transform.position;
    }
}
