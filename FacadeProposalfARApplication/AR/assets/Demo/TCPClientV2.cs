using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//modified from this for network https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient?view=netframework-4.7.2

public class TCPClientV2 : MonoBehaviour
{

    private UdpConnection connection;
    void Start()
    {
        string sendIp = "192.168.2.254";
        int sendPort = 11000;
        int receivePort = 8881;

        connection = new UdpConnection();
        connection.StartConnection(sendIp, sendPort, receivePort);

    }

    public  void SendScreenshot()
    {
        try
        {
            //TcpClient client = new TcpClient("192.168.2.254", 8052);
            //GameObject.Find("MyCanvas").GetComponent<Canvas>().enabled = false;
            //var texture = ScreenCapture.CaptureScreenshotAsTexture();
            //byte[] clientMessageAsByteArray = texture.EncodeToPNG();
            //NetworkStream stream = client.GetStream();
            //stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            //stream.Close();
            //client.Close();
            ////GameObject.Find("MyCanvas").GetComponent<Canvas>().enabled = true;
            //UnityEngine.Object.Destroy(texture);

            StartCoroutine(CaptureScreen());
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }

       

    }

    public IEnumerator CaptureScreen()
    {
        TcpClient client = new TcpClient("192.168.2.254", 8052); //connecting to laptop
        // Wait till the last possible moment before screen rendering to hide the UI
        yield return null;
        GameObject.Find("MyCanvas").GetComponent<Canvas>().enabled = false;

        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();

        // Take screenshot
        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        byte[] clientMessageAsByteArray = texture.EncodeToPNG();
        NetworkStream stream = client.GetStream();
        stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
        stream.Close();
        client.Close();

        // Show UI after we're done
        GameObject.Find("MyCanvas").GetComponent<Canvas>().enabled = true;

        UnityEngine.Object.Destroy(texture);

       // yield return new WaitForSeconds(6);

        // GameObject.Find("field").GetComponentInChildren<Text>().text = "length: " + messsages.Length + "content: " + messsages[0];

        //string[] messsages = connection.getMessages();
        //DecodePoints.DecodeAndCreate(messsages[0]);

    }


    public void LookForMessage()
    {
        string messsage = connection.GetMessage();
        //GameObject.Find("destroyer").GetComponentInChildren<Text>().text = "length: " + messsage.Length + "content: " + messsage;
        DecodePoints.DecodeAndCreate(messsage);
    }

   
}
