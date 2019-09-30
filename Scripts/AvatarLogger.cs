using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AvatarLogger : MonoBehaviour {
    [SerializeField]
    Transform Hips, Spine, Chest, Neck, Head, ShoulderL, UpperArmL, ForeArmL, HandL, ShoulderR, UpperArmR, ForeArmR, HandR;

    //アバター位置計測
    StreamWriter avatarTransform;

	// Use this for initialization
	void Start () {
        avatarTransform = new StreamWriter("AvatarTransformLog.txt", false);
        }
	
	// Update is called once per frame
	void Update () {
        avatarTransform.WriteLine(System.DateTime.Now.Minute.ToString() + "分" + System.DateTime.Now.Second.ToString() + "秒" + System.DateTime.Now.Millisecond.ToString());
        //Hip, Spine, Chest, Neck, Head, ShoulderL, UpperArmL, ForeArmL, HandL, ShoulderR, UpperArmR, ForeArmR, HandR, Rotationで同じ 
        avatarTransform.WriteLine(Hips.position.x.ToString("F5") + "," + Hips.position.y.ToString("F5") + "," + Hips.position.z.ToString("F5")
            + "," + Spine.position.x.ToString("F5") + "," + Spine.position.y.ToString("F5") + "," + Spine.position.z.ToString("F5")
            + "," + Chest.position.x.ToString("F5") + "," + Chest.position.y.ToString("F5") + "," + Chest.position.z.ToString("F5")
            + "," + Neck.position.x.ToString("F5") + "," + Neck.position.y.ToString("F5") + "," + Neck.position.z.ToString("F5")
            + "," + Head.position.x.ToString("F5") + "," + Head.position.y.ToString("F5") + "," + Head.position.z.ToString("F5")
            + "," + ShoulderL.position.x.ToString("F5") + "," + ShoulderL.position.y.ToString("F5") + "," + ShoulderL.position.z.ToString("F5")
            + "," + UpperArmL.position.x.ToString("F5") + "," + UpperArmL.position.y.ToString("F5") + "," + UpperArmL.position.z.ToString("F5")
            + "," + ForeArmL.position.x.ToString("F5") + "," + ForeArmL.position.y.ToString("F5") + "," + ForeArmL.position.z.ToString("F5")
            + "," + HandL.position.x.ToString("F5") + "," + HandL.position.y.ToString("F5") + "," + HandL.position.z.ToString("F5")
            + "," + ShoulderR.position.x.ToString("F5") + "," + ShoulderR.position.y.ToString("F5") + "," + ShoulderR.position.z.ToString("F5")
            + "," + HandR.position.x.ToString("F5") + "," + HandR.position.y.ToString("F5") + "," + HandR.position.z.ToString("F5")
            + "," + Spine.rotation.x.ToString("F5") + "," + Spine.rotation.y.ToString("F5") + "," + Spine.rotation.z.ToString("F5")
            + "," + Chest.rotation.x.ToString("F5") + "," + Chest.rotation.y.ToString("F5") + "," + Chest.rotation.z.ToString("F5")
            + "," + Neck.rotation.x.ToString("F5") + "," + Neck.rotation.y.ToString("F5") + "," + Neck.rotation.z.ToString("F5")
            + "," + Head.rotation.x.ToString("F5") + "," + Head.rotation.y.ToString("F5") + "," + Head.rotation.z.ToString("F5")
            + "," + ShoulderL.rotation.x.ToString("F5") + "," + ShoulderL.rotation.y.ToString("F5") + "," + ShoulderL.rotation.z.ToString("F5")
            + "," + UpperArmL.rotation.x.ToString("F5") + "," + UpperArmL.rotation.y.ToString("F5") + "," + UpperArmL.rotation.z.ToString("F5")
            + "," + ForeArmL.rotation.x.ToString("F5") + "," + ForeArmL.rotation.y.ToString("F5") + "," + ForeArmL.rotation.z.ToString("F5")
            + "," + HandL.rotation.x.ToString("F5") + "," + HandL.rotation.y.ToString("F5") + "," + HandL.rotation.z.ToString("F5")
            + "," + ShoulderR.rotation.x.ToString("F5") + "," + ShoulderR.rotation.y.ToString("F5") + "," + ShoulderR.rotation.z.ToString("F5")
            + "," + HandR.rotation.x.ToString("F5") + "," + HandR.rotation.y.ToString("F5") + "," + HandR.rotation.z.ToString("F5"));
        /*
        avatarTransform.WriteLine("Hip : " + Hips.position.ToString("F5") + ", Spine : " + Spine.position.ToString("F5"));
        avatarTransform.WriteLine("Chest : " + Chest.position.ToString("F5"));
        avatarTransform.WriteLine("Neck : " + Neck.position.ToString("F5"));
        avatarTransform.WriteLine("Head : " + Hips.position.ToString("F5"));
        avatarTransform.WriteLine("ShoulderL : " + Hips.position.ToString("F5"));
        avatarTransform.WriteLine("UpperArmL : " + Hips.position.ToString("F5"));
        avatarTransform.WriteLine("ForeArmL : " + Hips.position.ToString("F5"));
        avatarTransform.WriteLine("HandL : " + Hips.position.ToString("F5"));
        avatarTransform.WriteLine("ShoulderR : " + Hips.position.ToString("F5"));
        avatarTransform.WriteLine("UpperArmR : " + Hips.position.ToString("F5"));
        avatarTransform.WriteLine("ForeArmR : " + Hips.position.ToString("F5"));
        avatarTransform.WriteLine("HandR : " + Hips.position.ToString("F5"));
        avatarTransform.WriteLine("Rotation");
        avatarTransform.WriteLine("Hip : " + Hips.rotation.ToString("F5"));
        avatarTransform.WriteLine("Spine : " + Spine.rotation.ToString("F5"));
        avatarTransform.WriteLine("Chest : " + Chest.rotation.ToString("F5"));
        avatarTransform.WriteLine("Neck : " + Neck.rotation.ToString("F5"));
        avatarTransform.WriteLine("Head : " + Hips.rotation.ToString("F5"));
        avatarTransform.WriteLine("ShoulderL : " + Hips.rotation.ToString("F5"));
        avatarTransform.WriteLine("UpperArmL : " + Hips.rotation.ToString("F5"));
        avatarTransform.WriteLine("ForeArmL : " + Hips.rotation.ToString("F5"));
        avatarTransform.WriteLine("HandL : " + Hips.rotation.ToString("F5"));
        avatarTransform.WriteLine("ShoulderR : " + Hips.rotation.ToString("F5"));
        avatarTransform.WriteLine("UpperArmR : " + Hips.rotation.ToString("F5"));
        avatarTransform.WriteLine("ForeArmR : " + Hips.rotation.ToString("F5"));
        avatarTransform.WriteLine("HandR : " + Hips.rotation.ToString("F5"));
        avatarTransform.WriteLine(" ");
        */
        avatarTransform.Flush();
    }

    private void OnDestroy()
    {
        avatarTransform.Close();
    }
}
