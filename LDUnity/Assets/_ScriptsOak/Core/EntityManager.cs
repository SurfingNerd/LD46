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


    public float interactableRadius = 1;

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("Starting up");
    }

    // Update is called once per frame
    void Update()
    {
        var allNPCs = GetNPCs();
        var allCorpses = GetCorpses().Where(x => !x.isHidden);

        foreach (var npc in allNPCs)
        {
            bool foundACorpse = false;
            foreach (var corpse in allCorpses)
            {
                var distance = Vector3.Distance(npc.transform.position, corpse.transform.position);
                if (distance < npcCorpseDetectionDistance)
                {
                    foundACorpse = true;
                    //Debug.LogWarning("Corpse detected!! Distance: " + distance);
                }
            }
            npc.ActivateFoundCorpseText(foundACorpse);
        }
    }

    //TODO: cache for performance
    public List<IInteractable> GetAllInteractables()
    {
        List<IInteractable> iList = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().ToList();

        //exclude carried corpse
        iList.Remove(CharacterPlayer.instance.GetCurrentCorpse());
        return iList;
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

    public Hideout[] GetCorpseHideouts()
    {
        //todo: cache for performance
        return Resources.FindObjectsOfTypeAll<Hideout>();
    }

    public IInteractable GetClosestInteractableWithinRange(Vector3 position)
    {
        var result = GetAllInteractables().Select(x => new { interactable = x, distance = Vector3.Distance(x.GetPosition(), position) })
            .Where(x => x.distance <= corpsePickupRadius).OrderBy(x => x.distance).FirstOrDefault();
        return result != null ? result.interactable : null;
    }

    public Corpse GetCorpseWithinRange(Vector3 position)
    {
        var result = GetCorpses().Select(x => new { Corpse = x, distance = Vector3.Distance(x.transform.position, position) })
            .Where(x => x.distance <= corpsePickupRadius).OrderBy(x => x.distance).FirstOrDefault();
        return result != null ? result.Corpse : null;
    }

    //public Hideout GetCorpseHideoutWithinRange(Vector3 position, bool fullOrEmpty)
    //{
    //    var result = GetCorpseHideouts()
    //        .Where(x => (x.currentCorpse != null) == fullOrEmpty)
    //        .Select(x => new { Hideout = x, distance = Vector3.Distance(x.transform.position, position) })
    //        .Where(x => x.distance <= corpsePickupRadius).OrderBy(x => x.distance).FirstOrDefault();
    //    return result != null ? result.Hideout : null;
    //}
}
