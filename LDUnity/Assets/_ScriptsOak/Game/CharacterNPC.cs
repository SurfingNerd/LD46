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

public class CharacterNPC : Character
{
    [SerializeField]
    EBodyPart DesiredPart;

    [SerializeField]
    float ThinkCooldownMin;
    [SerializeField]
    float ThinkCooldownMax;



    EAction CurrentAction;

    float CurrentThinkCooldown = 0.0f;
    float CurrentTaskDuration = 0.0f;

    bool bIsDying = false;

    public override void MoveCharacter()
    {
        base.MoveCharacter();
    }

    public override void InitCharacter()
    {

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

        CurrentAction = EAction.Idle;
        CurrentTaskDuration -= Time.deltaTime;

        if(CurrentTaskDuration >= 0.0f)
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
    }

    public void Think()
    {
        CurrentAction = (EAction)Random.Range(0, (int)EAction.MAX);

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
}
