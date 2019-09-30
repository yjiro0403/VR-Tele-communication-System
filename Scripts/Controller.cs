using UnityEngine;

public class Controller : MonoBehaviour {
    bool triggerPress = false;

    int jointNum = 3;
    Transform[] Pointer = new Transform[3];

    float closeHandPointerz = 80.0f;

    bool pointHandFlag = true;

    void Start()
    {
        //オブジェクト呼び出し
        Pointer[0] = transform.Find("Hand/JNT_Root/JNT_Wrist/JNT_Palm/JNT_Pointer01");
        Pointer[1] = Pointer[0].transform.Find("JNT_Pointer02");
        Pointer[2] = Pointer[1].transform.Find("JNT_Pointer03");
    }

    void Update() {
        SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
        var device = SteamVR_Controller.Input((int) trackedObject.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            triggerPress = true;
        }
    }

    public bool triggerCheck(){
        if(triggerPress){
            triggerPress = false;
            return true;
        } else { return false; }
    }
}