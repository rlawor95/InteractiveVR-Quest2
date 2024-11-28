using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum UserAvatarType
{
    MAN,
    WOMAN
};

public class MGR : MonoBehaviourPun
{
    public static MGR instance = null;
    
    public UICanvas _UICanvas;
    public NetworkManager _networkManager;
    
    public GameObject xrMan;
    public GameObject xrWoman;

    public GameObject sittingMan;
    public GameObject sittingWoman;
    public Transform sittingManTransform;
    public Transform sittingWomanTransform;

    public Transform LeftTransform;
    public Transform RightTransform;

 
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

    const string CharacterSelectionKey = "CharacterSelection";

    public UserAvatarType myUserType;

    public WalkingCharacter _WalkingCharacter; 
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }   

    private void Start()
    {
        _UICanvas.SelectAvatarEvent += OnSelectAvatar;

        _networkManager.OnLeftOtherPlayer += OnPlayerLeftRoom;
        _networkManager.OnLeftLocalPlayer += OnLeftRoom;
        _networkManager.OnRoomJoined += OnJoinRoom;
        _networkManager.OnOtherPlayerEnterRoom += OnEnterOtherPlayer;

      
    }

  
    
    void InitializeCharacterSelection()
    {
        Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        if (roomProperties.ContainsKey(CharacterSelectionKey))
        {
            int[] characterStatus = (int[])roomProperties["CharacterSelection"];
            Debug.Log($"현재 캐릭터 상태: {string.Join(", ", characterStatus)}");
        }
        else
        {
            Debug.Log("캐릭터 선택 상태가 없습니다.");
            roomProperties[CharacterSelectionKey] = new int[] { 0, 0 };
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            
        }
    }

   
    
    void OnJoinRoom()
    {
        _UICanvas.EnableButton();
        InitializeCharacterSelection();
    }
    
    public void OnEnterOtherPlayer(Player player)
    {
        Debug.Log("OnEnterOtherPlayer");
        //UpdateHeadChecker();
        
    }
    
    public void OnLeftRoom()
    {
       // 로컬 플레이어 아웃 
       
       int characterId = (int)myUserType;
       int[] characterStatus = (int[])PhotonNetwork.CurrentRoom.CustomProperties[CharacterSelectionKey];
        
       Hashtable updatedProperties = new Hashtable();
       characterStatus[characterId] = 0;
       updatedProperties[CharacterSelectionKey] = characterStatus;
       PhotonNetwork.CurrentRoom.SetCustomProperties(updatedProperties);
       
    }
    
    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 다른 유저 아웃 
    }
    

    void UpdateCharacterSelectionkey(UserAvatarType type)
    {
        int characterId = (int)type;
        int[] characterStatus = (int[])PhotonNetwork.CurrentRoom.CustomProperties[CharacterSelectionKey];
        
        Hashtable updatedProperties = new Hashtable();
        characterStatus[characterId] = PhotonNetwork.LocalPlayer.ActorNumber;
        updatedProperties[CharacterSelectionKey] = characterStatus;
        PhotonNetwork.CurrentRoom.SetCustomProperties(updatedProperties);
    }
    
    public List<GameObject> networkObjects = new List<GameObject>();
    
    private void OnSelectAvatar(UserAvatarType type)
    {
        myUserType = type;
        UpdateCharacterSelectionkey(type);
        
        _UICanvas.gameObject.SetActive(false);
        FadePanel.DOFade(1, 0.5f);
        FadePanel.DOFade(0, 1f).SetDelay(2f);

        Debug.Log("RoomJoined " + type.ToString());

        GameObject go = null;
        if (type == UserAvatarType.MAN)
        {
            go = PhotonNetwork.Instantiate(xrMan.name, LeftTransform.position, LeftTransform.rotation);
            Player.transform.position = LeftTransform.position;
            Player.transform.rotation = LeftTransform.rotation;
            networkObjects.Add(go);
        }
        else
        {
            go = PhotonNetwork.Instantiate(xrWoman.name, RightTransform.position, RightTransform.rotation);
            Player.transform.position = RightTransform.position;
            Player.transform.rotation = RightTransform.rotation;
            networkObjects.Add(go);
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
        
        
        UpdateHeadChecker();
        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("나는 마스터 ^^ ");
            var go1 = PhotonNetwork.Instantiate(sittingMan.name, sittingManTransform.position,
                sittingManTransform.rotation);
            go1.GetComponent<SittingAvatar>().PlayerHead = HMD.gameObject;
            var go2 = PhotonNetwork.Instantiate(sittingWoman.name, sittingWomanTransform.position,
                sittingWomanTransform.rotation);
            go2.GetComponent<SittingAvatar>().PlayerHead = HMD.gameObject;
        }
        else
        {
            //Debug.Log("난 마스터 아님^^ ");
        }
        
        _WalkingCharacter.StartActing();
    }

    public void UpdateHeadChecker()
    {
      
        Debug.Log("UpdateHeadChecker11111111111111");
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("User");
        Debug.Log("find length " + objectsWithTag.Length);
        foreach (var go in objectsWithTag)
        {
            if (go != null)
            {

                PhotonView pv = go.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine == false)
                {
                    Debug.Log("UpdateHeadChecker==================== " + go.name);
                    go.GetComponentInChildren<RiggingManager>().PlayerHead = HMD.gameObject;
                }

            }
        }
        
    }

}
