using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class PlayerState
{
    public PlayerState()
    {

    }


    [SerializeField]
    public int Currency = 100;
    [SerializeField]
    public string Version = "0";



    //public void ProgressQuest(int amount)
    //{
    //    if (!CurrentQuest.IsCompleted)
    //    {
    //        CurrentQuest.TargetProgress = Mathf.Clamp(CurrentQuest.TargetProgress + amount, 0, CurrentQuest.TargetRoll);
    //        if (CurrentQuest.TargetProgress >= CurrentQuest.TargetRoll)
    //        {
    //            CurrentQuest.IsCompleted = true;
    //            CurrentQuest.IsPendingReward = true;
    //            Analytics.CustomEvent("completedQuest", new Dictionary<string, object>
    //            {
    //             { "didComplete", true },
    //             { "type", (int)CurrentQuest.Type},
    //             { "targetProgress", (int)CurrentQuest.TargetProgress},
    //             { "targetRegionID", (int)CurrentQuest.RegionID},
    //             { "rolledTarget", (int)CurrentQuest.TargetRoll},
    //             { "targetID", (int)CurrentQuest.TargetID},
    //            });
    //        }
    //    }
    //    SaveGameManager.instance.Save();
    //    MenuScreen.Instance.UpdateQuestDisplay();
    //}


    public override string ToString()
    {
        string playerStateString = "PlayerState: Version: " + Version + "\n ";
        //playerStateString += "-Currency: " + Currency + "\n";
        //playerStateString += "-CurrentShipConfigurationIndex: " + CurrentShipConfigurationIndex + "\n";
        //playerStateString += "-CurrentRegionID: " + CurrentRegionID + "\n";
        //playerStateString += "-TimeSinceLastQuest: " + TimeSinceLastQuest + "\n";
        //playerStateString += "-HasCompletedTutorial: " + HasCompletedTutorial + "\n";
        //for(int i = 0; i < ShipConfigurations.Count; ++i)
        //{
        //    playerStateString += ShipConfigurations[i].ToString();
        //}
        //string collectiblesString = "-CollectibleIDs: ";
        //for (int i = 0; i < OwnedCollectibles.Count; ++i)
        //{
        //    collectiblesString += OwnedCollectibles[i] + "; ";
        //}

        //playerStateString += collectiblesString;

        return playerStateString;
    }
    //public Quest CurrentQuest;
}