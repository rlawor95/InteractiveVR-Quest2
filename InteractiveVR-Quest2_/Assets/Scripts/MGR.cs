using System;
using Photon.Pun;
using UnityEngine;

public enum UserAvatarType
{
    MAN,
    WOMAN
};

public class MGR : MonoBehaviour
{
    public NetworkManager _networkManager;
    
    public GameObject goUser;


    private void Start()
    {
        _networkManager.OnRoomJoined += NetworkManagerOnOnRoomJoined;
    }

    private void NetworkManagerOnOnRoomJoined()
    {
        PhotonNetwork.Instantiate(goUser.name, Vector3.zero, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("나는 마스터 ^^ ");
        }
        else
        {
            Debug.Log("난 마스터 아님^^ ");
        }
    }
}
