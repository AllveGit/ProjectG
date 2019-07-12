using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class JoyStick : MonoBehaviour
{
    public Transform stickTransform;

    private Vector3 stickFirstPos; // 클릭한 처음 위치.
    private Vector3 joyDir; // 조이스틱의 (방향)
    private float backGroundRadius; // 조이스틱 배경의 반지름.

    public float JoyScale { get; private set; }
    public Vector3 JoyDir { get => new Vector3(joyDir.x, 0f, joyDir.y); }

    public event UnityAction<Vector3, Vector3> OnStickDown;
    public event UnityAction<Vector3, Vector3> OnStickUp;

    private void Start()
    {
        backGroundRadius = (GetComponent<RectTransform>().sizeDelta.x * 0.5f);
        stickFirstPos = stickTransform.transform.position;
    }

    public void Drag(BaseEventData _Data)
    {
        // 마우스 And 터치와 관련된 이벤트 데이터.
        PointerEventData data = _Data as PointerEventData;
        Vector3 pos = data.position;

        joyDir = (pos - stickFirstPos).normalized;

        float distance = Vector3.Distance(pos, stickFirstPos);

        if (distance < backGroundRadius)
        {
            stickTransform.position = stickFirstPos + joyDir * distance;
            JoyScale = (1f / backGroundRadius) * distance;
        }
        else
        {
            stickTransform.position = stickFirstPos + joyDir * backGroundRadius;
            JoyScale = (1f / backGroundRadius) * backGroundRadius;
        }
        
        OnStickDown?.Invoke(stickTransform.position, joyDir);
    }

    public void DragEnd()
    {
        OnStickUp?.Invoke(stickTransform.position, joyDir);

        stickTransform.position = stickFirstPos;
        joyDir = Vector3.zero;
        JoyScale = 0f;
    }
}
