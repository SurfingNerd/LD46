using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurgeryManager : MonoBehaviour
{
    public static SurgeryManager Instance;


    public Transform BodySpawnMarker = null;

    public BodySurgery CurrentDonor = null;

    public List<EBodyPart> PendingPartsToTransfer = new List<EBodyPart>();

    public Camera CamRef;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSurgery()
    {
        gameObject.SetActive(true);

        HUD.Instance.GetComponent<Canvas>().worldCamera = CamRef;
        CurrentDonor = Instantiate(GameManager.Instance.ListSurgeryBodyTemplates[GameManager.Instance.GetCurrentLevelIndex()], BodySpawnMarker.position, new Quaternion());
        BodyPartSurgery[] parts = CurrentDonor.GetComponentsInChildren<BodyPartSurgery>();
        PendingPartsToTransfer.Clear();

        for (int i = 0; i < parts.Length; ++i)
        {
            if(parts[i].bIsMatch)
            {
                PendingPartsToTransfer.Add(parts[i].Type);
            }
        }
    }

    public void EndSurgery()
    {
        Destroy(CurrentDonor.gameObject);
        GameManager.Instance.NotifySurgeryEnded();
        HUD.Instance.ButtonEndSurgery.gameObject.SetActive(false);
    }

    public void NotifyPartAttached(EBodyPart part)
    {
        PendingPartsToTransfer.Remove(part);

        if(PendingPartsToTransfer.Count <= 0)
        {
            HUD.Instance.ButtonEndSurgery.gameObject.SetActive(true);
        }
    }
}
