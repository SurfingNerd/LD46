using TMPro;
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
    Image ImageProgressBarDecay = null;
    [SerializeField]
    Image ImageCaught = null;

    [SerializeField]
    Image ImageIntro = null;
    [SerializeField]
    Button ButtonRestart = null;
    [SerializeField]
    public Button ButtonEndSurgery = null;
    [SerializeField]
    TextMeshProUGUI TextWin = null;
    [SerializeField]
    TextMeshProUGUI TextIntro = null;

    [SerializeField]
    public Button ButtonToggleHelp = null;

    

    // Start is called before the first frame update
    void Start()
    {
        //ToggleHUD(false);
        ButtonRestart.onClick.AddListener(RestartClicked);
        ButtonEndSurgery.onClick.AddListener(SurgeryManager.Instance.EndSurgery);
        ButtonToggleHelp.onClick.AddListener(ToggleHelp);

        ButtonRestart.gameObject.SetActive(false);
        ButtonEndSurgery.gameObject.SetActive(false);
    }
    public void RestartClicked()
    {
        GameManager.Instance.RestartLevel();
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

    public void SetProgressBarProgressDecay(float progress)
    {
        ImageProgressBarDecay.fillAmount = progress;
    }

    public void SetGetCaught(bool isCaught)
    {
        if(isCaught)
        {
            ImageCaught.gameObject.SetActive(true);
            ButtonRestart.gameObject.SetActive(true);
        }
        else
        {
            ImageCaught.gameObject.SetActive(false);
            ButtonRestart.gameObject.SetActive(false);
        }
    }

    public void ShowGameWinScreen()
    {
        ImageCaught.gameObject.SetActive(true);
        TextWin.gameObject.SetActive(true);
    }

    public void ShowBlackness()
    {
        ImageCaught.gameObject.SetActive(true);
    }

    public void HideBlackness()
    {
        ImageCaught.gameObject.SetActive(false);
    }

    public void ShowIntro()
    {
        ImageIntro.gameObject.SetActive(true);
        TextIntro.gameObject.SetActive(true);
    }

    public void HideIntro()
    {
        ImageIntro.gameObject.SetActive(false);
        TextIntro.gameObject.SetActive(false);

        ToggleHelp();
    }

    public void ToggleHelp()
    {
        TutorialScreen.Instance.gameObject.SetActive(!TutorialScreen.Instance.gameObject.activeSelf);
    }
}
