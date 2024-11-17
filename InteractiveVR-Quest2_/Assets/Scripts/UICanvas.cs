using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
  
  public TextMeshProUGUI instructionTMP;

  public Image WomanHoverImg;
  public Image ManHoverImg;
  
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
    NetworkManager.Instance.SetupUserInfo(UserAvatarType.MAN);
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
    NetworkManager.Instance.SetupUserInfo(UserAvatarType.WOMAN);
  }
  
}
