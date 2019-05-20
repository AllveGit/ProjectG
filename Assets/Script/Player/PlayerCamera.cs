using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject targetObject = null;

    private bool isTargeting = false;

    [Tooltip("카메라의 이동속도입니다")]
    [SerializeField]
    private float followSpeed = 8.0f;
    public float FollowSpeed { get => followSpeed; set => followSpeed = value; }

    [Tooltip("Targeting할 오브젝트 기준 카메라 위치입니다")]
    public Vector3 vLocalCameraPos = new Vector3(0f, 0f, 0f);

    [Tooltip("Targeting할 오브젝트 기준 더한 lookat 값입니다.")]
    public Vector3 vAddLookAt = new Vector3(0f, 0f, 0f);

    void Start()
    {
        if (targetObject == null)
            Debug.LogError("PlayerCamera : Targeting GameObject가 null입니다");
    }

    void Update()
    {
        if (isTargeting)
            TargetingObject();
    }

    void TargetingObject()
    {
        // 카메라 캐싱
        Camera mainCamera = Camera.main;

        Vector3 vPos = targetObject.transform.position + vLocalCameraPos;
        Vector3 vLookAt = targetObject.transform.position + vAddLookAt;
        Vector3 vLookAtDir = vLookAt - vPos;

        vLookAt.Normalize();

        mainCamera.transform.position
            = Vector3.Lerp( mainCamera.transform.position,
                            vPos, Time.deltaTime * FollowSpeed);

        mainCamera.transform.rotation
            = Quaternion.LookRotation(vLookAtDir);
    }

    public void OnTargeting() { isTargeting = true; }
    public void OffTargeting() { isTargeting = false; }
}
