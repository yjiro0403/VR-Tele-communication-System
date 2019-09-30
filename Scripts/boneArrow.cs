using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boneArrow : MonoBehaviour {
    // ゴール
    [SerializeField]
    Transform target;
    // カーソル
    [SerializeField]
    GameObject cursor;
    Renderer cursorRenderer;

    [SerializeField]
    Vector3 leftCone, rightCone;
    [SerializeField]
    float angleyOffset = 90;
    
    // Use this for initialization
    void Start () {
        cursorRenderer = cursor.GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        cursor.transform.LookAt(target);

        //矢印の位置を決める
        Vector3 ArrowCameraToTarget = Quaternion.FromToRotation(transform.forward, target.position - transform.position).eulerAngles;
        
        if (ArrowCameraToTarget.y > 180)
        {
            ArrowCameraToTarget.y -= 360;
        }
        //Debug.Log(ArrowCameraToTarget.y);

        if (ArrowCameraToTarget.y > angleyOffset)
        {
            cursor.transform.localPosition = rightCone;
        }
        else
        {
            cursor.transform.localPosition = leftCone;
        }
    }

    private void OnTriggerEnter(Collider other){
        cursorRenderer.enabled = false;
    }

    private void OnTriggerExit(Collider other){
        cursorRenderer.enabled = true;
    }
}
