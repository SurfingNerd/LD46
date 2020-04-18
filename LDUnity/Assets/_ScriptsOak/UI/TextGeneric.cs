using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextGeneric : TextMeshProUGUI
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        if (gameObject != null && VisualsManager.Instance != null)
        {
            font = VisualsManager.Instance.FontGeneral;
        }
    }

}
