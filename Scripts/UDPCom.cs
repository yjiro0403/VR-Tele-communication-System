using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;


public class UDPCom : MonoBehaviour {
  //float[] position;
  const char splitSymbol = ',';
    int listenPort = 8000;
    private static bool received;

  private struct UdpState{
    public IPEndPoint endPoint;
    public UdpClient udpClient;
  }

  /*public float[] GetData(){
    Debug.Log("UDPCOM.cs : received floats " + position.Length);
    return position;
  }*/

  void Start(){
    //position = new float[3 * 6];
    //StartCoroutine(ReceivePosition());
    //Debug.Log("UDP Start");
  }
  
    public void SendBone(float[] bodyArray, string ipAddress)
    {
      string body = "";
      
      
      var remote = new IPEndPoint(
        IPAddress.Parse(ipAddress),
        listenPort);
      
      //var message = Encoding.UTF8.GetBytes("Hello world !");
      
      var client = new UdpClient(8000);
      client.Connect(remote); 
        
        //Bodyをstringにして変換
      for (int i = 0; i < bodyArray.Length; i++) {
          body += splitSymbol + bodyArray[i].ToString();
      }
      var message = Encoding.UTF8.GetBytes(body);
      client.Send(message, message.Length);
        //Debug.Log("Message = " + body);
      client.Close();
    }



}
