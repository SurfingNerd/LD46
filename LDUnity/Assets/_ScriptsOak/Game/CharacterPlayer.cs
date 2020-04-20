using System;
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
    MAX
}

[System.Serializable]
public class PlayerActionDefinition
{
    public EPlayerAction Action = EPlayerAction.None;
    public float ActionRate = 5.0f;
}

public class CharacterPlayer : Character
{
    public static CharacterPlayer instance;

    private Corpse currentCorpse;

  	private StreetSpriteSort sss;

    [SerializeField]
    SpriteRenderer TooltipRenderer;

    [SerializeField]
    float CarryingCorpseSpeedFactorMove = 0.5f;
    [SerializeField]
    float CarryingCorpseSpeedFactorAction = 0.5f;

    [SerializeField]
    List<PlayerActionDefinition> ListActionDefinitions = new List<PlayerActionDefinition>();

    Dictionary<EPlayerAction, PlayerActionDefinition> ActionDefinitions = new Dictionary<EPlayerAction, PlayerActionDefinition>();


    bool bIsHiding = false;
    
    bool bIsCaught = false;
    EPlayerAction CurrentAction = EPlayerAction.None;
    float CurrentActionProgress = 0.0f;
    bool bJustFinishedAction = false;

    public override void SetCurrentDirection(EDirection dir)
    {
        switch (dir)
        {
            case EDirection.Left:
                CurrentDirection.x = -1;
                if(currentCorpse != null)
                {
                    CharacterAnimator.SetAnimation(EAnimation.Drag, true, false);
                }
                else
                {
                    CharacterAnimator.SetAnimation(EAnimation.Move, true, false);
                }
                break;
            case EDirection.Right:
                CurrentDirection.x = 1;
                if (currentCorpse != null)
                {
                    CharacterAnimator.SetAnimation(EAnimation.Drag, false, false);
                }
                else
                {
                    CharacterAnimator.SetAnimation(EAnimation.Move, false, false);
                }
                break;
            case EDirection.Up:
                break;
            case EDirection.Down:
                break;
            case EDirection.Neutral:
                CharacterAnimator.SetAnimation(EAnimation.Idle, false, false);
                CurrentDirection.x = 0;
                break;
        }
    }
    protected void Start() {

        for(int i = 0; i < ListActionDefinitions.Count; ++i)
        {
            ActionDefinitions.Add(ListActionDefinitions[i].Action, ListActionDefinitions[i]);
        }
        instance = this;
        sss = GetComponent<StreetSpriteSort>();
        StreetSpriteSort.PlayerStreetSwapp(sss.street);
        SmoothCamera.Target = transform;
        base.Start();
    }
    public override void MoveCharacter()
    {
        if (bIsCaught)
        {
            return;
        }
        if (currentCorpse != null)
        {
            SetPosition(gameObject.transform.position + CurrentDirection * Time.deltaTime * MoveSpeed * CarryingCorpseSpeedFactorMove);
        }
        else
        {
            base.MoveCharacter();
        }
    }

    public bool IsCaught()
    {
        return bIsCaught;
    }

    public void HandleGetCaught()
    {
        if(bIsCaught)
        {
            return;
        }
        bIsCaught = true;

        if (HUD.Instance != null)
        {
            HUD.Instance.SetGetCaught(true);
        }
    }

    public void SetCaught(bool isCaught)
    {
        bIsCaught = isCaught;
    }

    public void SetJustFinishedAction(bool finished)
    {
        bJustFinishedAction = finished;
        if (!bJustFinishedAction)
        {
            if (HUD.Instance != null)
            {
                HUD.Instance.SetProgressBarProgress(0.0f);
            }
        }
    }

    public void StartInteraction()
    {
        CurrentActionProgress = 0.0f;
        if (CurrentClosestInteractable != null)
        {
            CurrentAction = CurrentClosestInteractable.GetPlayerActionType();
        }
    }

