﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayer : Character
{

    public static CharacterPlayer instance;

    private Corpse currentCorpse;

    private void Awake()
    {
        instance = this;
    }
    public override void MoveCharacter()
    {
        base.MoveCharacter();
    }

    public override void InitCharacter()
    {

    }

    public void TransitionToStreet(Alley alley)
    {
        gameObject.transform.SetParent(alley.GetTargetAlley().GetCurrentStreet().gameObject.transform);
        
        //SetPosition(alley.GetTargetAlley().gameObject.transform.position);
        Vector3 temp = alley.GetTargetAlley().gameObject.transform.localPosition;
        temp.y = alley.GetCurrentStreet().StreetYOffset;
        //temp.x = 0;
        gameObject.transform.localPosition = temp;
    }

    public override void Tick()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1.0f);

        for(int i = 0; i < colliders.Length; ++i)
        {
            Alley alley = colliders[i].gameObject.GetComponent<Alley>();
            if(alley != null)
            {
                Debug.Log("Player is near alley: " + alley);
                //alley.Interact();
                break;

            }
        }
    }

    public void TryEnterAlley()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1.0f);

        for (int i = 0; i < colliders.Length; ++i)
        {
            Alley alley = colliders[i].gameObject.GetComponent<Alley>();
            if (alley != null)
            {
                alley.Interact();
                break;
            }
        }
    }

    public void TryHandleCorpse()
    {
        if (currentCorpse != null)
        {
            //find a hideout if available.

            CorpseHideout hideout = EntityManager.Instance.GetCorpseHideoutWithinRange(this.transform.position, false);

            if (hideout != null)
            {
                hideout.currentCorpse = currentCorpse;
                hideout.currentCorpse.isHidden = true;
                currentCorpse.transform.SetParent(hideout.transform);
                currentCorpse = null;
            }
        }
        else
        {
            
            
            //check if there is a corpse.
            Corpse corpse = EntityManager.Instance.GetCorpseWithinRange(this.transform.position);
            if (corpse != null)
            {
                if (currentCorpse != null)
                {
                    throw new NotImplementedException("Holding already a Corpse");
                }
                
                //check the nearest hideout, maybe we picked the corpse just from this hideout.
                var corpseHideout =  EntityManager.Instance.GetCorpseHideoutWithinRange(this.transform.position, true);
                if (corpseHideout != null && corpseHideout.currentCorpse == corpse)
                {
                    corpseHideout.currentCorpse = null;
                }

                corpse.isHidden = false;
                currentCorpse = corpse;
            
                // using parenting here for moving corpse.
                // might be suboptimal for animation.
                currentCorpse.transform.SetParent(this.transform);
            }
        }
        
        

    }
}
