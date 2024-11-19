using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UICanvas : MonoBehaviourPunCallbacks
{
  
  public TextMeshProUGUI instructionTMP;

  public Image WomanHoverImg;
  public Image ManHoverImg;
  public Button ManButton;
  public Button WomanButton;

  public event Action<UserAvatarType> SelectAvatarEvent;
  
  public void Start()
  {
    ManHoverImg.color = Color.clear;
    WomanHoverImg.color = Color.clear;

    instructionTMP.DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo);
  }


  public void ManBtnHoverEvent(BaseEventData data)
  {
    ManHoverImg.color = Color.white;
  }

  public void ManBtnExitEvent(BaseEventData data)
  {
    ManHoverImg.color = Color.clear;
  }

  public void ManBtnClickEvent()
  {
    Debug.Log("ManBtnClickEvent");
    //NetworkManager.Instance.SetupUserInfo(UserAvatarType.MAN);
    SelectAvatarEvent?.Invoke(UserAvatarType.MAN);
    photonView.RPC("DisableManButton", RpcTarget.Others);
  }


  public void WomanBtnHoverEvent(BaseEventData data)
  {
    WomanHoverImg.color = Color.white;
  }
  public void WomanBtnExitEvent(BaseEventData data)
  {
    WomanHoverImg.color = Color.clear;
  }
  
  public void WomanBtnClickEvent()
  {
    //NetworkManager.Instance.SetupUserInfo(UserAvatarType.WOMAN);
    SelectAvatarEvent?.Invoke(UserAvatarType.WOMAN);
    photonView.RPC("DisableWomanButton", RpcTarget.Others);
  }
  
  [PunRPC]
  void DisableManButton()
  {
    if (ManButton != null)
    {
      ManButton.interactable = false;
    }
  }


  [PunRPC]
  void DisableWomanButton()
  {
    if (WomanButton != null)
    {
      WomanButton.interactable = false;
    }
  }
}
