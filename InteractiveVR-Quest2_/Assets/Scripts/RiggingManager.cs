using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using Photon.Pun;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RiggingManager : MonoBehaviourPun, IPunObservable
{
    public Transform leftHandIK;
    public Transform rightHandIK;
    public Transform headIK;
    
    [Space(10)]
    public Transform leftHandController;
    public Transform rightHandController;
    public Transform hmd;
    
    public Vector3[] leftOffset;
    public Vector3[] rightOffset;
    public Vector3[] headOffset;

    public Transform OriginPos;
    
    //========
    public Vector3 SyncPos;
    public Vector3 SyncLeftPos;
    public Quaternion SyncLeftRot;
    public Vector3 SyncRightPos;
    public Quaternion SyncRightRot;
    public Quaternion SyncHMD;

    private float lerpSpeed = 10f;
    
    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            MappingHandTransform(leftHandIK, leftHandController, true);
            MappingHandTransform(rightHandIK, rightHandController, false);
            MappingHeadTransform(headIK, hmd);
        }
        else
        {
            OriginPos.position = SyncPos;
            leftHandIK.position = Vector3.Lerp(leftHandIK.position, SyncLeftPos, Time.deltaTime * lerpSpeed);
            leftHandIK.rotation = Quaternion.Lerp(leftHandIK.rotation, SyncLeftRot, Time.deltaTime * lerpSpeed);
            rightHandIK.position = Vector3.Lerp(rightHandIK.position, SyncRightPos, Time.deltaTime * lerpSpeed);
            rightHandIK.rotation = Quaternion.Lerp(rightHandIK.rotation, SyncRightRot, Time.deltaTime * lerpSpeed);
            headIK.rotation = Quaternion.Lerp(headIK.rotation, SyncHMD, Time.deltaTime * lerpSpeed);
        }
    }

    private void MappingHandTransform(Transform ik, Transform controller, bool isleft)
    {
        var offset = isleft ? leftOffset : rightOffset;

        ik.position = controller.TransformPoint(offset[0]);
        ik.rotation = controller.rotation * Quaternion.Euler(offset[1]);
    }

    private void MappingHeadTransform(Transform ik, Transform hmd)
    {
        ik.position = hmd.TransformPoint(headOffset[0]);
        ik.rotation = hmd.rotation * Quaternion.Euler(headOffset[1]);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(OriginPos.position);
            stream.SendNext(leftHandController.TransformPoint(leftOffset[0]));
            stream.SendNext(leftHandController.rotation * Quaternion.Euler(leftOffset[1]));
            stream.SendNext(rightHandController.TransformPoint(rightOffset[0]));
            stream.SendNext(rightHandController.rotation * Quaternion.Euler(rightOffset[1]));
            stream.SendNext(hmd.rotation * Quaternion.Euler(headOffset[1]));
        }
        else
        {
            SyncPos = (Vector3)stream.ReceiveNext();
            SyncLeftPos = (Vector3)stream.ReceiveNext();
            SyncLeftRot = (Quaternion)stream.ReceiveNext();
            SyncRightPos = (Vector3)stream.ReceiveNext();
            SyncRightRot = (Quaternion)stream.ReceiveNext();
            SyncHMD = (Quaternion)stream.ReceiveNext();
        }
    }
}
