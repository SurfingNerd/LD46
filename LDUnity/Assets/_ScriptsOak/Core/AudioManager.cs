using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ManagerBase
{
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    [SerializeField]
    AudioSource SourceOneShot;
    [SerializeField]
    AudioSource SourceMusic;
    [SerializeField]
    AudioSource SourceShipEngine;
    [SerializeField]
    AudioSource SourceShipWheel;
    [SerializeField]
    AudioSource SourceAmbience;

    [SerializeField]
    AudioClip ThemeMusicClip;

    [SerializeField]
    public AudioClip SplishClip;

    [SerializeField]
    public AudioClip ShipSplishClip;

    [SerializeField]
    public AudioClip InsertClip;

    public override void InitManager()
    {
        base.InitManager();

        PlayMusic(ThemeMusicClip);
    }

    public void PlaySound(AudioClip clip)
    {
        SourceOneShot.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        SourceMusic.Stop();
        SourceMusic.clip = musicClip;
        SourceMusic.loop = true;
        SourceMusic.Play();
    }

    public void ToggleWaterWheelSound(bool active)
    {
        if(active)
        {
            if (!SourceShipWheel.isPlaying)
            {
                SourceShipWheel.Play();
                SourceOneShot.PlayOneShot(ShipSplishClip);
            }
        }
        else
        {
            SourceShipWheel.Stop();
        }
    }
}
