using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EDecayLevel
{
    Fresh,
    Medium,
    AlmostDone,
    WellDone
}

public class Corpse : MonoBehaviour, IInteractable
{

    public bool isHidden = false;

    [SerializeField]
    Sprite InteractIconInspect;
    [SerializeField]
    Sprite InteractIconViable;
    [SerializeField]
    Sprite InteractIconNonViable;

    [SerializeField]
    public EDecayLevel DecayLevel = EDecayLevel.Fresh;

    [SerializeField]
    public SpriteRenderer Rendy;

    [SerializeField]
    Sprite DragSprite;

    [SerializeField]
    public Sprite DroppedSprite;

    [SerializeField]
    float DecayRate = 0.01f;

    [SerializeField]
    float Decay = 0.0f;

    bool bIsInspected = false;

    bool bHasClipPlayedInspect = false;

    bool bHasClipPlayedMonologue = false;

    public void AdvanceDecay()
    {
        Decay = Mathf.Clamp(Decay + Time.deltaTime * DecayRate, 0.0f, 1.0f);
        HUD.Instance.SetProgressBarProgressDecay(Decay);

        DecayLevel = DecayValueToDecayLevel(Decay);
    }

    public Sprite GetInteractIcon()
    {
        if (bIsInspected)
        {
            switch (DecayLevel)
            {
                case EDecayLevel.Fresh:
                    return InteractIconViable;
                case EDecayLevel.Medium:
                    return InteractIconViable;
                case EDecayLevel.AlmostDone:
                    return InteractIconViable;
                case EDecayLevel.WellDone:
                    return InteractIconNonViable;
            }
        }
        else
        {
            return InteractIconInspect;
        }

        Debug.LogError("SHITSPLOSION TELL OTT TO FIX");
        return null;
    }

    public void Interact()
    {
        if (bIsInspected)
        {
            if (CharacterPlayer.instance.GetCurrentCorpse() == null)
            {
                if (DecayLevel != EDecayLevel.WellDone)
                {

                    //check if there is a corpse.
                    if (CharacterPlayer.instance.GetCurrentCorpse() != null)
                    {
                        throw new System.NotImplementedException("Holding already a Corpse");
                    }

                    isHidden = false;
                    CharacterPlayer.instance.SetCurrentCorpse(this);
                    AudioManager.Instance.PlaySoundOneShot(AudioManager.Instance.ClipsPickUpCorpse[Random.Range(0, AudioManager.Instance.ClipsPickUpCorpse.Count)]);

                    Rendy.sprite = DragSprite;

                    if (!bHasClipPlayedMonologue)
                    {
                        AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipsCarryingCorpse[Random.Range(0, AudioManager.Instance.ClipsCarryingCorpse.Count)]);
                        bHasClipPlayedMonologue = true;
                    }
                    
                }
                else
                {
                    
                }
            }
            else
            {

            }
        }

        else
        {
            bIsInspected = !bIsInspected;

            if(DecayLevel != EDecayLevel.WellDone)
            {
                if (!bHasClipPlayedInspect)
                {
                    AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipsViable[Random.Range(0, AudioManager.Instance.ClipsViable.Count)]);
                    bHasClipPlayedInspect = true;
                }
            }
            else
            {
                if (!bHasClipPlayedInspect)
                {
                    AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipsNonViable[Random.Range(0, AudioManager.Instance.ClipsNonViable.Count)]);
                    bHasClipPlayedInspect = true;
                }
            }
        }
    }

            
    float TresholdMedium = 0.3f;
    float TreshholdAlmostDone = 0.6f;
    float TreshholdWellDone = 0.99f;
    
    // Start is called before the first frame update
    void Start()
    {
        Decay = DecayLevelToDecayValue(DecayLevel);

        
        switch (GameManager.Instance.Difficulty)
        {
            case GameManager.GameDifficulty.Easy:
                TresholdMedium = 0.2f;
                TreshholdAlmostDone = 0.6f;
                break;
            case GameManager.GameDifficulty.Normal:
                TresholdMedium = 0.2f;
                TreshholdAlmostDone = 0.6f;
                break;
            case GameManager.GameDifficulty.Hard:
                TresholdMedium = 0.2f;
                TreshholdAlmostDone = 0.6f;
                break;
        }
    }

    public EDecayLevel DecayValueToDecayLevel(float value)
    {
        if (value < TresholdMedium)
        {
            return EDecayLevel.Fresh;
        }
        
        if (value < TreshholdAlmostDone)
        {
            return EDecayLevel.Medium;
        }
        
        if (value < TreshholdWellDone)
        {
            return EDecayLevel.AlmostDone;
        }
        
        return EDecayLevel.WellDone;
    }
    
    public float DecayLevelToDecayValue(EDecayLevel level)
    {
        switch (level)
        {
            case EDecayLevel.Fresh:
                return 0;
            case EDecayLevel.Medium:
                return TresholdMedium;
            case EDecayLevel.AlmostDone:
                return TreshholdAlmostDone;
            case EDecayLevel.WellDone:
                return TreshholdWellDone;
        }

        throw new NotImplementedException("No Implementation for this decay and difficulty.");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public EPlayerAction GetPlayerActionType()
    {
        if (bIsInspected)
        {
            return EPlayerAction.PickUp;

        }
        else
        {
            return EPlayerAction.Inspect;
        }
    }

    public int GetStreetSpriteSortIndex()
    {
        StreetSpriteSort sort = GetComponent<StreetSpriteSort>();

        if (sort != null)
        {
            return sort.street;
        }
        else
        {
            return 0;
        }
    }
}
