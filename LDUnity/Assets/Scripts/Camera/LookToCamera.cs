using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToCamera : MonoBehaviour
{
    private Camera _camera; 
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(_camera.transform);
    }
}
