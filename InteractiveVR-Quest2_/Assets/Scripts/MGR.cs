using System;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public enum UserAvatarType
{
    MAN,
    WOMAN
};

public class MGR : MonoBehaviour
{
    public UICanvas _UICanvas;
    public NetworkManager _networkManager;
    
    public GameObject xrMan;
    public GameObject xrWoman;

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

    public Image FadePanel;

    private void Start()
    {
        //_networkManager.OnRoomJoined += NetworkManagerOnOnRoomJoined;
        //_networkManager.OnPlayerEnterRoom += NetworkManagerOnPlayerEntered;

        _UICanvas.SelectAvatarEvent += OnSelectAvatar;
        //ActiveNPC(false);
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

    private void NetworkManagerOnPlayerEntered(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(" PlayerEntered 나는 마스터 ^^ ");
           // TraceBox.Log("나는 마스터 ^^ ");
        }
        else
        {
            Debug.Log("PlayerEntered 난 마스터 아님^^ ");
          //  TraceBox.Log("난 마스터 아님^^ ");
        }

        //TraceBox.Log("다른 플레이어 입장 " + newPlayer.NickName);
        Debug.Log("PlayerEntered 다른 플레이어 입장  " + newPlayer.NickName);
        
       
        /*
        if (type == UserAvatarType.MAN)
        {
            go = PhotonNetwork.Instantiate(xrMan.name, LeftTransform.position, LeftTransform.rotation);
            TraceBox.Log("다른 플레이어 스폰 " + type.ToString());
            Debug.Log("다른 플레이어 스폰  " + type.ToString());

        }
        else
        {
            go = PhotonNetwork.Instantiate(xrWoman.name, RightTransform.position, RightTransform.rotation);
            TraceBox.Log("다른 플레이어 스폰 " + type.ToString());
            Debug.Log("다른 플레이어 스폰  " + type.ToString());
        }
        */

        //var rig = go.GetComponentInChildren<RiggingManager>();
        //rig.isXR = false;
    }

    private void OnSelectAvatar(UserAvatarType type)
    {
        _UICanvas.gameObject.SetActive(false);
        FadePanel.DOFade(1, 0.5f);
        FadePanel.DOFade(0, 1f).SetDelay(2f);
      //  TraceBox.Log("RoomJoined " + type.ToString());
        Debug.Log("RoomJoined " + type.ToString());
        //Destroy(SelectionXR);
        GameObject go = null;
        if (type == UserAvatarType.MAN)
        {
            go =  PhotonNetwork.Instantiate(xrMan.name, LeftTransform.position, LeftTransform.rotation);
            Player.transform.position = LeftTransform.position;
            Player.transform.rotation = LeftTransform.rotation;
        }
        else
        {
            go =  PhotonNetwork.Instantiate(xrWoman.name, RightTransform.position, RightTransform.rotation);
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

        //ActiveNPC(true);

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
