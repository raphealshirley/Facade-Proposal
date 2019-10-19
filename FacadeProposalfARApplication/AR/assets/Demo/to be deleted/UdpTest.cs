using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UdpTest : MonoBehaviour
{

    private UdpConnection connection;
    // Start is called before the first frame update
    void Start()
    {
        string sendIp = "192.168.2.254";
        int sendPort = 11000;
        int receivePort = 8881;

        connection = new UdpConnection();
        connection.StartConnection(sendIp, sendPort, receivePort);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        connection.Stop();
    }

    public void SendMessageTest()
    {
       
        StartCoroutine(RecordFrame());

    }

    IEnumerator RecordFrame()
    {
        /*
        yield return new WaitForEndOfFrame();
        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        // do something with texture
        byte[] bytes = texture.EncodeToPNG();
        string testMessage = "sent from mobile" + bytes.Length;
        connection.Send(testMessage);
        //connection.SendBytes(bytes);
        // cleanup
        Object.Destroy(texture);
        */
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        //byte[] bytes = tex.EncodeToPNG();
        var bytes = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
        string test = ""+bytes.Length;
        connection.SendBytes(bytes);
        Object.Destroy(tex);

    }


}
