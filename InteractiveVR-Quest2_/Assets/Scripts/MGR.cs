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

    [Header("XR")]
    public GameObject Player;
    public Transform XRChildTransform;

    public Transform LeftController;
    public Transform RightController;
    public Transform HMD;

    public GameObject LeftControllerVisual;
    public GameObject RightControllerVisual;
    public GameObject LeftControllerInteractor;
    public GameObject RightControllerInteractor;

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
        //Destroy(SelectionXR);
        GameObject go = null;
        if (type == UserAvatarType.MAN)
        {
            go =  PhotonNetwork.Instantiate(goMan.name, LeftTransform.position, LeftTransform.rotation);
            Player.transform.position = LeftTransform.position;
            Player.transform.rotation = LeftTransform.rotation;
        }
        else
        {
            go =  PhotonNetwork.Instantiate(goWoman.name, RightTransform.position, RightTransform.rotation);
            Player.transform.position = RightTransform.position;
            Player.transform.rotation = RightTransform.rotation;
        }
        
       
        
        go.transform.parent = Player.transform;
        go.transform.position = XRChildTransform.position;
        var rig = go.GetComponentInChildren<RiggingManager>();
        rig.hmd = HMD;
        rig.leftHandController = LeftController;
        rig.rightHandController = RightController;
        
        LeftControllerInteractor.SetActive(false);
        RightControllerInteractor.SetActive(false);
        LeftControllerVisual.SetActive(false);
        RightControllerVisual.SetActive(false);
            
      

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
