using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioSource EffectSource;

    public static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;

        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.Stop();
        MusicSource.clip = clip;
        MusicSource.Play();
        MusicSource.loop = true;
    }

    public void PlayEffect(AudioClip clip)
    {
        EffectSource.clip = clip;
        EffectSource.Play();
        EffectSource.loop = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="percent">0.0 ~ 1.0</param>
    public void SetEffectVolume(float percent)
    {
        EffectSource.volume = percent;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="percent">0.0 ~ 1.0</param>
    public void SetBGVolume(float percent)
    {
        MusicSource.volume = percent;
    }
}
