using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UICanvas : MonoBehaviourPunCallbacks
{
  static public UICanvas instance = null;
  public TextMeshProUGUI instructionTMP;

  public Image WomanHoverImg;
  public Image ManHoverImg;
  public Button ManButton;
  public Button WomanButton;

  public Image WomanCharacterImg;
  public Image ManCharacterImg;

  public event Action<UserAvatarType> SelectAvatarEvent; //MGR
  
  public void Start()
  {
    if (instance == null)
      instance = this;
    
    ManHoverImg.color = Color.clear;
    WomanHoverImg.color = Color.clear;

    instructionTMP.DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo);
    
    ManButton.interactable = false;
    WomanButton.interactable = false;
    
    WomanCharacterImg.color = Color.gray;
    ManCharacterImg.color = Color.gray;

  }

  public void UpdateCharacterSelection(int[] characterStatus)
  {
    for (int i = 0; i < characterStatus.Length; i++)
    {
      if (i == 0)
      {
        var value = characterStatus[i];
        if (value > 0)
        {
          ManButton.interactable = false;
          ManCharacterImg.color = Color.gray;
        }
        else
        {
          ManButton.interactable = true;
          ManCharacterImg.color = Color.white;
        }
      }
      else if (i == 1)
      {
        var value = characterStatus[i];
        if (value > 0)
        {
          WomanButton.interactable = false;
          WomanCharacterImg.color = Color.gray;
    
        }
        else
        {
          WomanButton.interactable = true;
          WomanCharacterImg.color = Color.white;
        }
      }
    }
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
    SelectAvatarEvent?.Invoke(UserAvatarType.WOMAN);
    photonView.RPC("DisableWomanButton", RpcTarget.Others);
  }

  public void EnableButton()
  {
    ManButton.interactable = true;
    WomanButton.interactable = true;
    
    WomanCharacterImg.color = Color.white;
    ManCharacterImg.color = Color.white;
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
