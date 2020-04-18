using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGeneric : Button
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        //if(gameObject != null && gameObject.GetComponentInChildren<Text>() != null)
        //{
        //    gameObject.GetComponentInChildren<Text>().font = VisualsManager.Instance.FontGeneral;
        //}
    }

}
