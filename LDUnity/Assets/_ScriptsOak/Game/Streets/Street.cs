using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
    [SerializeField]
    public float StreetYOffset = 1.876028f;

    private const float yParalaxStep = 7;

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
        p.x=camT.position.x*(1-Mathf.Pow(2,(camT.position.y-transform.position.y-StreetYOffset)/yParalaxStep));
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
