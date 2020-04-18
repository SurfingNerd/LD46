using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : ManagerBase
{
    #region singleton
    public static EntityManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion // singleton

    public float npcCorpseDetectionDistance = 3;

    public float corpsePickupRadius = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("Starting up");
    }

    // Update is called once per frame
    void Update()
    {
        var allNPCs = GetNPCs();
        var allCorpses = GetCorpses().Where(x=>!x.isHidden);
        
        foreach (var npc in allNPCs)
        {
            bool foundACorpse = false;
            foreach (var corpse in allCorpses)
            {
                var distance = Vector3.Distance(npc.transform.position, corpse.transform.position);
                if (distance < npcCorpseDetectionDistance)
                {
                    foundACorpse = true;
                    Debug.LogWarning("Corpse detected!! Distance: " + distance);
                }
            }
            npc.ActivateFoundCorpseText(foundACorpse);
        }
    }
    
    

    public Corpse[] GetCorpses()
    {
        //todo: cache for performance
        return Resources.FindObjectsOfTypeAll<Corpse>();
    }
    
    public CharacterNPC[] GetNPCs()
    {
        //todo: cache for performance
        return Resources.FindObjectsOfTypeAll<CharacterNPC>();
    }

    public Corpse GetCorpseWithinPickupRange(Vector3 position)
    {
        var result = GetCorpses().Select(x => new {Corpse = x, distance = Vector3.Distance(x.transform.position, position)})
            .Where(x => x.distance <= corpsePickupRadius).OrderBy(x => x.distance).FirstOrDefault();
        return result != null ? result.Corpse : null;
    }
}
