using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : ManagerBase
{
    public static PopUpManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public PopUpScreen PopUpTemplate;
    public PopUpScreen ShopPopUpTemplate;

    List<PopUpScreen> PopUps = new List<PopUpScreen>();

    public override void InitManager()
    {
        base.InitManager();
    }


    public PopUpScreen MakePopUp(PopUpScreen template, UnityEngine.Events.UnityAction yes = null, UnityEngine.Events.UnityAction no = null, UnityEngine.Events.UnityAction acknowledge = null, string desc = "")
    {
        PopUpScreen newPopUp = Instantiate(template);

        if (yes != null)
        {
            newPopUp.ButtonYes.onClick.AddListener(yes);
        }
        else
        {
            newPopUp.ButtonYes.gameObject.SetActive(false);
        }
        if (no != null)
        {
            newPopUp.ButtonNo.onClick.AddListener(no);
        }
        else
        {
            newPopUp.ButtonNo.gameObject.SetActive(false);
        }
        if (acknowledge != null)
        {
            newPopUp.ButtonAcknowledge.onClick.AddListener(acknowledge);
        }
        else
        {
            newPopUp.ButtonAcknowledge.gameObject.SetActive(false);
        }

        newPopUp.PopUpText.text = desc;

        PopUps.Add(newPopUp);

        return newPopUp;
    }

    public void ClosePopUp(PopUpScreen popUp)
    {
        if (popUp != null)
        {
            PopUps.Remove(popUp);
            Destroy(popUp.gameObject);
        }
    }
}
