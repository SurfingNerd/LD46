using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetManager : ManagerBase
{
    public static StreetManager Instance;


    Street CurrentStreet;

    private void Awake()
    {
        Instance = this;
    }

    public override void InitManager()
    {
        base.InitManager();
    }

    public void TransitionStreet(Alley alley)
    {
        //if(CurrentStreet != null)
        //{
        //    CurrentStreet.Hide();
        //}
        CurrentStreet = alley.GetTargetAlley().GetCurrentStreet();
        CharacterPlayer.instance.TransitionToStreet(alley);

        //if (CurrentStreet != null)
        //{
        //    CurrentStreet.Show();
        //}
    }
}
