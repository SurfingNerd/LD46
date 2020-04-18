using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMarker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleActivate()
    {
        Debug.Log("Player activated Home Marker: " + this);

        CharacterPlayer.instance.DropOffCorpseAtHome();
    }
}
