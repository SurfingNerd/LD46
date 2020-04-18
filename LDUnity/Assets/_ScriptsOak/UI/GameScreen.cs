using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : ScreenBase
{
    public static GameScreen Instance;

    private void Awake()
    {
        Instance = this;
    }


    public override void InitScreen()
    {
        base.InitScreen();
    }



}
