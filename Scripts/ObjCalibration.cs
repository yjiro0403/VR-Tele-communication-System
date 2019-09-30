using UnityEngine;
using System.Collections;

public class ObjCalibration : MonoBehaviour {
    public Camera hmd;
    public GameObject[] obj;
    public Vector3 offsetVal = new Vector3(3.0f,1.0f,1.2f);

    private Vector3[] positionOriginal;
    // Use this for initialization
    void Start() {
        positionOriginal = new Vector3[obj.Length];
        for (int i = 0; i < obj.Length; i++)
        {
            positionOriginal[i] = obj[i].transform.localPosition;
        }
    }
        // Update is called once per frame
        void Update () {
        for (int i = 0; i < obj.Length; i++) {
            Vector3 offset = new Vector3((positionOriginal[i].x - hmd.transform.localPosition.x) * offsetVal.x, (positionOriginal[i].y - hmd.transform.localPosition.y) * offsetVal.y, (positionOriginal[i].z - hmd.transform.localPosition.z) * offsetVal.z);
            Vector3 pos = new Vector3(positionOriginal[i].x + offset.x, positionOriginal[i].y + offset.y, positionOriginal[i].z + offset.z);
            //Debug.Log(pos);
            obj[i].transform.localPosition = pos;
        }
        //Debug.Log(obj[0].transform.localPosition.x * offset.x);
    }
}
