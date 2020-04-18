using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : ScreenBase
{
    public static MenuScreen Instance;

    private void Awake()
    {
        Instance = this;
    }
    [SerializeField]
    public Button ButtonStart;
    [SerializeField]
    public Button ButtonExit;

    public override void InitScreen()
    {
        Show();

    }

    public void Exit()
    {
        Application.Quit();
    }

}
