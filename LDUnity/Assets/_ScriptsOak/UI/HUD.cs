using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    private void Awake()
    {
        Instance = this;    
    }

    [SerializeField]
    TextMeshProUGUI TextTime = null;
    [SerializeField]
    TextMeshProUGUI TextScore = null;

    // Start is called before the first frame update
    void Start()
    {
        ToggleHUD(false);
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
}
