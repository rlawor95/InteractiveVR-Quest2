using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class RiggingManager : MonoBehaviour
{
    public Transform leftHandIK;
    public Transform rightHandIK;

    public Transform leftHandController;
    public Transform rightHandController;

    public Vector3[] leftOffset;
    public Vector3[] rightOffset;


    private void LateUpdate()
    {
        MappingHandTransform(leftHandIK, leftHandController, true);
        MappingHandTransform(rightHandIK, rightHandController, false);
    }

    private void MappingHandTransform(Transform ik, Transform controller, bool isleft)
    {
        var offset = isleft ? leftOffset : rightOffset;

        ik.position = controller.TransformPoint(offset[0]);
        ik.rotation = controller.rotation * Quaternion.Euler(offset[1]);
    }
}
