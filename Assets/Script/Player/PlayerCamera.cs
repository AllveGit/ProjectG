using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject vTarget = null;

    private bool IsTargeting = false;

    [Tooltip("Targeting할 오브젝트 기준 카메라 위치입니다")]
    public Vector3 vLocalCameraPos = new Vector3(0f, 0f, 0f);

    [Tooltip("Targeting할 오브젝트 기준 더한 lookat 값입니다.")]
    public Vector3 vAddLookAt = new Vector3(0f, 0f, 0f);

    void Start()
    {
        if (vTarget == null)
            Debug.LogError("PlayerCamera : Targeting GameObject가 Null입니다");
    }

    void Update()
    {
        if (IsTargeting)
            TargetingObject();
    }

    void TargetingObject()
    {
        Vector3 vPos = vTarget.transform.position + vLocalCameraPos;
        Vector3 vLookAt = vTarget.transform.position + vAddLookAt;
        Vector3 vLookAtDir = vLookAt - vPos;
        vLookAt.Normalize();

        Camera.main.transform.position = vPos;
        Camera.main.transform.rotation = Quaternion.LookRotation(vLookAtDir);
    }

    public void OnTargeting() { IsTargeting = true; }
    public void OffTargeting() { IsTargeting = false; }
}
