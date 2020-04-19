using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseContainer : MonoBehaviour
{
    [SerializeField]
    Sprite InteractIcon;

    public bool bIsRummaged = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (bIsRummaged)
        {
            return;
        }

        Instantiate(BodyPartManager.Instance.GetRandomCorpseTemplate(),
            gameObject.transform.position,
            new Quaternion(),
            gameObject.transform.parent);

        bIsRummaged = true;
    }

    public Sprite GetInteractIcon()
    {
        return InteractIcon;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
}