    public void ProgressInteraction()
    {
        if (bJustFinishedAction || CurrentClosestInteractable == null)
        {
            Debug.Log($"can't interact. {bJustFinishedAction}, {CurrentClosestInteractable}");
            return;
        }

       
        CurrentActionProgress += Time.deltaTime * ActionDefinitions[CurrentAction].ActionRate * (GetCurrentCorpse() != null ? CarryingCorpseSpeedFactorAction : 1.0f);

        //Debug.LogWarning("Progress: " + CurrentActionProgress.ToString());

        HUD.Instance.SetProgressBarProgress(CurrentActionProgress);
        if (CurrentActionProgress >= 1.0f)
        {
            TryInteract();
            HUD.Instance.SetProgressBarProgress(0.0f);
            bJustFinishedAction = true;
        }
    }

    public void TransitionToStreet(Alley alley)
    {
    	var tempsss = alley.GetCurrentStreet().GetComponent<StreetSpriteSort>();
    	if(tempsss != null){
    		tempsss.spriteColour = Color.white;
    		tempsss.transitionFrac = 1;
    		tempsss.layer = SortLayer.BUILDING_FRONT;
    	}
    	tempsss = (CurrentStreet=alley.GetTargetAlley().GetCurrentStreet()).GetComponent<StreetSpriteSort>();
    	if(tempsss != null){
    		tempsss.spriteColour = Color.black;
    		tempsss.transitionFrac = 1;
    		tempsss.layer = SortLayer.BACKGROUND;
    	}
    	var delta = transform.position.y;

        gameObject.transform.SetParent(CurrentStreet.gameObject.transform);
        Vector3 temp = alley.GetTargetAlley().gameObject.transform.localPosition;
        temp.y = CurrentStreet.StreetYOffset;
        sss.street = CurrentStreet.streetID;
        if(currentCorpse!=null)currentCorpse.gameObject.GetComponent<StreetSpriteSort>().street = sss.street;
        gameObject.transform.localPosition = temp;

        delta -= transform.position.y;

        StreetSpriteSort.PlayerStreetSwapp(sss.street);


        // SmoothCamera.camT.transform.parent = transform.parent;
        if(SmoothCamera.locked = CurrentStreet.lockable){
        	SmoothCamera.lockPos = Vector3.zero;
        	SmoothCamera.lockSize = CurrentStreet.size;
        	float dy = CurrentStreet.transform.position.y+CurrentStreet.StreetYOffset-(CurrentStreet.GetSuperStreet().transform.position.y+CurrentStreet.GetSuperStreet().StreetYOffset);
        	SmoothCamera.lockSize.y+=Mathf.Abs(dy);
			SmoothCamera.lockPos.y-=dy*0.5f;
			SmoothCamera.Target = CurrentStreet.transform;
        }else{
        	SmoothCamera.Target = transform;
        	SmoothCamera.lockPos = Vector3.zero;
        // SmoothCamera.targetPosition.x=-transform.position.x/SmoothCamera.Parallax(1,0,delta);
        }

        AudioManager.Instance.PlaySoundOneShot(AudioManager.Instance.ClipEffectTransition,0.25f);
    }

    IInteractable CurrentClosestInteractable = null;


    public override void Tick()
    {
        base.Tick();

        if (currentCorpse != null)
        {
            currentCorpse.AdvanceDecay();
        }

        StreetSpriteSort sordo = GetComponent<StreetSpriteSort>();
        if(sordo != null)
        {
            IInteractable closestInteractable = EntityManager.Instance.GetClosestInteractableWithinRange(gameObject.transform.position, sordo.street);
            if(CurrentClosestInteractable != closestInteractable)
            {
                CurrentActionProgress = 0.0f;
                HUD.Instance.SetProgressBarProgress(0.0f);
            }
            CurrentClosestInteractable = closestInteractable;
        }
        else
        {
            CurrentClosestInteractable = null;
        }

        if (CurrentClosestInteractable != null)
        {
            TooltipRenderer.sprite = CurrentClosestInteractable.GetInteractIcon();
        }
        else
        {
            TooltipRenderer.sprite = null;
        }


        // if (CurrentClosestInteractable != closestInteractable && closestInteractable != null)
        // {
        //     Debug.Log("Player is near interactable: " + closestInteractable);
        // }


    }


    public void SetCurrentCorpse(Corpse corpse)
    {
        currentCorpse = corpse;
    }

    public void DropCorpse()
    {
        if (currentCorpse != null)
        {
            currentCorpse.transform.SetParent(gameObject.transform.parent);
            currentCorpse.Rendy.sprite = currentCorpse.DroppedSprite;
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

            GameManager.Instance.CompleteLevel();
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
