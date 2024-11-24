using System;
using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;

public class SittingAvatar : MonoBehaviour
{
    
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
            
            Debug.Log(gameObject.name + "  :  " + dot);
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

    private void OnVFX()
    {
        Debug.LogError("ON VFX");
        leftVFXGraph.gameObject.SetActive(true);
        rightVFXGraph.gameObject.SetActive(true);
        HeadVFXGraph.gameObject.SetActive(true);
    }

    private void OffVFX()
    {
        Debug.LogError("OFF VFX");
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
