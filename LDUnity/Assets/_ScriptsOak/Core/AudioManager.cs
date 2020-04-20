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
    public AudioSource SourceMusic1;
    [SerializeField]
    public AudioSource SourceMusic2;
    [SerializeField]
    AudioSource SourceVoice;
    [SerializeField]
    public AudioSource SourceAmbience1;
    [SerializeField]
    public AudioSource SourceAmbience2;
    [SerializeField]
    AudioSource SourceFootsteps;
    [SerializeField]
    public AudioSource SourceBodyDrag;

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

    public List<AudioClip> ClipsSurgeryCut = new List<AudioClip>();
    public List<AudioClip> ClipsSurgeryDrop = new List<AudioClip>();
    public List<AudioClip> ClipsSurgeryReplace = new List<AudioClip>();


    public List<AudioClip> ClipsPickUpCorpse = new List<AudioClip>();

    public AudioClip ClipCorpseDrop;

    public List<AudioClip> ClipsStairs = new List<AudioClip>();
    public AudioClip ClipDoorClose;
    public List<AudioClip> ClipsFootstepsPlayer = new List<AudioClip>();
    public List<AudioClip> ClipsFootstepsNPC = new List<AudioClip>();

    public AudioClip ClipHideBarrelIn;
    public AudioClip ClipHideBarrelOut;
    public AudioClip ClipHideBoxIn;
    public AudioClip ClipHideBoxOut;
    public AudioClip ClipHideBushIn;
    public AudioClip ClipHideBushOut;
    public AudioClip ClipHideClosetIn;
    public AudioClip ClipHideClosetOut;
    public AudioClip ClipHideSewerIn;
    public AudioClip ClipHideSewerOut;

    public AudioClip ClipAtmoStreet;
    public AudioClip ClipAtmoCanal;
    public AudioClip ClipAtmoIndoors;

    public override void InitManager()
    {
        base.InitManager();


    }

    public void SetFootstepsOn(bool isOn)
    {
        SourceFootsteps.mute = !isOn;
    }

    public void PlaySoundOneShot(AudioClip clip, float range = 0, float volume = 1.0f)
    {
    	SourceOneShot.pitch = 1+range*(Random.value-0.5f);
        SourceOneShot.PlayOneShot(clip, volume);
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

    public void SwitchAtmosphere(EStreetAtmoType atmoType)
    {

        AudioClip newAtmo = null;

        switch (atmoType)
        {
            case EStreetAtmoType.Street:
                newAtmo = ClipAtmoStreet;
                break;
            case EStreetAtmoType.Indoors:
                newAtmo = ClipAtmoIndoors;
                break;
            case EStreetAtmoType.Canal:
                newAtmo = ClipAtmoCanal;
                break;
        }

        if (SourceAmbience1.clip == newAtmo || SourceAmbience2.clip == newAtmo)
        {
            return;
        }

        if (bIsCrossfadingAtmo)
        {
            QueuedCrossfadeClip = newAtmo;
        }
        else
        {
            StartCoroutine(CrossfadeAtmoRoutine(newAtmo));
            bIsCrossfadingAtmo = true;
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
    bool bIsCrossfadingAtmo = false;

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

            SourceMusic1.volume = 1.0f - alpha * 0.5f;
            SourceMusic2.volume = Mathf.Min(1.0f, alpha) * 0.5f;
            yield return null;
        }
        SourceMusic1.Stop();
        SourceMusic1.clip = newMusic;
        SourceMusic1.time = SourceMusic2.time;
        SourceMusic1.volume = 1.0f * 0.5f;
        SourceMusic1.Play();

        SourceMusic2.mute = true;
        SourceMusic2.Stop();

        bIsCrossfading = false;
    }

    IEnumerator CrossfadeAtmoRoutine(AudioClip newMusic)
    {
        float alpha = 0.0f;

        SourceAmbience2.volume = 0.0f;
        SourceAmbience2.mute = false;
        SourceAmbience2.clip = newMusic;
        SourceAmbience2.Play();
        SourceAmbience2.loop = true;

        while (alpha < 1.0f)
        {
            alpha += Time.deltaTime * 1.0f;

            SourceAmbience1.volume = 1.0f - alpha * 0.8f;
            SourceAmbience2.volume = Mathf.Min(1.0f, alpha) * 0.8f;
            yield return null;
        }
        SourceAmbience1.Stop();
        SourceAmbience1.clip = newMusic;
        SourceAmbience1.time = SourceAmbience2.time;
        SourceAmbience1.volume = 1.0f * 0.8f;
        SourceAmbience1.Play();
        SourceAmbience2.mute = true;
        SourceAmbience2.Stop();

        bIsCrossfadingAtmo = false;
    }

    public void PlayVoiceLine(AudioClip line)
    {
        SourceVoice.Stop();
        SourceVoice.clip = line;
        SourceVoice.loop = false;
        SourceVoice.Play();
    }
}
