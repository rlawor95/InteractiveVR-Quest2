using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TCPClient : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;

    public string esp32IPAddress; // ESP32 IP ÁÖ¼Ò
    public int esp32Port = 80;    // ESP32 Æ÷Æ® ¹øÈ£
    //public Slider motor1Slider;   // ¸ðÅÍ 1 ½½¶óÀÌ´õ (UI)
    //public Slider motor2Slider;   // ¸ðÅÍ 2 ½½¶óÀÌ´õ (UI)

    private byte length1Value = 0; // ¸ðÅÍ 1 °ª (½½¶óÀÌ´õ·Î Á¶Á¤)
    private byte length2Value = 0; // ¸ðÅÍ 2 °ª (½½¶óÀÌ´õ·Î Á¶Á¤)

    public float sendInterval = 0.5f; // ½ÅÈ£¸¦ º¸³»´Â ÁÖ±â (ÃÊ ´ÜÀ§)
    private float lastSendTime = 0.0f; // ¸¶Áö¸· ½ÅÈ£ Àü¼Û ½Ã°£

    void Start()
    {
        // ½½¶óÀÌ´õ °ªÀÌ º¯°æµÉ ¶§¸¶´Ù °ª ¾÷µ¥ÀÌÆ®
        //motor1Slider.onValueChanged.AddListener((value) => motor1Value = ConvertSliderToByte(value));
        //motor2Slider.onValueChanged.AddListener((value) => motor2Value = ConvertSliderToByte(value));
//#if !UNITY_EDITOR
        ConnectToESP32();
//#endif
    }

    void Update()
    {
//#if !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetSendData(0.0f, 0.0f);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            SetSendData(0.5f, 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SetSendData(1.0f, 1.0f);
        }
        

        // ÀÏÁ¤ ÁÖ±â¸¶´Ù ½ÅÈ£¸¦ Àü¼Û
        if (Time.time - lastSendTime > sendInterval)
        {
            SendMotorData(length1Value, length2Value);
            lastSendTime = Time.time; // ¸¶Áö¸· ½ÅÈ£ Àü¼Û ½Ã°£ °»½Å
        }
//#endif
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftLengthValue"></param>
    /// <param name="rightLengthValue"></param>
    public void SetSendData(float leftLengthValue, float rightLengthValue)
    {
        length1Value = ConvertSliderToByte(leftLengthValue);
        length2Value = ConvertSliderToByte(rightLengthValue);
    }


    void OnApplicationQuit()
    {
        DisconnectFromESP32();
    }

    /// <summary>
    /// ½½¶óÀÌ´õ °ªÀ» 0~1 ¹üÀ§¿¡¼­ 0~255 ¹üÀ§·Î º¯È¯
    /// </summary>
    /// <param name="sliderValue">½½¶óÀÌ´õ °ª (0 ~ 1)</param>
    /// <returns>º¯È¯µÈ ¸ðÅÍ °ª (0 ~ 255)</returns>
    private byte ConvertSliderToByte(float sliderValue)
    {
        return (byte)(sliderValue * 255);
    }

    /// <summary>
    /// ESP32¿¡ ¿¬°á
    /// </summary>
    private void ConnectToESP32()
    {
        try
        {
            client = new TcpClient();
            client.Connect(esp32IPAddress, esp32Port);
            stream = client.GetStream();
            Debug.Log("Connected to ESP32");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to connect to ESP32: " + e.Message);
        }
    }

    /// <summary>
    /// ESP32¿ÍÀÇ ¿¬°á Á¾·á
    /// </summary>
    private void DisconnectFromESP32()
    {
        if (stream != null)
        {
            stream.Close();
        }
        if (client != null)
        {
            client.Close();
        }
        Debug.Log("Disconnected from ESP32");
    }

    /// <summary>
    /// ESP32·Î ¸ðÅÍ Á¦¾î µ¥ÀÌÅÍ Àü¼Û
    /// </summary>
    /// <param name="motor1">¸ðÅÍ 1 °ª (0~255)</param>
    /// <param name="motor2">¸ðÅÍ 2 °ª (0~255)</param>
    private void SendMotorData(byte motor1, byte motor2)
    {
        if (client == null || !client.Connected)
        {
            Debug.LogWarning("Not connected to ESP32");
            return;
        }

        try
        {
            // ½ÃÀÛ ¹ÙÀÌÆ®(0x02), ¸ðÅÍ °ª, ³¡ ¹ÙÀÌÆ®(0x03)·Î µ¥ÀÌÅÍ ÆÐÅ¶ »ý¼º
            byte[] data = new byte[4];
            data[0] = 0x02;        // ½ÃÀÛ ¹ÙÀÌÆ®
            data[1] = motor1;      // ¸ðÅÍ 1 °ª
            data[2] = motor2;      // ¸ðÅÍ 2 °ª
            data[3] = 0x03;        // ³¡ ¹ÙÀÌÆ®

            // µ¥ÀÌÅÍ Àü¼Û
            using (var stream = client.GetStream())
            {
                stream.Write(data, 0, data.Length);
                Debug.Log($"Sent data to ESP32: {BitConverter.ToString(data)}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to send data to ESP32: " + e.Message);
        }
    }
}
