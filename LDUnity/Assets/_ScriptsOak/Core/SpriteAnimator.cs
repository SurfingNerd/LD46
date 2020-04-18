using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAnimation
{
    Idle,
    Move,
    AlleyTransition,
    Murder,
    PickUp,
}

[System.Serializable]
public class AnimationDefinition
{
    [SerializeField]
    public EAnimation Type;
    [SerializeField]
    public List<Sprite> SpriteList = new List<Sprite>();
    [SerializeField]
    public float AnimRate = 0.125f;
    [SerializeField]
    public List<AudioClip> ClipList = new List<AudioClip>();

    public AnimationDefinition()
    {
        AnimRate = 0.125f;
    }
}

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer Renderer;
    [SerializeField]
    public List<AnimationDefinition> AnimList = new List<AnimationDefinition>();
    [SerializeField]
    bool DestroyOnFinish = false;

    public AnimationDefinition CurrentAnim;

    int CurrentAnimationSpriteIndex = 0;
    float AnimTime = 0.0f;
    float BlinkTime = 0.0f;

    bool bIsBlinking = false;
    bool blinkSwitch = false;



    Color BlinkColor = Color.yellow;
    Color CurrentColor;

    bool bRevertOnFinish;

    AnimationDefinition CachedAnim;

    // Start is called before the first frame update
    void Start()
    {
        CurrentColor = Color.white;
        if (CurrentAnim == null || CurrentAnim.SpriteList.Count == 0)
        {
            SetAnimation(AnimList[0].Type, false, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AnimList != null && AnimList.Count > 0)
        {
            AnimTime += Time.deltaTime;
            if (AnimTime >= CurrentAnim.AnimRate)
            {
                PlayAnimation();
                AnimTime = 0.0f;
            }
            if (bIsBlinking)
            {
                BlinkTime += Time.deltaTime;
                if (BlinkTime >= 0.5f)
                {
                    blinkSwitch = !blinkSwitch;
                    BlinkTime = 0.0f;
                    if (blinkSwitch)
                    {
                        if (Renderer != null)
                        {
                            Renderer.material.SetColor("_Color", CurrentColor);
                        }
                    }
                    else
                    {
                        if (Renderer != null)
                        {
                            Renderer.material.SetColor("_Color", BlinkColor);
                        }
                    }
                }
            }
        }
    }

    public void PlayAnimation()
    {
        CurrentAnimationSpriteIndex++;
        if (CurrentAnimationSpriteIndex >= CurrentAnim.SpriteList.Count)
        {
            CurrentAnimationSpriteIndex = 0;
            if (DestroyOnFinish)
            {
                Destroy(gameObject);
            }
            if (bRevertOnFinish)
            {
                CurrentAnim = CachedAnim;
                bRevertOnFinish = false;
            }
        }
        Renderer.sprite = CurrentAnim.SpriteList[CurrentAnimationSpriteIndex];
        if (CurrentAnim.ClipList.Count > 0)
        {
            AudioManager.Instance.PlaySound(CurrentAnim.ClipList[Random.Range(0, CurrentAnim.ClipList.Count)]);
        }
    }

    public float GetAnimDuration(EAnimation anim)
    {
        for (int i = 0; i < AnimList.Count; ++i)
        {
            if (AnimList[i].Type == anim)
            {
                return AnimList[i].SpriteList.Count * AnimList[i].AnimRate;
            }
        }
        return 0.0f;
    }

    public void SetAnimation(EAnimation anim, bool invertX, bool revertOnFinish)
    {
        for (int i = 0; i < AnimList.Count; ++i)
        {
            if (AnimList[i].Type == anim)
            {
                SetSpriteList(AnimList[i], invertX, revertOnFinish);
                return;
            }
        }
        CurrentAnim = AnimList[0];
        Debug.LogError("ANIM SPRITES NOT FOUND: " + anim);
    }

    public void SetSpriteList(AnimationDefinition animDef, bool invertX = false, bool revertOnFinish = false)
    {
        bool changedX = invertX ^ Renderer.flipX;
        CachedAnim = CurrentAnim;
        bRevertOnFinish = revertOnFinish;
        if (CurrentAnim == animDef && !changedX)
        {
            return;
        }
        Renderer.flipX = invertX;

        CurrentAnim = animDef;
        CurrentAnimationSpriteIndex = 0;
        AnimTime = 0.0f;
        if (AnimList == null || AnimList.Count == 0)
        {
            Renderer.sprite = null;
        }
        else
        {
            Renderer.sprite = CurrentAnim.SpriteList[0];
        }
    }

    public void SetColor(Color color)
    {
        if (Renderer != null)
        {
            CurrentColor = color;
            Renderer.material.SetColor("_Color", color);
        }
    }

    public void ToggleAnimator(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetBlinking(bool isBlinking, Color color = new Color())
    {
        BlinkColor = color;
        bIsBlinking = isBlinking;
        SetColor(CurrentColor);
    }
}
