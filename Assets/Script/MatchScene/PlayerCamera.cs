using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject = null;

    private bool isTargeting = false;

    [Tooltip("카메라의 이동속도입니다")]
    [SerializeField]
    private float followSpeed = 8.0f;
    public float FollowSpeed { get => followSpeed; set => followSpeed = value; }
    public GameObject TargetObject { get => targetObject; set => targetObject = value; }
    public bool IsTargeting { get => isTargeting; set => isTargeting = value; }

    [Tooltip("Targeting할 오브젝트 기준 카메라 위치입니다")]
    public Vector3 vLocalCameraPos = new Vector3(0f, 0f, 0f);

    [Tooltip("Targeting할 오브젝트 기준 더한 lookat 값입니다.")]
    public Vector3 vAddLookAt = new Vector3(0f, 0f, 0f);

    private void FixedUpdate()
    {
        if (isTargeting)
            TargetingObject();
    }

    public void TargetingObject()
    {
        // 카메라 캐싱
        Camera mainCamera = Camera.main;

        Vector3 vPos = TargetObject.transform.position + vLocalCameraPos;
        Vector3 vLookAt = TargetObject.transform.position + vAddLookAt;
        Vector3 vLookAtDir = vLookAt - vPos;

        vLookAt.Normalize();

        mainCamera.transform.position
            = Vector3.Lerp( mainCamera.transform.position,
                            vPos, Time.deltaTime * FollowSpeed);

        mainCamera.transform.rotation
            = Quaternion.LookRotation(vLookAtDir);
    }
}
