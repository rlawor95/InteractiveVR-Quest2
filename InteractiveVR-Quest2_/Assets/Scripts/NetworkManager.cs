using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private const string RoomName = "InteractiveVR";
    private const int MaxPlayer = 2;
    
    private void Awake()
    {
        SetupUserInfo();
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
        Debug.Log("Joined Room as " + (PhotonNetwork.IsMasterClient ? "Host" : "Client"));
        // 여기서 게임 시작 또는 플레이어 초기화 로직 추가
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player Joined: " + newPlayer.NickName);
        // 추가 플레이어에 대한 처리
    }
    
    void SetupUserInfo()
    {
        string playerName = "Jack";

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }
    }

    void CreatePhotonRoom()
    {
        string roomName = RoomName;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

        RoomOptions options = new RoomOptions {MaxPlayers = MaxPlayer, PlayerTtl = 10000 };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }
}
