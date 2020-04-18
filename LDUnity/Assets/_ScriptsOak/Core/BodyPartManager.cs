using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartManager : ManagerBase
{
    public static BodyPartManager Instance;

    [SerializeField]
    List<BodyPartWorld> BodyPartList = new List<BodyPartWorld>();


    Dictionary<EBodyPart, List<BodyPartWorld>> BodyPartDictionary = new Dictionary<EBodyPart, List<BodyPartWorld>>();

    private void Awake()
    {
        Instance = this;

    }

    public override void InitManager()
    {
        base.InitManager();

        for(int i = 0; i < BodyPartList.Count; ++i)
        {
            if(BodyPartDictionary.ContainsKey(BodyPartList[i].PartType))
            {
            }
            else
            {
                BodyPartDictionary.Add(BodyPartList[i].PartType, new List<BodyPartWorld>());
            }

            BodyPartDictionary[BodyPartList[i].PartType].Add(BodyPartList[i]);
        }
    }

    public BodyPartWorld GetBodyPartTemplateByType(EBodyPart type)
    {
        return BodyPartDictionary[type][Random.Range(0, BodyPartDictionary[type].Count)];
    }

}
