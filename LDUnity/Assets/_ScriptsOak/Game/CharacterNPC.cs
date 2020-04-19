using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBodyPart
{
    Eyes,
    Hair,
    Torso,
    Arm,
    Leg
}

public enum EAction
{
    Idle,
    Wander,
    Busy,
    MAX,
}

public enum ENPCStatus
{
    Neutral,
    Alert,
    Alarmed,
    Aggressive,
}

public class CharacterNPC : Character
{
    [SerializeField]
    EBodyPart DesiredPart;

    [SerializeField]
    float ThinkCooldownMin;
    [SerializeField]
    float ThinkCooldownMax;

    [SerializeField]
    public SpriteRenderer TooltipRenderer;

    [SerializeField]
    Sprite AlertIcon;
    [SerializeField]
    Sprite AlarmedIcon;


    ENPCStatus CurrentStatus;
    EAction CurrentAction;

    float CurrentThinkCooldown = 0.0f;
    float CurrentTaskDuration = 0.0f;

    bool bIsDying = false;

    public void SetCurrentAction(EAction action)
    {
        CurrentAction = action;
    }

    public override void MoveCharacter()
    {
        base.MoveCharacter();
    }

    public override void InitCharacter()
    {
        SetStatus(ENPCStatus.Neutral);
        SetCurrentAction(EAction.Idle);
    }

    public void SetStatus(ENPCStatus status)
    {
        if(CurrentStatus == status)
        {
            return;
        }
        CurrentStatus = status;

        switch (CurrentStatus)
        {
            case ENPCStatus.Neutral:
                TooltipRenderer.sprite = null;
                break;
            case ENPCStatus.Alert:
                TooltipRenderer.sprite = AlertIcon;
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ListClipUWot[0]);
                }
                break;
            case ENPCStatus.Alarmed:
                TooltipRenderer.sprite = AlarmedIcon;
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ListClipUWot[0]);
                }
                break;
            case ENPCStatus.Aggressive:
                TooltipRenderer.sprite = AlarmedIcon;
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ListClipUWot[0]);
                }
                break;
        }
    }

    public void HandleGetStabbed()
    {
        if(bIsDying)
        {
            return;
        }


        //TODO: remove red bs
        GetComponent<SpriteAnimator>().SetColor(Color.red);

        Debug.Log(this + " is getting killed and will drop: " + DesiredPart);

        bIsDying = true;
        CurrentDirection = Vector3.zero;

        //Instantiate(BodyPartManager.Instance.GetBodyPartTemplateByType(DesiredPart), 
        //    gameObject.transform.position, 
        //    new Quaternion(), 
        //    gameObject.transform.parent);

        Instantiate(BodyPartManager.Instance.GetRandomCorpseTemplate(),
            gameObject.transform.position,
            new Quaternion(),
            gameObject.transform.parent);

        Destroy(gameObject);
    }

    public override void Tick()
    {
        base.Tick();

        if(bIsDying)
        {
            return;
        }

        //CurrentAction = EAction.Idle;
        CurrentTaskDuration -= Time.deltaTime;

        if(CurrentTaskDuration >= 0.0f || CurrentAction == EAction.Busy)
        {

        }
        else
        {
            CurrentThinkCooldown -= Time.deltaTime;
            if (CurrentThinkCooldown <= 0.0f)
            {
                Think();
            }
        }

        bool foundACorpse = false;
        bool foundPlayerCarryingCorpse = false;

        if (GetComponent<StreetSpriteSort>().street == CharacterPlayer.instance.gameObject.GetComponent<StreetSpriteSort>().street)
        {
            var distance = Vector3.Distance(gameObject.transform.localPosition, CharacterPlayer.instance.gameObject.transform.localPosition);

            if (distance < EntityManager.Instance.npcCorpseDetectionDistance)
            {
                if (CharacterPlayer.instance.GetCurrentCorpse() != null && !CharacterPlayer.instance.IsHiding())
                {
                    SetCurrentAction(EAction.Busy);
                    if ((CharacterPlayer.instance.transform.position - transform.position).normalized.x < 0)
                    {
                        SetCurrentDirection(EDirection.Left);
                    }
                    else
                    {
                        SetCurrentDirection(EDirection.Right);
                    }

                    if (distance < EntityManager.Instance.npcCorpseDetectionDistance / 4)
                    {
                        CharacterPlayer.instance.HandleGetCaught();
                        SetStatus(ENPCStatus.Aggressive);
                    }
                    else if (distance < EntityManager.Instance.npcCorpseDetectionDistance / 2)
                    {
                        SetStatus(ENPCStatus.Alarmed);
                    }
                    else
                    {
                        SetStatus(ENPCStatus.Alert);
                    }
                    foundPlayerCarryingCorpse = true;
                }
                else if (CharacterPlayer.instance.IsHiding())
                {
                    SetStatus(ENPCStatus.Neutral);
                    SetCurrentAction(EAction.Idle);
                }
                ActivateFoundCorpseText(foundACorpse);

                if (AudioManager.Instance != null)
                {
                    if (foundPlayerCarryingCorpse)
                    {
                        AudioManager.Instance.SwitchMusic(AudioManager.Instance.ClipMusicChase);
                        //Debug.Log("Player is seen carrying corpse by: " + npc);
                    }
                    else
                    {
                        AudioManager.Instance.SwitchMusic(AudioManager.Instance.ClipMusicWander);
                    }
                }
                //Debug.LogWarning("Corpse detected!! Distance: " + distance);
            }
            
        }

    }

    public void Think()
    {
        CurrentAction = (EAction)Random.Range(0, (int)EAction.Busy);

        switch (CurrentAction)
        {
            case EAction.Idle:
                CurrentDirection = Vector3.zero;
                break;
            case EAction.Wander:
                int trolo = Random.Range(0, 2);
                if (trolo == 0)
                {
                    SetCurrentDirection(EDirection.Left);
                }
                else
                {
                    SetCurrentDirection(EDirection.Right);
                }
                break;
            case EAction.Busy:
                break;
            case EAction.MAX:
                Debug.LogError("MASSIVE SHITSPLOSION FIX NOW OTT PLS");
                break;
        }
        CurrentTaskDuration = Random.Range(1.0f, 2.5f);

    }

    public void ActivateFoundCorpseText(bool value)
    {
        var foundCorpse = this.transform.Find("found_corpse");
        if(foundCorpse != null)
        {
            foundCorpse.gameObject.SetActive(value);
        }
    }

    public void Interact()
    {
        //throw new System.NotImplementedException();
    }

    public Sprite GetInteractIcon()
    {
        return null;
        //throw new System.NotImplementedException();
    }
}
