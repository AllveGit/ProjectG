using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    public float restoreSpeed = 0.15f;
    public Vector3 endScale;

    private void ScaleRestore()
    {
        
    }

    public virtual void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public virtual void ShowWindow()
    {
        if (gameObject.activeSelf == true)
            return;

        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        gameObject.SetActive(true);
    }

    private void Start()
    {
        CloseWindow();
    }

    private void FixedUpdate()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, endScale, restoreSpeed);
    }
}
