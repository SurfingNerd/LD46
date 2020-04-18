using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNPC : Character
{
    public override void MoveCharacter()
    {
        base.MoveCharacter();
    }

    public override void InitCharacter()
    {

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
