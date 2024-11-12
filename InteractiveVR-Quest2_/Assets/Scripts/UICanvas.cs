using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UICanvas : MonoBehaviour
{
  
  public TextMeshProUGUI instructionTMP;

  public void Start()
  {
    
  }


  public void ManBtnHoverEvent(BaseEventData data)
  {
    Debug.Log("ManBtnHoverEvent");
  }
  
  public void ManBtnClickEvent()
  {
    Debug.Log("ManBtnClickEvent");
  }
  
  public void WomanBtnClickEvent()
  {
    Debug.Log("WomanBtnClickEvent");
  }
  
}
