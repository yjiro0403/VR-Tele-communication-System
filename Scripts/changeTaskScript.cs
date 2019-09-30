using UnityEngine; 
using System.Collections;
using System.IO;

public class changeTaskScript : MonoBehaviour {
    public Material[] _material;
    private int i;
    
    public Controller _controllerLeft, _controllerRight;

    //時間計測
    StreamWriter streamWriter;
    System.Diagnostics.Stopwatch stopWatch;

    // Use this for initialization 
    void Start () { 
        i = 0;

        streamWriter = new StreamWriter("TimerRealSpace.txt", false);
        stopWatch = new System.Diagnostics.Stopwatch();
    }

    // Update is called once per frame 
    void Update () {
        if (_controllerLeft.triggerCheck()) {
            if(i > 0) { i--; }
            this.GetComponent<Renderer>().material=_material[i]; 
        } else if (_controllerRight.triggerCheck()){
            if(i < _material.Length - 1) {
                i++;
                Debug.Log(i);
                this.GetComponent<Renderer>().material = _material[i];
                if (i == 1) {
                    //計測開始
                    stopWatch.Start();
                    streamWriter.WriteLine("task " + i + " : " + stopWatch.ElapsedMilliseconds + " ms");
                    streamWriter.Flush();
                } else if ( i == _material.Length - 1 ) {
                    Debug.Log("Finish ");
                    streamWriter.WriteLine("End Time : " + stopWatch.ElapsedMilliseconds + " ms");
                    streamWriter.Close();
                    stopWatch.Stop();
                } else {
                    streamWriter.WriteLine("task " + i + " : " + stopWatch.ElapsedMilliseconds + " ms");
                    streamWriter.Flush();
                }
            }
        }
    }
}