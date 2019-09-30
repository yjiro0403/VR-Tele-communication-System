using UnityEngine;
using System.Collections;

public class AvaterJointSend : AvatarController {
    public int jointNum = 20; //Kinect.JointType.ThumbRight
    float[] positionFloats; //jointNum * xyz
    public float[] skipPosition; //送信しないボーン番号を入力

    UDPCom _udpCom;
    public int sendInterval = 5;
    int ct = 0;
    public string ipAddress = "192.168.10.2";

    // Public variables that will get matched to bones. If empty, the Kinect will simply not track it.
    public Transform HipCenter;
    public Transform Spine;
    public Transform ShoulderCenter;
    public Transform Neck;
    //	public Transform Head;

    public Transform ClavicleLeft;
    public Transform ShoulderLeft;
    public Transform ElbowLeft;
    public Transform HandLeft;
    public Transform FingersLeft;
    //	private Transform FingerTipsLeft = null;
    //	private Transform ThumbLeft = null;

    public Transform ClavicleRight;
    public Transform ShoulderRight;
    public Transform ElbowRight;
    public Transform HandRight;
    public Transform FingersRight;
    //	private Transform FingerTipsRight = null;
    //	private Transform ThumbRight = null;

    public Transform HipLeft;
    public Transform KneeLeft;
    public Transform FootLeft;
    //	private Transform ToesLeft = null;

    public Transform HipRight;
    public Transform KneeRight;
    public Transform FootRight;
    //	private Transform ToesRight = null;

    [Tooltip("The body root node (optional).")]
    public Transform BodyRoot;

    // Offset node this transform is relative to, if any (optional)
    //public GameObject OffsetNode;

    //作業者カメラ位置送信
    public Transform _camera;

    // If the bones to be mapped have been declared, map that bone to the model.
    protected override void MapBones()
    {
        bones[0] = HipCenter;
        bones[1] = Spine;
        bones[2] = ShoulderCenter;
        bones[3] = Neck;
        //		bones[4] = Head;

        bones[5] = ShoulderLeft;
        bones[6] = ElbowLeft;
        bones[7] = HandLeft;
        bones[8] = FingersLeft;
        //		bones[9] = FingerTipsLeft;
        //		bones[10] = ThumbLeft;

        bones[11] = ShoulderRight;
        bones[12] = ElbowRight;
        bones[13] = HandRight;
        bones[14] = FingersRight;
        //		bones[15] = FingerTipsRight;
        //		bones[16] = ThumbRight;

        bones[17] = HipLeft;
        bones[18] = KneeLeft;
        bones[19] = FootLeft;
        //		bones[20] = ToesLeft;

        bones[21] = HipRight;
        bones[22] = KneeRight;
        bones[23] = FootRight;
        //		bones[24] = ToesRight;

        // special bones
        bones[25] = ClavicleLeft;
        bones[26] = ClavicleRight;

        // body root and offset
        bodyRoot = BodyRoot;
    }

    // Use this for initialization
    void Start () {
        //positionFloats = new float[jointNum * 3];
        positionFloats = new float[(jointNum+1+2) * 3]; //中心座標と各rotationの送信 + cameraTransform

        _udpCom = new UDPCom();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("HipLeft = " + bones[17].position.x);

        positionFloats[0] = bodyRoot.transform.position.x;
        positionFloats[1] = bodyRoot.transform.position.y;
        positionFloats[2] = bodyRoot.transform.position.z;

        int j = 1;
        for (int i = 0; i < 27; i++) {
            while (SkipBoneCheck(i)) {
                i++;
            }
            if (i >= 27) {
                positionFloats[j * 3] = _camera.transform.position.x;
                positionFloats[(j * 3)+1] = _camera.transform.position.y;
                positionFloats[(j * 3) + 2] = _camera.transform.position.z;
                positionFloats[(j * 3) + 3] = _camera.transform.eulerAngles.x;
                positionFloats[(j * 3) + 4] = _camera.transform.eulerAngles.y;
                positionFloats[(j * 3) + 5] = _camera.transform.eulerAngles.z;
                //Debug.Log("positionF " + positionFloats[(j * 3) + 4]);
                break; }
            /*
            positionFloats[j*3] = bones[i].transform.position.x;
            positionFloats[(j*3)+1] = bones[i].transform.position.y;
            positionFloats[(j*3) + 2] = bones[i].transform.position.z;
            */
            positionFloats[j * 3] = bones[i].transform.localEulerAngles.x;
            positionFloats[(j * 3) + 1] = bones[i].transform.localEulerAngles.y;
            positionFloats[(j * 3) + 2] = bones[i].transform.localEulerAngles.z;
            j++;
        }

        if (ct%sendInterval == 0){
            _udpCom.SendBone(positionFloats, ipAddress);
        }
        ct++;
    }

    bool SkipBoneCheck(int checknum)
    {
        for (int i = 0; i < skipPosition.Length;i++) {
            if (checknum == skipPosition[i]) {
                //Debug.Log("Skipped : " + skipPosition[i]);
                return true;
            }
        }
        return false;
    }
}