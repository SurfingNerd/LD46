using System;
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
        Vector3 temp = alley.GetTargetAlley().gameObject.transform.localPosition;
        temp.y = alley.GetCurrentStreet().StreetYOffset;
        gameObject.transform.localPosition = temp;
    }

    public override void Tick()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1.0f);

        for (int i = 0; i < colliders.Length; ++i)
        {
            Alley alley = colliders[i].gameObject.GetComponent<Alley>();
            if (alley != null)
            {
                Debug.Log("Player is near alley: " + alley);
            }
            CharacterNPC npc = colliders[i].gameObject.GetComponent<CharacterNPC>();
            if (npc != null)
            {
                Debug.Log("Player is near NPC: " + npc);
            }
            BodyPartWorld bodyPart = colliders[i].gameObject.GetComponent<BodyPartWorld>();
            if (bodyPart != null)
            {
                Debug.Log("Player is near Body Part: " + bodyPart);
            }
            CorpseContainer corpseContainer = colliders[i].gameObject.GetComponent<CorpseContainer>();
            if (corpseContainer != null)
            {
                Debug.Log("Player is near Corpse Container: " + corpseContainer);
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
                Debug.Log("Player is transitioning from street: " + alley.GetCurrentStreet() + "using Alley : " + alley +
                    "; to street: " + alley.GetTargetAlley().GetCurrentStreet() + "to Alley: " + alley.GetTargetAlley() + ";");

                alley.Interact();
                break;
            }
        }
    }

    public void TryStabNPC()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1.0f);

        for (int i = 0; i < colliders.Length; ++i)
        {
            CharacterNPC npc = colliders[i].gameObject.GetComponent<CharacterNPC>();
            if (npc != null)
            {
                Debug.Log("Player is gonna stab NPC: " + npc);
                npc.HandleGetStabbed();
            }
        }
    }

    public void TryRummageCorpseContainer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1.0f);

        for (int i = 0; i < colliders.Length; ++i)
        {
            CorpseContainer container = colliders[i].gameObject.GetComponent<CorpseContainer>();
            if (container != null)
            {
                Debug.Log("Player is gonna rummage Container: " + container);
                container.Rummage();
            }
        }
    }

    public void TryPickupBodyPart()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1.0f);

        for (int i = 0; i < colliders.Length; ++i)
        {
            BodyPartWorld bodyPart = colliders[i].gameObject.GetComponent<BodyPartWorld>();
            if (bodyPart != null)
            {
                Debug.Log("Player is gonna pick up Body Part: " + bodyPart.PartType);
                bodyPart.HandlePickedUp();
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
                var corpseHideout = EntityManager.Instance.GetCorpseHideoutWithinRange(this.transform.position, true);
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
