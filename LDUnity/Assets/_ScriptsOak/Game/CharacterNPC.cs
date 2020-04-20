using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EBodyPart
{
    Face,
    Hair,
    LeftLeg,
    RightLeg,
    LeftArm,
    RightArm,
    Torso
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
    Sleeping,
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
    float SleepinessIncrease = 0.01f;

    [SerializeField]
    public float currentSleepiness = 0.0f;

    [SerializeField]
    public float AlarmedDuration = 8f;

    public bool harmlessNPCCheat = false;

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


    private Vector3? lastKnownPosition;

    //last known alley this npc has seen the player fleeing.
    private Alley lastKnownFleeAlley;

    /*public void SetCurrentAction(EAction action)
    {
        CurrentAction = action;
    }*/

    public override void MoveCharacter()
    {
        base.MoveCharacter();

        if(CurrentDirection.x != 0)
        {
            FootstepDelay -= Time.deltaTime;

            if(FootstepDelay <= 0.0f && IntroManager.instance.bIntroDone)
            {
                FootstepDelay = 0.7f;

                AudioManager.Instance.PlaySoundOneShot(AudioManager.Instance.ClipsFootstepsNPC[Random.Range(0, AudioManager.Instance.ClipsFootstepsNPC.Count)], 0.0f, 0.2f);
            }
        }
    }

    public override void InitCharacter()
    {
        SetStatus(ENPCStatus.Neutral);
        // SetCurrentAction(EAction.Idle);
    }

    public void SetStatus(ENPCStatus status)
    {
        if (CurrentStatus == status)
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
                    AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipsNPCBarks[Random.Range(0, AudioManager.Instance.ClipsNPCBarks.Count)]);
                    CharacterAnimator.SetAnimation(EAnimation.Alert, bShouldInvertX, true);
                }
                break;
            case ENPCStatus.Alarmed:
                TooltipRenderer.sprite = AlarmedIcon;
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipsNPCBarks[Random.Range(0, AudioManager.Instance.ClipsNPCBarks.Count)]);
                    CharacterAnimator.SetAnimation(EAnimation.Alert, bShouldInvertX, true);
                }
                break;
            case ENPCStatus.Aggressive:
                TooltipRenderer.sprite = AlarmedIcon;
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipsNPCBarks[Random.Range(0, AudioManager.Instance.ClipsNPCBarks.Count)]);
                }
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
        if (bIsDying)
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

    public bool CheckPlayerIsInVision(out float distance)
    {
        distance = Vector3.Distance(CharacterPlayer.instance.transform.position, this.transform.position);
        bool result = this.CurrentStatus != ENPCStatus.Sleeping
                      && distance < EntityManager.Instance.npcCorpseDetectionDistance
                      && !CharacterPlayer.instance.IsHiding()
                      && CharacterPlayer.instance.GetCurrentStreet() == this.GetCurrentStreet();

        //if (result)
        //{
        //check if they are also on the same street level.
        //}

        return result;
    }

    public override void Tick()
    {
        base.Tick();

        if (bIsDying || IntroManager.instance == null || !IntroManager.instance.bIntroDone)
        {
            return;
        }

        if (CurrentStatus != ENPCStatus.Sleeping)
        {
            currentSleepiness += (SleepinessIncrease * Time.deltaTime);
        }

        //CurrentAction = EAction.Idle;
        CurrentTaskDuration -= Time.deltaTime;

        Log("state: " + CurrentStatus + "" +
            "CurrentTaskDuration: " + CurrentTaskDuration.ToString("##.##") + " - " +
            currentSleepiness.ToString("#.###"));

        float distance = 0;
        bool isInVision = CheckPlayerIsInVision(out distance);


        if (CurrentStatus == ENPCStatus.Alarmed || CurrentStatus == ENPCStatus.Alert || CurrentStatus == ENPCStatus.Aggressive)
        {
            if (!lastKnownPosition.HasValue && lastKnownFleeAlley == null)
            {
                //lost all tracks, where is she ?
                Debug.LogWarning("Inconsistent State: no last knownPosition. setting NPC Status back to Neutral.");
                SetStatus(ENPCStatus.Neutral);
                return;
            }

            if (isInVision)
            {
                lastKnownPosition = CharacterPlayer.instance.transform.position;
                lastKnownFleeAlley = null; // we saw the player, we don't need to remember potential fleeAlley.

                //we see here, don't give up.
                this.CurrentTaskDuration = this.AlarmedDuration;

                //EntityManager.Instance.npcCorpseDetectionDistance
                MoveToTargetPos(lastKnownPosition.Value);
                //check gameover instance here ?!
                
                //Second winning condition:
                // if the NPC saw you already wtih the corpse,
                // he will chase you to dead even if you don't wear the corpse anymore.
                CheckForCatchDistance(distance);
            }
            else if (lastKnownFleeAlley != null)
            {
                var distanceAlley = Vector3.Distance(lastKnownFleeAlley.transform.position, transform.position);
                if (distanceAlley < EntityManager.Instance.interactableRadius)
                {
                    //Alley targetAlley =  lastKnownFleeAlley.GetTargetAlley();
                    LogWarn("Warping!");
                    TransitionToStreet(lastKnownFleeAlley);

                    //this.transform.position = targetAlley.GetPosition();
                    lastKnownFleeAlley = null;
                    lastKnownPosition = null;
                }
                else
                {

                    //abbroach further to this alley.
                    //because of parallax scrolling the allay moves - so we update the move to every frame.
                    MoveToTargetPos(lastKnownFleeAlley.GetPosition());
                }

            }
            else
            {
                if (CurrentTaskDuration <= 0)
                {
                    Log("Calming down from State.");
                    //maybe we chased long enought and can calm down.
                    SetStatus(ENPCStatus.Neutral);
                    return;
                }

                //we don't see the player anymore. maybe he used an interactable ?!
                StreetSpriteSort sort = this.GetComponent<StreetSpriteSort>();
                if (sort == null)
                {
                    Debug.LogError("This NPC does not have a StreetSpriteSort", this);
                    return;
                }
                int streetID = 1;
                if (sort == null)
                {
                    Debug.LogWarning("Unable to determine Street ID of NPC ", this);
                }
                else
                {
                    streetID = sort.street;
                }

                var interactable = EntityManager.Instance.GetClosestInteractableWithinRange(lastKnownPosition.Value, streetID);

                if (interactable != null)
                {
                    if (interactable is Alley)
                    {
                        lastKnownPosition = null;
                        lastKnownFleeAlley = interactable as Alley;
                    }
                    else
                    {
                        Debug.LogError("Wheres the Player  ? " + interactable.ToString());
                    }

                }



                // we are moving to the last known position.
                //maybe there is an interactable there ?
                //MoveToTargetPos(lastKnownPosition.Value);
            }
            //if ()
            //is player still in sight ?!
            //GetClosestInteractableWithinRange
        }
        else if (CurrentTaskDuration >= 0.0f)
        {
            //just finish current action.
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

        // START: Stuff from EntityManager
        bool foundACorpse = false;
        bool foundPlayerCarryingCorpse = false;

        if (isInVision)
        {
            if (CharacterPlayer.instance.GetCurrentCorpse() != null && !CharacterPlayer.instance.IsHiding())
            {
                FollowCharacter(CharacterPlayer.instance);
            }
            /*else if (CharacterPlayer.instance.IsHiding())
            {
                SetStatus(ENPCStatus.Neutral);
            }*/

            foundPlayerCarryingCorpse = true;
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
            Log("Corpse detected!! Distance: " + distance);
        }
        // END: Stuff from EntityManager

    }


    public void TransitionToStreet(Alley alley)
    {
        CurrentStreet = alley.GetTargetAlley().GetCurrentStreet();
        gameObject.transform.SetParent(CurrentStreet.gameObject.transform);
        Vector3 temp = alley.GetTargetAlley().gameObject.transform.localPosition;
        temp.y = CurrentStreet.StreetYOffset;

        StreetSpriteSort sss = GetComponent<StreetSpriteSort>();
        sss.street = CurrentStreet.streetID;
        gameObject.transform.localPosition = temp;
    }

    private void MoveToTargetPos(Vector3 pos)
    {
        if ((pos - transform.position).normalized.x < 0)
        {
            SetCurrentDirection(EDirection.Left);
        }
        else
        {
            SetCurrentDirection(EDirection.Right);
        }

    }

    private void OnDrawGizmos()
    {
        if (lastKnownPosition.HasValue)
        {
            Gizmos.color = new Color(1, 1, 0, 0.5f);
            Gizmos.DrawLine(transform.position, lastKnownPosition.Value);
        }
    }

    public void Think()
    {
        //if we are sleepy, we fall asleep.
        if (currentSleepiness > 1)
        {
            //CurrentAction = EAction.Idle;
            CurrentTaskDuration = 7f;
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
        if (foundCorpse != null)
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

    public void FollowCharacter(CharacterPlayer instance)
    {
        float distance = Vector3.Distance(transform.position, CharacterPlayer.instance.transform.position);
        MoveToTargetPos(instance.transform.position);

        if (distance < EntityManager.Instance.npcCorpseDetectionDistance / 4)
        {
            CheckForCatchDistance(distance);
        } else if (distance < EntityManager.Instance.npcCorpseDetectionDistance / 2)
        {
            lastKnownPosition = instance.GetPosition();
            lastKnownFleeAlley = null;
            CurrentTaskDuration = AlarmedDuration;
            SetStatus(ENPCStatus.Alarmed);
        }
        else if (distance < EntityManager.Instance.npcCorpseDetectionDistance)
        {
            lastKnownPosition = instance.GetPosition();
            lastKnownFleeAlley = null;
            CurrentTaskDuration = AlarmedDuration;
            SetStatus(ENPCStatus.Alert);
        }
        else
        {
            Debug.LogError("Incosistent State: Cant follow player that is to far away.");
        }
    }

    private void CheckForCatchDistance(float distance)
    {
        if (distance <  EntityManager.Instance.npcCorpseDetectionDistance / 4)
        {
            if (!harmlessNPCCheat)
            {
                CharacterPlayer.instance.HandleGetCaught();
            }

            SetStatus(ENPCStatus.Aggressive);
            CurrentTaskDuration = AlarmedDuration;
        }
    }
}
