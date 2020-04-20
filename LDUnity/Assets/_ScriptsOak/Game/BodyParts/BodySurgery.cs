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
}
