using UnityEngine;
using UnityEngine.VFX;


public class SittingAvatar : MonoBehaviour
{
    
    public VisualEffect leftVFXGraph;
    public VisualEffect rightVFXGraph;
    public VisualEffect HeadVFXGraph;
    
    public Transform LeftHand;
    public Transform RightHand;
    public GameObject Head;
    
    void Start()
    {
        HeadVFXGraph.SetVector3("PlayerPosition", Head.transform.position);
    }
    
    
    private void LateUpdate()
    {
        leftVFXGraph.SetVector3("SensingPosition", LeftHand.position);
        rightVFXGraph.SetVector3("SensingPosition", RightHand.position);
        HeadVFXGraph.SetVector3("PlayerForward", Head.transform.forward);
    }
}
