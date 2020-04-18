using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer Renderer;
    [SerializeField]
    List<Sprite> SpriteList = new List<Sprite>();
    [SerializeField]
    float AnimRate = 0.125f;
    [SerializeField]
    bool DestroyOnFinish = false;

    int CurrentAnimationSpriteIndex = 0;
    float AnimTime = 0.0f;
    float BlinkTime = 0.0f;

    bool bIsBlinking = false;
    bool blinkSwitch = false;

    Color BlinkColor = Color.yellow;
    Color CurrentColor;

    // Start is called before the first frame update
    void Start()
    {
        CurrentColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if(SpriteList != null && SpriteList.Count > 0)
        {
            AnimTime += Time.deltaTime;
            if (AnimTime >= AnimRate)
            {
                PlayAnimation();
                AnimTime = 0.0f;
            }
            if(bIsBlinking)
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
        Renderer.sprite = SpriteList[CurrentAnimationSpriteIndex];
        CurrentAnimationSpriteIndex++;
        if (CurrentAnimationSpriteIndex >= SpriteList.Count)
        {
            CurrentAnimationSpriteIndex = 0;
            if(DestroyOnFinish)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetSpriteList(List<Sprite> spriteList)
    {
        SpriteList = spriteList;
        CurrentAnimationSpriteIndex = 0;
        AnimTime = 0.0f;
        if(SpriteList == null || SpriteList.Count == 0)
        {
            Renderer.sprite = null;
        }
        else
        {
            Renderer.sprite = SpriteList[0];
        }
    }

    public void SetColor(Color color)
    {
        if(Renderer != null)
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
