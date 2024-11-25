using System;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance = null;
    
    private const string RoomName = "InteractiveVR";
    private const int MaxPlayer = 2;
    
    public event Action OnRoomJoined; // MGR 
    public event Action<Player> OnOtherPlayerEnterRoom; 

    //private UserAvatarType selectedType;
    public event Action OnLeftLocalPlayer;
    public event Action<Player> OnLeftOtherPlayer;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        SetupUserInfo();
        //CreatePhotonRoom();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Local player has left the room.");
        OnLeftLocalPlayer?.Invoke();
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player " + otherPlayer.NickName + " has left the room.");
        OnLeftOtherPlayer?.Invoke(otherPlayer);
    }

    public override void OnConnectedToMaster()
    {
      //  TraceBox.Log("Connected to Photon Master Server");
        Debug.Log("Connected to Photon Master Server");
        PhotonNetwork.JoinRoom(RoomName);
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed: " + message);
        // 룸이 없으면 새로 생성
        PhotonNetwork.CreateRoom(RoomName, new RoomOptions { MaxPlayers = MaxPlayer }, null);
    }

    public override void OnJoinedRoom()
    {
        // 여기는 나만 타는 함수 
        Debug.Log("Joined Room as " + (PhotonNetwork.IsMasterClient ? "Host" : "Client"));
        // 여기서 게임 시작 또는 플레이어 초기화 로직 추가
      //  TraceBox.Log("Joined Room as " + (PhotonNetwork.IsMasterClient ? "Host" : "Client"));
        OnRoomJoined?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 여기는 다른 플레이어 접속시 타는 함수 
        Debug.Log("네트워크 매니저 OnPlayerEnteredRoom : " + newPlayer.NickName);
        var type = newPlayer.NickName;
        // 추가 플레이어에 대한 처리
        OnOtherPlayerEnterRoom?.Invoke(newPlayer);
       // OnPlayerEnterRoom?.Invoke(newPlayer);
  
    }

    public void SetupUserInfo()
    {
        //string playerName = type.ToString();
        //selectedType = type;
        string playerName = "User" + Random.Range(0, 100);
        
        if (!playerName.Equals(""))
        {
            //TraceBox.Log("SetupUserInfo " + type.ToString());
            Debug.Log("SetupUserInfo   " + playerName);
            PhotonNetwork.SendRate = 20; // 초당 20번 데이터 전송
            PhotonNetwork.SerializationRate = 10; // 초당 10번 직렬화 호출
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
           // TraceBox.Log("Player Name is invalid.");
            Debug.LogError("Player Name is invalid.");
        }
    }

    /*void CreatePhotonRoom()
    {
        string roomName = RoomName;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

        RoomOptions options = new RoomOptions {MaxPlayers = MaxPlayer, PlayerTtl = 10000 };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }*/
}
