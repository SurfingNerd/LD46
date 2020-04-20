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
    AudioSource SourceMusic1;
    [SerializeField]
    AudioSource SourceMusic2;
    [SerializeField]
    AudioSource SourceVoice;
    [SerializeField]
    AudioSource SourceAmbience;
    [SerializeField]
    AudioSource SourceFootsteps;

    [SerializeField]
    public AudioClip ClipMusicWander;
    [SerializeField]
    public AudioClip ClipMusicChase;
    [SerializeField]
    public AudioClip ClipMusicSurgery;

    [SerializeField]
    public AudioClip ClipEffectTransition;

    [SerializeField]
    public List<AudioClip> ListClipMonologue = new List<AudioClip>();

    [SerializeField]
    public List<AudioClip> ListClipUWot = new List<AudioClip>();


    AudioClip QueuedCrossfadeClip = null;

    public AudioClip ClipOpening;
    public AudioClip ClipFinale;

    public List<AudioClip> ClipsCarryingCorpse = new List<AudioClip>();
    public List<AudioClip> ClipsNPCBarks = new List<AudioClip>();
    public List<AudioClip> ClipsNonViable = new List<AudioClip>();
    public List<AudioClip> ClipsViable = new List<AudioClip>();


    public AudioClip ClipSurgeryFace;
    public AudioClip ClipSurgeryLegs;
    public AudioClip ClipSurgeryArms;
    public AudioClip ClipSurgeryTorso;
    public AudioClip ClipSurgeryHair;

    public AudioClip ClipSurgeryCut;


    public override void InitManager()
    {
        base.InitManager();


    }

    public void SetFootstepsOn(bool isOn)
    {
        SourceFootsteps.mute = !isOn;
    }

    public void PlaySoundOneShot(AudioClip clip, float range = 0)
    {
    	SourceOneShot.pitch = 1+range*(Random.value-0.5f);
        SourceOneShot.PlayOneShot(clip);
    }

    public void SwitchMusic(AudioClip newMusic)
    {
        if (SourceMusic1.clip == newMusic || SourceMusic2.clip == newMusic)
        {
            return;
        }

        if (bIsCrossfading)
        {
            QueuedCrossfadeClip = newMusic;
        }
        else
        {
            StartCoroutine(CrossfadeMusicRoutine(newMusic));
            bIsCrossfading = true;
        }
    }

    private void Update()
    {
        if (QueuedCrossfadeClip && !bIsCrossfading)
        {
            StartCoroutine(CrossfadeMusicRoutine(QueuedCrossfadeClip));
            bIsCrossfading = true;
            QueuedCrossfadeClip = null;
        }
    }

    bool bIsCrossfading = false;

    IEnumerator CrossfadeMusicRoutine(AudioClip newMusic)
    {
        float alpha = 0.0f;

        SourceMusic2.volume = 0.0f;
        SourceMusic2.mute = false;
        SourceMusic2.clip = newMusic;
        SourceMusic2.Play();
        SourceMusic2.loop = true;

        while (alpha < 1.0f)
        {
            alpha += Time.deltaTime * 1.0f;

            SourceMusic1.volume = 1.0f - alpha;
            SourceMusic2.volume = Mathf.Min(1.0f, alpha);
            yield return null;
        }
        SourceMusic1.Stop();
        SourceMusic1.clip = newMusic;
        SourceMusic1.time = SourceMusic2.time;
        SourceMusic1.volume = 1.0f;
        SourceMusic1.Play();

        SourceMusic2.mute = true;
        SourceMusic2.Stop();

        bIsCrossfading = false;
    }

    public void PlayVoiceLine(AudioClip line)
    {
        SourceVoice.Stop();
        SourceVoice.clip = line;
        SourceVoice.loop = false;
        SourceVoice.Play();
    }
}
