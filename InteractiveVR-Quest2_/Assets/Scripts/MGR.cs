using System;
using System.Collections.Generic;
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
    
    public GameObject goMan;
    public GameObject goWoman;

    public Transform LeftTransform;
    public Transform RightTransform;

    public List<GameObject> NPC_List;

    public GameObject SelectionXR;

    private void Start()
    {
        _networkManager.OnRoomJoined += NetworkManagerOnOnRoomJoined;
        _networkManager.OnPlayerEnterRoom += NetworkManagerOnPlayerEntered;
        ActiveNPC(false);
    }

    void ActiveNPC(bool b)
    {
        if (b)
        {
            foreach (var item in NPC_List)
            {
                item.SetActive(true);
            }
        }
        else
        {
            foreach (var item in NPC_List)
            {
                item.SetActive(false);
            }
        }
    }

    private void NetworkManagerOnPlayerEntered(UserAvatarType type)
    {
        
    }

    private void NetworkManagerOnOnRoomJoined(UserAvatarType type)
    {
        Destroy(SelectionXR);
        PhotonNetwork.Instantiate(goMan.name, RightTransform.position, RightTransform.rotation);
            
        /*if (type == UserAvatarType.MAN)
            PhotonNetwork.Instantiate(goMan.name, Vector3.zero, Quaternion.identity);
        else
            PhotonNetwork.Instantiate(goWoman.name, Vector3.zero, Quaternion.identity);*/

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
