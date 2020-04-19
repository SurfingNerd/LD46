using TMPro;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        //ToggleHUD(false);
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
}
