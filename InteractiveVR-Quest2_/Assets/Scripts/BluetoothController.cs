using UnityEngine;
using System.IO.Ports;

public class BluetoothController : MonoBehaviour
{
    private SerialPort serialPort;  // 블루투스 시리얼 포트
    public string portName = "COM5"; // 블루투스 포트 이름 (PC에 따라 변경)
    public int baudRate = 9600;     // 통신 속도

    public int x = 25; // 전송할 데이터 값 (value 뒤에 오는 숫자)

    private float sendInterval = 1f; // 데이터 전송 주기 (초 단위)
    private float lastSendTime = 0f;

    void Start()
    {
        // 시리얼 포트 초기화
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 1000; // 읽기 타임아웃 설정

        try
        {
            serialPort.Open(); // 포트 열기
            Debug.Log("Serial port opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }
    }

    void Update()
    {
        // 주기적으로 데이터 송신
        if (Time.time - lastSendTime >= sendInterval)
        {
            SendDataToArduino();
            lastSendTime = Time.time;
        }

        // 아두이노로부터 데이터 수신
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            try
            {
                string receivedData = serialPort.ReadLine(); // '\n' 기준으로 데이터 읽기
                Debug.Log("Received from Arduino: " + receivedData);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error reading from serial port: " + e.Message);
            }
        }
    }

    void SendDataToArduino()
    {
        if (serialPort.IsOpen)
        {
            string dataToSend = "value," + x + "\n"; // 데이터 구성
            serialPort.Write(dataToSend); // 데이터 전송
            Debug.Log("Sent to Arduino: " + dataToSend);
        }
        else
        {
            Debug.LogWarning("Serial port is not open.");
        }
    }

    void OnApplicationQuit()
    {
        // 애플리케이션 종료 시 포트 닫기
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }
    }
}
