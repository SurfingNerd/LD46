using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartWorld : MonoBehaviour
{
    [SerializeField]
    public EBodyPart PartType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandlePickedUp()
    {
        Debug.Log("Player picked up Body Part: " + PartType);

        Destroy(gameObject);
    }
}
