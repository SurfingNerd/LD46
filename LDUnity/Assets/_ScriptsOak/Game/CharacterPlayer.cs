﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum EPlayerAction
{
    None,
    Hide,
    Inspect,
    PickUp,
    Transition,
    DropOff,
}

public class CharacterPlayer : Character
{
    public static CharacterPlayer instance;

    private Corpse currentCorpse;

    [SerializeField]
    SpriteRenderer TooltipRenderer;

    [SerializeField]
    float CarryingCorpseSpeedFactor = 0.5f;


    bool bIsHiding = false;

    bool bIsBusy = false;
    EPlayerAction CurrentAction = EPlayerAction.None;
    float CurrentActionProgress = 0.0f;
    bool bJustFinishedAction = false;

    private void Awake()
    {
        instance = this;
    }
    public override void MoveCharacter()
    {
        if(currentCorpse != null)
        {
            SetPosition(gameObject.transform.position + CurrentDirection * Time.deltaTime * MoveSpeed * CarryingCorpseSpeedFactor);
        }
        else
        {
            base.MoveCharacter();
        }
    }

    public void SetJustFinishedAction(bool finished)
    {
        bJustFinishedAction = finished;
        if(!bJustFinishedAction)
        {
            HUD.Instance.SetProgressBarProgress(0.0f);
        }
    }

    public void StartInteraction()
    {
        CurrentActionProgress = 0.0f;
        if(CurrentClosestInteractable != null)
        {
            CurrentAction = CurrentClosestInteractable.GetPlayerActionType();
        }
    }

    public void ProgressInteraction()
    {
        if(bJustFinishedAction || CurrentClosestInteractable == null)
        {
            return;
        }

        switch (CurrentAction)
        {
            case EPlayerAction.None:
                CurrentActionProgress = 1.0f;
                break;
            case EPlayerAction.Hide:
                CurrentActionProgress += Time.deltaTime * 5.0f;
                break;
            case EPlayerAction.Inspect:
                CurrentActionProgress += Time.deltaTime * 5.0f;
                break;
            case EPlayerAction.PickUp:
                CurrentActionProgress += Time.deltaTime * 5.0f;
                break;
            case EPlayerAction.Transition:
                CurrentActionProgress += Time.deltaTime * 5.0f;
                break;
            case EPlayerAction.DropOff:
                CurrentActionProgress += Time.deltaTime * 5.0f;
                break;
        }

        HUD.Instance.SetProgressBarProgress(CurrentActionProgress);
        if (CurrentActionProgress >= 1.0f)
        {
            TryInteract();
            HUD.Instance.SetProgressBarProgress(0.0f);
            bJustFinishedAction = true;
        }
    }

    public override void InitCharacter()
    {

    }
    public void TransitionToStreet(Alley alley)
    {
        gameObject.transform.SetParent(alley.GetTargetAlley().GetCurrentStreet().gameObject.transform);
        Vector3 temp = alley.GetTargetAlley().gameObject.transform.localPosition;
        temp.y = alley.GetCurrentStreet().StreetYOffset;
        gameObject.transform.localPosition = temp;
        SmoothCamera.lockX = true;
    }

    IInteractable CurrentClosestInteractable = null;


    public override void Tick()
    {
        base.Tick();

        if(currentCorpse != null)
        {
            currentCorpse.AdvanceDecay();
        }

        IInteractable closestInteractable = EntityManager.Instance.GetClosestInteractableWithinRange(gameObject.transform.position);


        if (CurrentClosestInteractable != closestInteractable && closestInteractable != null)
        {
            Debug.Log("Player is near interactable: " + closestInteractable);
        }

        CurrentClosestInteractable = closestInteractable;

        if(CurrentClosestInteractable != null)
        {
            TooltipRenderer.sprite = CurrentClosestInteractable.GetInteractIcon();
        }
        else
        {
            TooltipRenderer.sprite = null;
        }
    }


    public void SetCurrentCorpse(Corpse corpse)
    {
        currentCorpse = corpse;
    }

    public void DropCorpse()
    {
        if(currentCorpse != null)
        {
            currentCorpse.transform.SetParent(gameObject.transform.parent);
            currentCorpse = null;
        }
    }
    public void TryInteract()
    {
        //IInteractable closestInteractable = EntityManager.Instance.GetClosestInteractableWithinRange(gameObject.transform.position);

        if (CurrentClosestInteractable != null)
        {
            CurrentClosestInteractable.Interact();
        }

        //Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1.0f);
        //List<IInteractable> interactables = new List<IInteractable>();
        //Corpse corpse = null;
        //// gather all interactables
        //for (int i = 0; i < colliders.Length; ++i)
        //{
        //    IInteractable interactable = colliders[i].gameObject.GetComponent<IInteractable>();
        //    corpse = colliders[i].gameObject.GetComponent<Corpse>();

        //    //if corpse found, interact with that
        //    if(corpse != null)
        //    {
        //        corpse.Interact();
        //        return;
        //    }
        //    if (interactable != null)
        //    {
        //        interactables.Add(interactable);
        //    }
        //}

        //if(interactables.Count > 0)
        //{
        //    Debug.Log("Player is gonna interact with: " + interactables[0]);
        //    interactables[0].Interact();
        //}
    }

    public Corpse GetCurrentCorpse()
    {
        return currentCorpse;
    }

    //public void TryHandleCorpse()
    //{
    //    if (currentCorpse != null)
    //    {
    //        //find a hideout if available.

    //        Hideout hideout = EntityManager.Instance.GetCorpseHideoutWithinRange(this.transform.position, false);

    //        if (hideout != null)
    //        {
    //            hideout.currentCorpse = currentCorpse;
    //            hideout.currentCorpse.isHidden = true;
    //            currentCorpse.transform.SetParent(hideout.transform);
    //            currentCorpse = null;
    //        }
    //    }
    //    else
    //    {
    //        //check if there is a corpse.
    //        Corpse corpse = EntityManager.Instance.GetCorpseWithinRange(this.transform.position);
    //        if (corpse != null)
    //        {
    //            if (currentCorpse != null)
    //            {
    //                throw new NotImplementedException("Holding already a Corpse");
    //            }

    //            //check the nearest hideout, maybe we picked the corpse just from this hideout.
    //            var corpseHideout = EntityManager.Instance.GetCorpseHideoutWithinRange(this.transform.position, true);
    //            if (corpseHideout != null && corpseHideout.currentCorpse == corpse)
    //            {
    //                corpseHideout.currentCorpse = null;
    //            }

    //            corpse.isHidden = false;
    //            currentCorpse = corpse;

    //            // using parenting here for moving corpse.
    //            // might be suboptimal for animation.
    //            currentCorpse.transform.SetParent(this.transform);
    //        }
    //    }

    //}

    public void DropOffCorpseAtHome()
    {
        if (currentCorpse != null)
        {
            Destroy(currentCorpse.gameObject);
        }
    }

    public bool IsHiding()
    {
        return bIsHiding;
    }

    public void ToggleHiding()
    {
        bIsHiding = !bIsHiding;
        if (bIsHiding)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

}
