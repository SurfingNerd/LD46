using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SurgeryManager : MonoBehaviour
{
    public static SurgeryManager Instance;


    public Transform BodySpawnMarker = null;

    public BodySurgery CurrentDonor = null;

    public List<EBodyPart> PendingPartsToTransfer = new List<EBodyPart>();

    public Camera CamRef;
    // public PixelPerfectCamera ppc;

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

        AudioManager.Instance.SwitchMusic(AudioManager.Instance.ClipMusicSurgery);
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

        switch(GameManager.Instance.GetCurrentLevelIndex())
        {
            case 0:
                AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipSurgeryHair);
                break;
            case 1:
                AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipSurgeryTorso);
                break;
            case 2:
                AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipSurgeryLegs);
                break;
            case 3:
                AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipSurgeryArms);
                break;
            case 4:
                AudioManager.Instance.PlayVoiceLine(AudioManager.Instance.ClipSurgeryFace);
                break;
        }
        // ppc.cropFrameY = true; 
    }

    public void EndSurgery()
    {
        Destroy(CurrentDonor.gameObject);
        GameManager.Instance.NotifySurgeryEnded();
        HUD.Instance.ButtonEndSurgery.gameObject.SetActive(false);
        // ppc.cropFrameY = false;
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
