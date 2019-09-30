using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System.IO;

public class Bonesystem : MonoBehaviour
{
    public Material BoneMaterial;
    public Material transparentMaterial;
    public GameObject BodySourceManager;
    public GameObject Body;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    int jointNum = 12; //Kinect.JointType.ThumbRight
    float[] positionFloats; //jointNum * xyz

    Kinect.Body[] data;
    List<ulong> trackedIds;
    List<ulong> knownIds;

    StreamWriter streamWriter;
    int boneNumber = 0;

    public float boneSize = 1.0f;
    public int xTransferAmount = 30;
    public int yTransferAmount = 30;
    public int zTransferAmount = 10;
    public float xOffset = 0;
    public float yOffset = 0;
    public float zOffset = 40;

    public Vector3 bodyOffset = new Vector3(-0.175f, 0, -0.114f);
    public Vector3 rightHandOffset = new Vector3(-0.2f, 0, 0);

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>() {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

    void Start()
    {
        positionFloats = new float[jointNum*3];

        if (BodySourceManager == null)
        {
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }

        trackedIds = new List<ulong>();
        knownIds = new List<ulong>(_Bodies.Keys);
        /*
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.HandRight; jt++)
        {
            GameObject jointObj = Body.transform.FindChild(jt.ToString()).gameObject;
            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.01f, 0.01f);
            lr.SetColors(Color.green, Color.green);
        }*/

        streamWriter = new StreamWriter("./bone" + boneNumber + ".txt", true);
    }

    void Update()
    {

        data = _BodyManager.GetData();
        //Debug.Log(positionFloats.Length);
        if (data == null)
        {
            return;
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }


        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                RefreshBodyObject(body, Body);
            }
        }
        //_udpCom.SendBone(positionFloats, "10");
    }

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        int i = 0;
                                           
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.HandRight; jt++) {
            Kinect.Joint sourceJoint = body.Joints[jt]; //size ?
            
            Kinect.Joint? targetJoint = null;

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }

            Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
            if (jt == Kinect.JointType.HandRight){
                jointObj.localPosition = GetVector3FromJoint(sourceJoint) + bodyOffset + rightHandOffset;
            } else { jointObj.localPosition = GetVector3FromJoint(sourceJoint) + bodyOffset; }
            Vector3 transPosition = GetVector3FromJoint(sourceJoint) + bodyOffset;  //’ˆÓ

            Debug.Log(transPosition.z);
            Debug.Log(jointObj.localPosition.z);


            positionFloats[(i * 3)] = transPosition.x;
            positionFloats[(i * 3) + 1] = transPosition.y;
            positionFloats[(i * 3) + 2] = transPosition.z;

            i++;

            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if (targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition);
                if (jt == Kinect.JointType.HandRight)
                {
                    lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value) + bodyOffset + rightHandOffset);
                }
                else { lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value) + bodyOffset); }
            }
            else
            {
                lr.enabled = false;
            }
        }

        /*for (Kinect.JointType jt = Kinect.JointType.HandRight+1; jt <= Kinect.JointType.ThumbRight; jt++) {
            Kinect.Joint sourceJoint = body.Joints[jt]; //size ?
            Vector3 transPosition = GetVector3FromJointforSend(sourceJoint);

            positionFloats[(i * 3)] = transPosition.x;
            positionFloats[(i * 3) + 1] = transPosition.y;
            positionFloats[(i * 3) + 2] = transPosition.z;

            i++;
        }*/

        if (Input.GetMouseButton(2)) {
            i = 0;
            if (Input.GetMouseButtonDown(2)) {
                streamWriter.Close();
                boneNumber++;
                streamWriter = new StreamWriter("./bone" + boneNumber + ".txt", true);
            }

            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.HandRight; jt++){
                //Start Record
                i++;
            }
            streamWriter.Write(":");
            streamWriter.Flush();
        }
    }

    private Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        //Debug.Log("joint = " + joint.Position.X + " y: "+ joint.Position.Y + " z: " + joint.Position.Z);
        return new Vector3(joint.Position.X * boneSize * xTransferAmount + xOffset, joint.Position.Y * boneSize * yTransferAmount + yOffset, joint.Position.Z * boneSize * -1 * zTransferAmount + zOffset);
    }

    public float[] GetPositionfloats()
    {
        return positionFloats;
    }

    void OnApplicationQuit()
    {
        streamWriter.Close();
    }

    /*private Vector3 GetVector3FromJoint(Kinect.Joint joint)
{
    //Debug.Log("joint = " + joint.Position.X + " y: "+ joint.Position.Y + " z: " + joint.Position.Z);
    return new Vector3(joint.Position.X * boneSize * xTransferAmount + xOffset, joint.Position.Y * boneSize * yTransferAmount + yOffset, joint.Position.Z * boneSize * -1 * zTransferAmount + zOffset);
}*/


    /*private GameObject CreateBodyObject(ulong id)
{
    GameObject body = new GameObject("Body");

    for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
    {
        GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        jointObj.GetComponent<MeshRenderer>().material = transparentMaterial;

        if (lineRendering)
        {
            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);
            lr.SetColors(Color.green, Color.green);

            //lr.SetWidth(1.8f, 1.8f);

        }

        if (jt == Kinect.JointType.HandLeft || jt == Kinect.JointType.HandRight) { jointObj.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize); }
        else { jointObj.transform.localScale = new Vector3(0.00001f, 0.00001f, 0.00001f); }
        jointObj.name = jt.ToString();
        jointObj.transform.parent = body.transform;
    }

    return body;
}*/
}
