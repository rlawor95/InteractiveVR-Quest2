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
    
    public event Action<UserAvatarType> OnRoomJoined;
    public event Action<UserAvatarType> OnPlayerEnterRoom;

    private UserAvatarType selectedType;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        //SetupUserInfo(UserAvatarType.MAN);
        //CreatePhotonRoom();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
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
        
        OnRoomJoined?.Invoke(selectedType);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 여기는 다른 플레이어 접속시 타는 함수 
        Debug.Log("Player Joined: " + newPlayer.NickName);
        // 추가 플레이어에 대한 처리
        //OnRoomJoined?.Invoke();
        if (selectedType == UserAvatarType.MAN)
            OnPlayerEnterRoom?.Invoke(UserAvatarType.WOMAN);
        else
            OnPlayerEnterRoom?.Invoke(UserAvatarType.MAN);

    }

    public void SetupUserInfo(UserAvatarType type)
    {
        string playerName = type.ToString();
        selectedType = type;
        
        if (!playerName.Equals(""))
        {
            Debug.Log("SetupUserInfo   " + playerName);
            PhotonNetwork.SendRate = 20; // 초당 20번 데이터 전송
            PhotonNetwork.SerializationRate = 10; // 초당 10번 직렬화 호출
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
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
