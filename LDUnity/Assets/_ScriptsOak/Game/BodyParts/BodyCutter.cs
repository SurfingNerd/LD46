﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCutter : MonoBehaviour
{

    [SerializeField]
    LineRenderer LineCut;

    Vector3 MouseDragStart = Vector3.zero;
    Vector3 MouseDragEnd = Vector3.zero;

    BodyPartSurgery DraggedBodyPart = null;

    Vector3 DragOffset = Vector3.zero;

    [SerializeField]
    Color ColorHighlightCut;

    [SerializeField]
    Color ColorHighlightAttach;

    // Start is called before the first frame update
    void Start()
    {
        LineCut.SetPosition(0, Vector3.zero);
        LineCut.SetPosition(1, Vector3.zero);
        LineCut.enabled = false;
    }

    BodyPartSurgery LastHitBodyPart = null;

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit;

        //int layerMask = 1 << LayerMask.NameToLayer("Booty");
        //layerMask = ~layerMask;
        BodyPartSurgery hitBodyPart = null;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);
        if (hit.collider != null)
        {
            hitBodyPart = hit.collider.gameObject.GetComponent<BodyPartSurgery>();
            if (hitBodyPart != null)
            {
                if (LastHitBodyPart != null && LastHitBodyPart != hitBodyPart)
                {
                    LastHitBodyPart.DeHighlight();
                }
                LastHitBodyPart = hitBodyPart;
                if (LastHitBodyPart != null && LastHitBodyPart.bCanBeDetached)
                {
                    LastHitBodyPart.Highlight(ColorHighlightCut);
                }
            }
            else
            {
                if (LastHitBodyPart != null)
                {
                    LastHitBodyPart.DeHighlight();
                    LastHitBodyPart = null;
                }
            }
        }
        else
        {
            if (LastHitBodyPart != null)
            {
                LastHitBodyPart.DeHighlight();
                LastHitBodyPart = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (hitBodyPart != null)
            {
                //TODO: start drag body part
                if (hitBodyPart.bIsDetached)
                {
                    DraggedBodyPart = hitBodyPart;
                    DragOffset = (new Vector3(hit.point.x, hit.point.y) - DraggedBodyPart.gameObject.transform.position);
                }
                else
                {
                    MouseDragStart = getMouseWorldPos();
                    LineCut.SetPosition(0, MouseDragStart);
                }
            }
            else
            {
                MouseDragStart = getMouseWorldPos();
                LineCut.SetPosition(0, MouseDragStart);
            }

        }

        if (Input.GetMouseButton(0))
        {
            if (DraggedBodyPart != null)
            {
                //TODO: update position of dragged body part
                DraggedBodyPart.gameObject.transform.position = getMouseWorldPos() - DragOffset;

                if (BodySurgery.Henry.CanAttach(DraggedBodyPart) &&
                    Vector3.Distance(DraggedBodyPart.SpurtMarker.transform.position,
                    BodySurgery.Henry.GetSnapPositionForBodyPart(DraggedBodyPart.Type)) < 1.2f)
                {
                    DraggedBodyPart.bPendingAttach = true;
                    DraggedBodyPart.HighlightLock(ColorHighlightAttach);
                    DraggedBodyPart.transform.position = Vector3.zero;
                }
                else
                {
                    DraggedBodyPart.bPendingAttach = false;
                    DraggedBodyPart.DeHighlight();
                }
            }
            else
            {
                LineCut.SetPosition(1, getMouseWorldPos());
                if (Vector3.Distance(LineCut.GetPosition(0), LineCut.GetPosition(1)) > 0.5f)
                {
                    LineCut.enabled = true;
                }
                else
                {
                    LineCut.enabled = false;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (DraggedBodyPart != null)
            {
                //TODO: drop body part
                if (DraggedBodyPart.bPendingAttach)
                {
                    DraggedBodyPart.Attach();

                    AudioManager.Instance.PlaySoundOneShot(AudioManager.Instance.ClipsSurgeryReplace[Random.Range(0, AudioManager.Instance.ClipsSurgeryReplace.Count)]);
                }
                else
                {
                    if(!SurgeryManager.Instance.PendingPartsToTransfer.Contains(DraggedBodyPart.Type) || !DraggedBodyPart.bIsMatch)
                    {
                        BloodManager.Spurt(DraggedBodyPart, Vector3.zero);
                        Destroy(DraggedBodyPart.gameObject);
                    }
                    AudioManager.Instance.PlaySoundOneShot(AudioManager.Instance.ClipsSurgeryDrop[Random.Range(0, AudioManager.Instance.ClipsSurgeryDrop.Count)]);
                }
            }
            else
            {
                MouseDragEnd = getMouseWorldPos();
                LineCut.SetPosition(1, MouseDragEnd);
                LineCut.enabled = false;

                if (Vector3.Distance(LineCut.GetPosition(0), LineCut.GetPosition(1)) > 0.5f)
                {
                    RaycastHit2D[] cutHits = Physics2D.RaycastAll(MouseDragStart,
                    (MouseDragEnd - MouseDragStart).normalized,
                    Vector3.Distance(MouseDragStart, MouseDragEnd));


                    if (cutHits.Length > 0)
                    {
                        for (int i = 0; i < cutHits.Length; ++i)
                        {
                            BodyPartSurgery hitBodyPartCut = cutHits[i].collider.gameObject.GetComponent<BodyPartSurgery>();
                            if (hitBodyPartCut != null && !hitBodyPartCut.bIsDetached)
                            {
                                Debug.Log("Heureka bitch");
                                Vector3 cutOffset = cutHits[i].point - new Vector2(hitBodyPartCut.gameObject.transform.position.x, hitBodyPartCut.gameObject.transform.position.y);
                                BloodManager.Spurt(hitBodyPartCut, cutOffset);
                                hitBodyPartCut.Detach();
                                break;
                            }
                        }

                    }
                }
                else
                {

                }

                
            }
            DraggedBodyPart = null;
        }
    }

    public static Vector3 getMouseWorldPos()
    {
        var p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0;
        return p;
    }
}
