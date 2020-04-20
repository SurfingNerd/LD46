using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySurgery : MonoBehaviour
{
    public static BodySurgery Henry;
    private void Awake()
    {
        if(bIsHenry)
        {
            Henry = this;
        }
    }
    public bool bIsHenry = false;

    public List<Transform> SnapMarkers = new List<Transform>();

    public List<EBodyPart> DetachedParts = new List<EBodyPart>();
    public List<EBodyPart> ReplacedParts = new List<EBodyPart>();

    public bool CanAttach(BodyPartSurgery part)
    {
        return DetachedParts.Contains(part.Type) && part.bIsMatch;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetSnapPositionForBodyPart(EBodyPart part)
    {
        return SnapMarkers[(int)part].position;
    }

    public void AddReplacedBodypart(EBodyPart part)
    {
        if(ReplacedParts.Contains(part))
        {
            Debug.LogError("MASSIVE SHITSPLOSION; MORE THAN ONE OF SAME BODY PART BEING ATTACHED TO HENRY; TELL OTT TO FIX");
        }
        else
        {
            ReplacedParts.Add(part);
            if(ReplacedParts.Count >= (int)EBodyPart.Torso)
            {
                //TODO: end game!
            }
        }
    }
}
