using System;
using Photon.Pun;

using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class UserSync : MonoBehaviourPun, IPunObservable
{
    
    public float health;
    public Vector3 someVariable;

     float health2;
     Vector3 someVariable2;
     
     // 스켈레탈 데이터 참조
     public Transform spine;
     public Transform leftArm;
     public Transform rightArm;

     // 보간(interpolation)을 위한 변수들
     private Vector3 syncSpinePos;
     private Quaternion syncSpineRot;
     private Vector3 syncLeftArmPos;
     private Quaternion syncLeftArmRot;
     private Vector3 syncRightArmPos;
     private Quaternion syncRightArmRot;
     
     
    void Start()
    {
     
    }

        
    void Update()
    {
        if (!photonView.IsMine)
        {
            Debug.LogWarning(health + "  " + someVariable + "   " + health2 + "   " + someVariable2);
            health = health2;
            someVariable = someVariable2;
            
            /*spine.position = Vector3.Lerp(spine.position, syncSpinePos, Time.deltaTime * 10);
            spine.rotation = Quaternion.Lerp(spine.rotation, syncSpineRot, Time.deltaTime * 10);

            leftArm.position = Vector3.Lerp(leftArm.position, syncLeftArmPos, Time.deltaTime * 10);
            leftArm.rotation = Quaternion.Lerp(leftArm.rotation, syncLeftArmRot, Time.deltaTime * 10);

            rightArm.position = Vector3.Lerp(rightArm.position, syncRightArmPos, Time.deltaTime * 10);
            rightArm.rotation = Quaternion.Lerp(rightArm.rotation, syncRightArmRot, Time.deltaTime * 10);*/
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(someVariable);

        }
        else
        {
            health2 = (float)stream.ReceiveNext();
            someVariable2 = (Vector3)stream.ReceiveNext();
        }
    }
}
