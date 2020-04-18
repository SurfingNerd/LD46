using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alley : MonoBehaviour
{

    //[SerializeField]
    Street CurrentStreet;

    [SerializeField]
    Alley TargetAlley;


    // Start is called before the first frame update
    void Start()
    {
        CurrentStreet = gameObject.GetComponentInParent<Street>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        StreetManager.Instance.TransitionStreet(this);
    }


    public Street GetCurrentStreet()
    {
        return CurrentStreet;
    }

    public Alley GetTargetAlley()
    {
        return TargetAlley;
    }
}
