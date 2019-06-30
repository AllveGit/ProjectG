using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchToScreen : MonoBehaviour
{
    //
    public GameObject Character2D = null;

    public float Character2DHeight = 1f;
    public float Character2DSpeed = 1f;

    private Vector3 Character2DOriginPos;
    private float Character2DSin = 0f;

    //
    public GameObject Logo = null;

    public float LogoHeight = 1f;
    public float LogoSpeed = 1f;

    private Vector3 LogoOriginPos;
    private float LogoSin = Mathf.PI;

    // 
    public GameObject TouchToScreenText = null;
    public GameObject ConnectingText = null;

    public UnityEvent OnTouchToScreen;

    void Start()
    {
        Character2DOriginPos = Character2D.transform.position;
        LogoOriginPos = Logo.transform.position;

        TouchToScreenText.SetActive(true);
        ConnectingText.SetActive(false);
    }
    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            TouchToScreenText.SetActive(false);
            ConnectingText.SetActive(true);

            OnTouchToScreen?.Invoke();
        }

        if (Character2DSin >= (Mathf.PI * 2))
            Character2DSin = 0f;

        float y = Character2DOriginPos.y + (Mathf.Sin(Character2DSin) * Character2DHeight);
        Character2D.transform.position = new Vector3(Character2DOriginPos.x, y, 0f);

        Character2DSin += Time.deltaTime * Character2DSpeed;

        if (LogoSin >= (Mathf.PI * 2))
            LogoSin = 0f;

        y = LogoOriginPos.y + (Mathf.Sin(LogoSin) * LogoHeight);
        Logo.transform.position = new Vector3(LogoOriginPos.x, y, 0f);

        LogoSin += Time.deltaTime * LogoSpeed;
    }
}
