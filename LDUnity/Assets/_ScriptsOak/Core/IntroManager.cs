using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : ManagerBase
{
    public static IntroManager instance;

    float IntroDelay = 0.0f;

    public bool bIntroDone = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(!bIntroDone)
        {
            IntroDelay -= Time.deltaTime;
            if(IntroDelay <= 0.0f)
            {
                bIntroDone = true;
                HUD.Instance.HideIntro();
                AudioManager.Instance.SwitchMusic(AudioManager.Instance.ClipMusicWander);
            }
        }
    }

    public void SkipIntro()
    {
        IntroDelay = 0.0f;
    }

    public override void InitManager()
    {
        base.InitManager();
        IntroDelay = AudioManager.Instance.ClipOpening.length;
        AudioManager.Instance.PlaySoundOneShot(AudioManager.Instance.ClipOpening);

        HUD.Instance.ShowIntro();

    }
}
