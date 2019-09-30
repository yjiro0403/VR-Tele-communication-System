using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WorkerLogger : MonoBehaviour {
    [SerializeField]
    Transform Hmd;

    public enum SceneType
    {
        Avatar, HandTip
    }

    [SerializeField]
    SceneType _scene;

    StreamWriter workerTransform;

    // Use this for initialization
    void Start()
    {
        if (_scene == SceneType.Avatar)
        {
            workerTransform = new StreamWriter("WorkerTransformAvatarLog.txt", false);
        } else if (_scene == SceneType.HandTip)
        {
            workerTransform = new StreamWriter("WorkerTransformHandTipLog.txt", false);
        }
    }

    // Update is called once per frame
    void Update () {
        workerTransform.WriteLine(System.DateTime.Now.Minute.ToString() + "分" + System.DateTime.Now.Second.ToString() + "秒" + System.DateTime.Now.Millisecond.ToString());
        workerTransform.WriteLine(Hmd.position.x.ToString("F5") + "," + Hmd.position.y.ToString("F5") + "," + Hmd.position.z.ToString("F5")
            + "," + Hmd.rotation.x.ToString("F5") + "," + Hmd.rotation.y.ToString("F5") + "," + Hmd.rotation.z.ToString("F5"));
        workerTransform.Flush();
    }

    private void OnDestroy()
    {
        workerTransform.Close();
    }
}
