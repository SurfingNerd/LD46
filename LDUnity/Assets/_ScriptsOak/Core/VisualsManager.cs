using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class SpriteContainer
{
    [SerializeField]
    public List<Sprite> SpriteList = new List<Sprite>();
}

public class VisualsManager : ManagerBase
{
    public static VisualsManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    public TMP_FontAsset FontGeneral;

    [SerializeField]
    List<SpriteContainer> SpriteContainers = new List<SpriteContainer>();

    [SerializeField]
    public Color ColorYos;
    [SerializeField]
    public Color ColorAntiYos;

    //Dictionary<EBootySize, List<Sprite>> SizeSpriteDictionary = new Dictionary<EBootySize, List<Sprite>>();


    public override void InitManager()
    {
        base.InitManager();

        //for(int i = 0; i < SpriteContainers.Count; ++i)
        //{
        //    SizeSpriteDictionary.Add(SpriteContainers[i].Size, SpriteContainers[i].SpriteList);
        //}
    }

    //public List<Sprite> GetSpriteListForBootySize(EBootySize size)
    //{
    //    return SizeSpriteDictionary[size];
    //}


}
