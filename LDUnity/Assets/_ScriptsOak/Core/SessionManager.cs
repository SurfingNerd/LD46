using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ERoundEndReason
{
    Time,
    Capsize,
    MAX
}

public class SessionManager : ManagerBase
{
    public static SessionManager Instance;

    private void Awake()
    {
        Instance = this;
    }


    public override void InitManager()
    {
        base.InitManager();
    }

    private void Update()
    {

    }


}
