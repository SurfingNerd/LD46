using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EBodyPart
{
    Eyes,
    Hair,
    Torso,
    Arm,
    Leg
}

/*
public enum EAction
{
    Idle,
    Wander,
    Following,
    MAX,
}
*/

public enum ENPCStatus
{
    Neutral,
    MoveToNewPosition,
    Alert,
    Alarmed,
    Sleeping
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
    float SleepinessIncrease = 0.01f; 

    [SerializeField]
    public SpriteRenderer TooltipRenderer;

    [SerializeField]
    Sprite AlertIcon;
    [SerializeField]
    Sprite AlarmedIcon;

    [SerializeField] 
    private Sprite SleepingIcon;

    ENPCStatus CurrentStatus;
    //EAction CurrentAction;

    float CurrentThinkCooldown = 0.0f;
    float CurrentTaskDuration = 0.0f;

    bool bIsDying = false;

    float currentSleepiness = 0.0f;
    

    /*public void SetCurrentAction(EAction action)
    {
        CurrentAction = action;
    }*/

    public override void MoveCharacter()
    {
        base.MoveCharacter();
    }

    public override void InitCharacter()
    {
        SetStatus(ENPCStatus.Neutral);
        // SetCurrentAction(EAction.Idle);
    }

    public void SetStatus(ENPCStatus status)
    {
        CurrentStatus = status;

        switch (CurrentStatus)
        {
            case ENPCStatus.Neutral:
                TooltipRenderer.sprite = null;
                break;
            case ENPCStatus.Alert:
                TooltipRenderer.sprite = AlertIcon;
                break;
            case ENPCStatus.Alarmed:
                TooltipRenderer.sprite = AlarmedIcon;
                break;
            case ENPCStatus.Sleeping:
                TooltipRenderer.sprite = SleepingIcon;
                break;
            case ENPCStatus.MoveToNewPosition:
                TooltipRenderer.sprite = null;
                break;
            default:
                Debug.LogError("No Icon for State: " + status);
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

        if (CurrentStatus != ENPCStatus.Sleeping)
        {
            currentSleepiness += (SleepinessIncrease * Time.deltaTime);
        }

        //CurrentAction = EAction.Idle;
        CurrentTaskDuration -= Time.deltaTime;
        
        Debug.Log("state: " + CurrentStatus + "" +
                  "CurrentTaskDuration: " + CurrentTaskDuration.ToString("##.##") + " - " + currentSleepiness.ToString("#.###"));

        //if(CurrentTaskDuration >= 0.0f || CurrentAction == EAction.Following)
        if(CurrentTaskDuration >= 0.0f)
        {
            //Debug.Log("current Action: " + CurrentAction);
        }
        else
        {
            CurrentThinkCooldown -= Time.deltaTime;
            if (CurrentThinkCooldown <= 0.0f)
            {
                Think();
            }
        }
    }

    public void Think()
    {
        //if we are sleepy, we fall asleep.
        if (currentSleepiness > 1)
        {
            //CurrentAction = EAction.Idle;
            CurrentTaskDuration = 5f;
            currentSleepiness = 0;
            SetCurrentDirection(EDirection.Neutral);
            SetStatus(ENPCStatus.Sleeping);
            //Debug.LogError("Sleeeping!!");
        }
        else
        {
            //randomly decide on stay here or to walk to a new Position.
            int trolo = Random.Range(0, 2);
            if (trolo == 0)
            {
                SetCurrentDirection(EDirection.Left);
                SetStatus(ENPCStatus.MoveToNewPosition);
            }
            else
            {
                SetCurrentDirection(EDirection.Right);
                SetStatus(ENPCStatus.MoveToNewPosition);
            }
            CurrentTaskDuration = Random.Range(1.0f, 2.5f);
        }
        

        //CurrentAction = (EAction)Random.Range(0, (int)EAction.MAX);

        /*
        switch (CurrentAction)
        {
            case EAction.Idle:
                CurrentDirection = Vector3.zero;
                break;
            case EAction.Wander:

                break;
            case EAction.Following:
                break;
            case EAction.MAX:
                Debug.LogError("MASSIVE SHITSPLOSION FIX NOW OTT PLS");
                break;
        }
        */
        

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

    public bool IsAbleToSee()
    {
        return this.CurrentStatus != ENPCStatus.Sleeping;
    }
}
