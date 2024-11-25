using System;
using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using DG.Tweening;

public class SittingAvatar : MonoBehaviour
{
    enum VFXONOFF
    {
        ON,
        OFF
    }

    private VFXONOFF _vfxonoff = VFXONOFF.ON;
    public VisualEffect leftVFXGraph;
    public VisualEffect rightVFXGraph;
    public VisualEffect HeadVFXGraph;
    
    public Transform LeftHand;
    public Transform RightHand;
    public GameObject Head;

    [Space(10)] 
    public GameObject PlayerHead;
    public GameObject PlayerHeadChecker;

    private float threshold = 0.75f;
    
    void Start()
    {
        HeadVFXGraph.SetVector3("PlayerPosition", Head.transform.position);
    }


    private void Update()
    {
        if (PlayerHead != null)
        {
            var dir = PlayerHeadChecker.transform.position - PlayerHead.transform.position;
            var dot = Vector3.Dot(dir.normalized, PlayerHead.transform.forward);
            
          //  Debug.Log(gameObject.name + "  :  " + dot);
            if (dot > threshold)
            {
                OffVFX();
               
            }
            else
            {
                OnVFX();
            }
        }
    }

    private float scale = 0.3f;
    private float scaleOrigin = 0.3f;
    private void OnVFX()
    {
        /*if(_vfxonoff == VFXONOFF.ON)
            return;
        _vfxonoff = VFXONOFF.ON;*/
        
       // Debug.LogError("ON VFX");
        leftVFXGraph.gameObject.SetActive(true);
        rightVFXGraph.gameObject.SetActive(true);
        HeadVFXGraph.gameObject.SetActive(true);

        /*leftVFXGraph.SetFloat("Scale", scale);
        rightVFXGraph.SetFloat("Scale", scale);
        HeadVFXGraph.SetFloat("Scale", scale);
        
        DOTween.To(() => scale, x => scale = x, scaleOrigin, 2f)
            .OnUpdate(() =>
            {
                // 애니메이션 중 매 프레임 호출됨
                Debug.Log("현재 myValue: " + myValue);
            })*/
            
    }

    private void OffVFX()
    {
        /*
        if(_vfxonoff == VFXONOFF.OFF)
            return;
        _vfxonoff = VFXONOFF.OFF;
        */
        
        //Debug.LogError("OFF VFX");
        leftVFXGraph.gameObject.SetActive(false);
        rightVFXGraph.gameObject.SetActive(false);
        HeadVFXGraph.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        leftVFXGraph.SetVector3("SensingPosition", LeftHand.position);
        rightVFXGraph.SetVector3("SensingPosition", RightHand.position);
        HeadVFXGraph.SetVector3("PlayerForward", Head.transform.forward);
    }
}
