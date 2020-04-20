using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartSurgery : MonoBehaviour
{

    [SerializeField]
    public EBodyPart Type;

    [SerializeField]
    public Transform SpurtMarker;

    [SerializeField]
    public bool bIsMatch = false;

    public bool bIsDetached = false;

    public bool bCanBeDetached = false;

    public bool bPendingAttach = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Detach()
    {
        if (bIsDetached || !bCanBeDetached)
        {
            return;
        }
        bIsDetached = true;

        AudioManager.Instance.PlaySoundOneShot(AudioManager.Instance.ClipSurgeryCut);

        if(GetComponentInParent<BodySurgery>() == BodySurgery.Henry)
        {
            BodySurgery.Henry.DetachedParts.Add(Type);
        }
        gameObject.transform.SetParent(gameObject.transform.parent.parent);
        gameObject.transform.position = gameObject.transform.position + new Vector3(Random.Range(-0.5f, -0.2f), 0);
    }

    public void Attach()
    {
        bIsDetached = false;
        BodySurgery.Henry.AddReplacedBodypart(Type);

        gameObject.transform.position = Vector3.zero;

        gameObject.transform.SetParent(BodySurgery.Henry.gameObject.transform);
        gameObject.transform.localPosition = new Vector3(-2.46f, -0.61f, 0);

        AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipsViable[Random.Range(0, AudioManager.Instance.ClipsViable.Count)]);

        bCanBeDetached = false;
        SurgeryManager.Instance.NotifyPartAttached(Type);
    }
}
