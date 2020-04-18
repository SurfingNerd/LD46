using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour, IInteractable
{

    public bool isHidden = false;

    public Sprite GetInteractIcon()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        if (CharacterPlayer.instance.GetCurrentCorpse() == null)
        {
            //check if there is a corpse.
            if (CharacterPlayer.instance.GetCurrentCorpse() != null)
            {
                throw new System.NotImplementedException("Holding already a Corpse");
            }

            //check the nearest hideout, maybe we picked the corpse just from this hideout.
            var corpseHideout = EntityManager.Instance.GetCorpseHideoutWithinRange(this.transform.position, true);
            if (corpseHideout != null && corpseHideout.currentCorpse == this)
            {
                corpseHideout.currentCorpse = null;
            }

            isHidden = false;
            CharacterPlayer.instance.SetCurrentCorpse(this);

            // using parenting here for moving corpse.
            // might be suboptimal for animation.
            transform.SetParent(CharacterPlayer.instance.transform);
        }
        else
        {
            CorpseHideout hideout = EntityManager.Instance.GetCorpseHideoutWithinRange(this.transform.position, false);

            if (hideout != null)
            {
                hideout.currentCorpse = CharacterPlayer.instance.GetCurrentCorpse();
                hideout.currentCorpse.isHidden = true;
                CharacterPlayer.instance.GetCurrentCorpse().transform.SetParent(hideout.transform);
                CharacterPlayer.instance.SetCurrentCorpse(null);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
}
