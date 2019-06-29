using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    private Camera mainCamera = null;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("HpBar.cs : MainCamera를 찾을 수 없습니다");
    }
    void Update()
    {
        Vector3 vPos = transform.position;
        transform.LookAt(mainCamera.transform);
        transform.position = vPos;
    }
}
