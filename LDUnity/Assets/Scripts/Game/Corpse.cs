using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDecayLevel
{
    Fresh,
    Medium,
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
    EDecayLevel DecayLevel = EDecayLevel.Fresh;

    [SerializeField]
    float DecayRate = 0.01f;

    float Decay = 0.0f;

    bool bIsInspected = false;

    public void AdvanceDecay()
    {
        Decay += Time.deltaTime * DecayRate;
        if (Decay < 0.3f)
        {
            DecayLevel = EDecayLevel.Fresh;
        }
        else if (Decay < 0.9f)
        {
            DecayLevel = EDecayLevel.Medium;
        }
        else
        {
            DecayLevel = EDecayLevel.WellDone;
        }
        Debug.Log(this + " Decay: " + Decay);
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
        if(bIsInspected)
        {
            if(DecayLevel != EDecayLevel.WellDone)
            {
                if (CharacterPlayer.instance.GetCurrentCorpse() == null)
                {
                    //check if there is a corpse.
                    if (CharacterPlayer.instance.GetCurrentCorpse() != null)
                    {
                        throw new System.NotImplementedException("Holding already a Corpse");
                    }

                    isHidden = false;
                    CharacterPlayer.instance.SetCurrentCorpse(this);

                    // using parenting here for moving corpse.
                    // might be suboptimal for animation.
                    transform.SetParent(CharacterPlayer.instance.transform);
                }
            }
        }
        else
        {
            bIsInspected = !bIsInspected;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (DecayLevel)
        {
            case EDecayLevel.Fresh:
                Decay = 0.0f;
                break;
            case EDecayLevel.Medium:
                Decay = 0.3f;
                break;
            case EDecayLevel.WellDone:
                Decay = 0.9f;
                break;
        }
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
        if(bIsInspected)
        {
            return EPlayerAction.PickUp;

        }
        else
        {
            return EPlayerAction.Inspect;
        }
    }
}
