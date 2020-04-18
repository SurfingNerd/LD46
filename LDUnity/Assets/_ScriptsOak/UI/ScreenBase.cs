using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBase : MonoBehaviour
{
    

    public virtual void InitScreen()
    {
        Hide();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Toggle()
    {
        if(gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public virtual void RefreshScreen()
    {

    }
}
