using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayer : Character
{

    public static CharacterPlayer instance;

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


}
