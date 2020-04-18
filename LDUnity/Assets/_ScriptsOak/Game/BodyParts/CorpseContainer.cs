using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseContainer : MonoBehaviour
{

    public bool bIsRummaged = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rummage()
    {
        if(bIsRummaged)
        {
            return;
        }

        Instantiate(BodyPartManager.Instance.GetRandomCorpseTemplate(),
            gameObject.transform.position,
            new Quaternion(),
            gameObject.transform.parent);

        bIsRummaged = true;
    }
}
