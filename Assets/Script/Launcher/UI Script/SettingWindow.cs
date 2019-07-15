using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : BaseWindow
{
    public Scrollbar effectScroll;
    public Scrollbar bgmScroll;

    public SettingWindow()
    {

    }

    public override void CloseWindow()
    {
        base.CloseWindow();
    }

    public override void ShowWindow()
    {
        base.ShowWindow();
    }

    private void Start()
    {
        CloseWindow();
    }

    private void FixedUpdate()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, endScale, restoreSpeed);
    }

    public void SetEffectVolume()
    {
        SoundManager.instance.SetEffectVolume(effectScroll.value);
    }

    public void SetBGVolume()
    {
        SoundManager.instance.SetBGVolume(bgmScroll.value);
    }
}
