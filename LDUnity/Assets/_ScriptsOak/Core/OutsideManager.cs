using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideManager : MonoBehaviour
{
    public static OutsideManager Instance;
    public Camera CamRef;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
