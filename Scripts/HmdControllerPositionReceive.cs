using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class HmdControllerPositionReceive : MonoBehaviour{
    public Transform bodyPosition, controllerLeft, controllerRight, kinectHead, kinectBody;

    public int listenPort = 8080;
    private static bool received = false;
    const char splitSymbol = ',';

    //キャリブレーション
    public Camera hmd;
    public Vector3 avatarOffset = new Vector3(0.0f, -0.5f, -2.0f);
    public Vector3 offsetVal = new Vector3(3.0f, 1.0f, 1.2f);
    public Vector3 initialPositionOffset = new Vector3(0.0f, 0.0f, 0.7f);

    float[] receivedFloats;

    int jointNum = 3;
    float closeHandPointerz = 80.0f;
    Transform[] leftPointer = new Transform[3];
    Transform[] rightPointer = new Transform[3];


    private struct UdpState {
        public IPEndPoint endPoint;
        public UdpClient udpClient;
    }

    // Use this for initialization
    void Start () {
        //オブジェクト呼び出し
        leftPointer[0] = transform.Find("HandLTip/Hand/JNT_Root/JNT_Wrist/JNT_Palm/JNT_Pointer01");
        leftPointer[1] = leftPointer[0].transform.Find("JNT_Pointer02");
        leftPointer[2] = leftPointer[1].transform.Find("JNT_Pointer03");
        rightPointer[0] = transform.Find("HandRTip/Hand/JNT_Root/JNT_Wrist/JNT_Palm/JNT_Pointer01");
        rightPointer[1] = rightPointer[0].transform.Find("JNT_Pointer02");
        rightPointer[2] = rightPointer[1].transform.Find("JNT_Pointer03");

        //receivedFloats = new float[3 * 6];
        receivedFloats = new float[(3 * 6)+2]; //handState 

        StartCoroutine(ReceivePosition());
        Debug.Log("UDP Start(Vive)");        
    }

    // Update is called once per frame
    void Update () {
        bodyPosition.transform.position = new Vector3(receivedFloats[0] - (kinectHead.position.x - kinectBody.position.x) + avatarOffset.x, receivedFloats[1] - (kinectHead.position.y - kinectBody.position.y) + avatarOffset.y, receivedFloats[2] - (kinectHead.position.z - kinectBody.position.z) + avatarOffset.z);

        kinectHead.eulerAngles = new Vector3(receivedFloats[3], receivedFloats[4], receivedFloats[5]);
        //Debug.Log(kinectHead.localEulerAngles.y);
        Vector3 offset = new Vector3((bodyPosition.transform.localPosition.x - hmd.transform.localPosition.x) * offsetVal.x, (bodyPosition.transform.localPosition.y - hmd.transform.localPosition.y) * 0, ((bodyPosition.transform.localPosition.z - hmd.transform.localPosition.z) * offsetVal.z) + ((bodyPosition.transform.localPosition.y - hmd.transform.localPosition.y) * offsetVal.y));
        Vector3 pos = new Vector3(bodyPosition.transform.localPosition.x + offset.x, bodyPosition.transform.localPosition.y + offset.y, bodyPosition.transform.localPosition.z + offset.z);
        bodyPosition.transform.localPosition = pos;
        //Debug.Log("hmd = " + bodyPosition.transform.localPosition.z);

        controllerLeft.transform.position = new Vector3(receivedFloats[6] + initialPositionOffset.x, receivedFloats[7] + initialPositionOffset.y, receivedFloats[8] + initialPositionOffset.z);
        controllerLeft.localEulerAngles = new Vector3(receivedFloats[9], receivedFloats[10], receivedFloats[11]);
        //Debug.Log("Left x = " + receivedFloats[6] + " Left y = " + receivedFloats[7] +  " Left z = " + receivedFloats[8]);


        controllerRight.transform.position = new Vector3(receivedFloats[12] + initialPositionOffset.x, receivedFloats[13] + initialPositionOffset.y, receivedFloats[14] + initialPositionOffset.z);
        controllerRight.localEulerAngles = new Vector3(receivedFloats[15], receivedFloats[16], receivedFloats[17]);
        //Debug.Log("Right x = " + receivedFloats[12] + " Right y = " + receivedFloats[13] + " Right z = " + receivedFloats[14]);

        if (receivedFloats[18] == 0)
        {
            //Debug.Log("Close Hand");
            for (int i = 0; i < jointNum; i++)
            {
                leftPointer[i].localEulerAngles = new Vector3(leftPointer[i].localEulerAngles.x, leftPointer[i].localEulerAngles.y, closeHandPointerz);
            }
        } else {
            //Debug.Log("Pointing Hand");
            for (int i = 0; i < jointNum; i++)
            {
                leftPointer[i].localEulerAngles = new Vector3(leftPointer[i].localEulerAngles.x, leftPointer[i].localEulerAngles.y, 0.0f);
            }
        }

        if (receivedFloats[19] == 0)
        {
            //Debug.Log("Close Hand");
            for (int i = 0; i < jointNum; i++)
            {
                rightPointer[i].localEulerAngles = new Vector3(rightPointer[i].localEulerAngles.x, rightPointer[i].localEulerAngles.y, closeHandPointerz);
            }
        }
        else
        {
            //Debug.Log("Pointing Hand");
            for (int i = 0; i < jointNum; i++)
            {
                rightPointer[i].localEulerAngles = new Vector3(rightPointer[i].localEulerAngles.x, rightPointer[i].localEulerAngles.y, 0.0f);
            }
        }
    }

    public void ReceiveCallback(System.IAsyncResult ar)
    {
        UdpClient udpClient = ((UdpState)(ar.AsyncState)).udpClient;
        IPEndPoint endPoint = ((UdpState)(ar.AsyncState)).endPoint;
        var receiveBytes = udpClient.EndReceive(ar, ref endPoint);
        var receiveString = Encoding.UTF8.GetString(receiveBytes);

        //Debug.Log(string.Format("Hmd Controller Received: {0}", receiveString));

        string[] arrayData = receiveString.Split(splitSymbol);

        for (int i = 1; i < arrayData.Length; i++)
        {
            receivedFloats[i - 1] = float.Parse(arrayData[i]);
        }
        //Debug.Log("Bodies i = " + receivedFloats[0]);
        //Debug.Log("Bodies Length = " + receivedFloats.Length);

        received = true;
    }

    public IEnumerator ReceivePosition()
    {
        var endPoint = new IPEndPoint(IPAddress.Any, listenPort);
        var udpClient = new UdpClient(endPoint);
        var udpState = new UdpState();
        udpState.endPoint = endPoint;
        udpState.udpClient = udpClient;

        while (true)
        {
            received = false;

            udpClient.BeginReceive(new System.AsyncCallback(ReceiveCallback), udpState);

            while (!received)
            {
                yield return null;
            }

        }
    }
}
