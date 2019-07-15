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
    }
}
