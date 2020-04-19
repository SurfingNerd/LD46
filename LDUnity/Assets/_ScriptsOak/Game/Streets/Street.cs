using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
    [SerializeField]
    public float StreetYOffset = 1.876028f;

    private static Transform camT;

    // Start is called before the first frame update
    void Start()
    {
        //Hide();
        camT = Camera.main.transform;
    }

    void Update()
    {
        var p = transform.position;
        p.x=SmoothCamera.Parallax(camT.position.x,camT.position.y,transform.position.y-StreetYOffset);
        transform.position = p;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
