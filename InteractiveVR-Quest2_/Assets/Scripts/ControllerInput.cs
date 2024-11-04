using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class ControllerInput : MonoBehaviour
{
    public XRInputValueReader<Vector2> m_StickInput;

    public Transform CamTransform;
    void Start()
    {
        m_StickInput?.EnableDirectActionIfModeUsed();
    }

    private void OnDestroy()
    {
        m_StickInput?.DisableDirectActionIfModeUsed();
    }

    
    void Update()
    {
        if (m_StickInput != null)
        {
            var stickVal = m_StickInput.ReadValue();
            //Debug.Log(stickVal);
            if (stickVal.x > 0.9f)
            {
                Debug.LogWarning("Right");
            }
            else if (stickVal.x < -0.9f)
            {
                Debug.LogWarning("Left");
            }
        }
    }
    
    void LateUpdate()
    {
        // 카메라의 로컬 위치를 (0,0,0)으로 고정하여 위치 변화를 무시
       //CamTransform.transform.localPosition = Vector3.zero;
    }
}
