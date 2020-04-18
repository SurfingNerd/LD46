using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : ManagerBase
{
    public static ScreenManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<ScreenBase> ScreenTemplates = new List<ScreenBase>();


    public override void InitManager()
    {
        base.InitManager();

        for (int i = 0; i < ScreenTemplates.Count; ++i)
        {
            ScreenBase screen = Instantiate(ScreenTemplates[i]);
            screen.gameObject.transform.SetParent(gameObject.transform);
            screen.InitScreen();
        }


    }

}
