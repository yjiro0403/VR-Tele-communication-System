using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class moveContactPoint : MonoBehaviour {
    public Transform _camera, _workerLooking, _helperFace, _helperLooking;
    [SerializeField]
    Renderer workerContactRenderer;

    public enum SceneType
    {
        Avatar, HandTip
    }

    [SerializeField]
    SceneType _scene;

    StreamWriter workerContactPoint, helperContactPoint;

	// Use this for initialization
	void Start () {
        if (_scene == SceneType.Avatar)
        {
            workerContactPoint = new StreamWriter("WorkerContactPointAvatarLog.txt", false);
            helperContactPoint = new StreamWriter("HelperContactPointAvatarLog.txt", false);
        }
        else if (_scene == SceneType.HandTip)
        {
            workerContactPoint = new StreamWriter("WorkerContactPointHamdTipLog.txt", false);
            helperContactPoint = new StreamWriter("HelperContactPointHamdTipLog.txt", false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "WorkerGazeLine")
        {
            workerContactRenderer.enabled = true;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        RaycastHit hit;
        //Debug.Log(collider.gameObject.name);
        if (collider.gameObject.name == "HelperGazeLine")
        {
            if (Physics.Raycast(_helperFace.transform.position, _helperFace.transform.forward, out hit))
            {
                //Debug.Log("Contact Point : " + hit.point);
                _helperLooking.position = hit.point;
                helperContactPoint.WriteLine(System.DateTime.Now.Minute.ToString()+"/"+ System.DateTime.Now.Second.ToString() + "/" + System.DateTime.Now.Millisecond.ToString()+" "+
                    _helperLooking.position.x.ToString("F5") + "/" + _helperLooking.position.y.ToString("F5") + "/"+_helperLooking.position.z.ToString("F5"));
            }
        } else if (collider.gameObject.name == "WorkerGazeLine") {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit))
            {
                //Debug.Log("Contact Point : " + hit.point);
                _workerLooking.position = hit.point;
                workerContactPoint.WriteLine(System.DateTime.Now.Minute.ToString() + "/" + System.DateTime.Now.Second.ToString() + "/" + System.DateTime.Now.Millisecond.ToString() + " " +
                    _workerLooking.position.x.ToString("F5") + "/" + _workerLooking.position.y.ToString("F5") + "/" + _workerLooking.position.z.ToString("F5"));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "WorkerGazeLine")
        {
            workerContactRenderer.enabled = false;
        }
        
    }

    private void OnDestroy()
    {
        workerContactPoint.Close();
        helperContactPoint.Close();
    }
}
