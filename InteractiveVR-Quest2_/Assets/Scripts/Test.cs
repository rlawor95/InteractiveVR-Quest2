using UnityEngine;
using UnityEngine.VFX;

public class Test : MonoBehaviour
{
    public GameObject Player;
    public VisualEffect VFXGraph;
    
    void Start()
    {
        VFXGraph.SetVector3("PlayerPosition", Player.transform.position);
    }

    void Update()
    {
        Debug.Log(Player.transform.forward);
        VFXGraph.SetVector3("PlayerForward", Player.transform.forward);
    }
}
