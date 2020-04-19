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
        if(alley == null)
        {
            Debug.LogError("Tried to transition to NULL alley!");
            return;
        }
        if (alley.GetTargetAlley() == null)
        {
            Debug.LogError("Alley: " + alley + "; TargetAlley is NULL!");
            return;
        }
        if (alley.GetTargetAlley().GetCurrentStreet() == null)
        {
            Debug.LogError("Alley: " + alley + "; TargetAlley's CurrentStreet is NULL!");
            return;
        }
        CurrentStreet = alley.GetTargetAlley().GetCurrentStreet();
        CharacterPlayer.instance.TransitionToStreet(alley);

        //if (CurrentStreet != null)
        //{
        //    CurrentStreet.Show();
        //}
    }
}
