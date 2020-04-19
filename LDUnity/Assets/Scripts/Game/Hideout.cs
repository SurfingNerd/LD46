using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideout : MonoBehaviour, IInteractable
{
    [SerializeField]
    Sprite InteractIcon;
    //public Corpse currentCorpse;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        CharacterPlayer.instance.ToggleHiding();
    }

    public Sprite GetInteractIcon()
    {
        return InteractIcon;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public EPlayerAction GetPlayerActionType()
    {
        return EPlayerAction.Hide;
    }

    public int GetStreetSpriteSortIndex()
    {
        StreetSpriteSort sort = GetComponent<StreetSpriteSort>();

        if (sort != null)
        {
            return sort.street;
        }
        else
        {
            return 0;
        }
    }
}
