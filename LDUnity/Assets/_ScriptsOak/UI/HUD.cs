﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    private void Awake()
    {
        Instance = this;    
    }

    [SerializeField]
    Image ImageProgressBar = null;
    [SerializeField]
    TextMeshProUGUI TextCaught = null;
    [SerializeField]
    Image ImageCaught = null;
    [SerializeField]
    Button ButtonRestart = null;

    // Start is called before the first frame update
    void Start()
    {
        //ToggleHUD(false);
        ButtonRestart.onClick.AddListener(RestartClicked);
        ImageCaught.gameObject.SetActive(false);
        ButtonRestart.gameObject.SetActive(false);
        TextCaught.gameObject.SetActive(false);
    }
    public void RestartClicked()
    {
        //TODO: ??? make it work however it should
        SceneManager.LoadScene("TestOak");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleHUD(bool visible)
    {
        gameObject.SetActive(visible);
        Debug.Log("HUD " + gameObject.activeSelf);
    }

    public void SetProgressBarProgress(float progress)
    {
        ImageProgressBar.fillAmount = progress;
    }

    public void SetGetCaught(bool isCaught)
    {
        if(isCaught)
        {
            TextCaught.text = "YOU HAVE BEEN CAUGHT";
            ImageCaught.gameObject.SetActive(true);
            ButtonRestart.gameObject.SetActive(true);
            TextCaught.gameObject.SetActive(true);
        }
        else
        {
            TextCaught.text = "";
            ImageCaught.gameObject.SetActive(false);
            ButtonRestart.gameObject.SetActive(false);
            TextCaught.gameObject.SetActive(false);
        }
    }
}
