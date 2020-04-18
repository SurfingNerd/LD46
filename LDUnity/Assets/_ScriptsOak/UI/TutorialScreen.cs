using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : ScreenBase
{
    public static TutorialScreen Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    TextMeshProUGUI TextTutorialInstruction;


    public override void InitScreen()
    {
        base.InitScreen();


    }


}
