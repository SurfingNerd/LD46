using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        LineCut.SetPosition(0, Vector3.zero);
        LineCut.SetPosition(1, Vector3.zero);
        LineCut.enabled = false;
    }

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

            }
            else
            {

            }
        }
        else
        {

        }

        if (Input.GetMouseButtonDown(0))
        {
            if (hitBodyPart != null)
            {
                //TODO: start drag body part
                if(hitBodyPart.bIsDetached)
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

                if(BodySurgery.Henry.CanAttach(DraggedBodyPart) && 
                    Vector3.Distance(DraggedBodyPart.SpurtMarker.transform.position, 
                    BodySurgery.Henry.GetSnapPositionForBodyPart(DraggedBodyPart.Type)) < 1.2f)
                {
                    DraggedBodyPart.bPendingAttach = true;
                    DraggedBodyPart.transform.position = Vector3.zero;
                }
                else
                {
                    DraggedBodyPart.bPendingAttach = false;
                }
            }
            else
            {
                LineCut.enabled = true;
                LineCut.SetPosition(1, getMouseWorldPos());
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (DraggedBodyPart != null)
            {
                //TODO: drop body part
                if(DraggedBodyPart.bPendingAttach)
                {
                    DraggedBodyPart.Attach();
                }
            }
            else
            {
                MouseDragEnd = getMouseWorldPos();
                LineCut.SetPosition(1, MouseDragEnd);
                LineCut.enabled = false;

                RaycastHit2D[] cutHits = Physics2D.RaycastAll(MouseDragStart,
                    (MouseDragEnd - MouseDragStart).normalized,
                    Vector3.Distance(MouseDragStart, MouseDragEnd));

                if (cutHits.Length > 0)
                {
                    for (int i = 0; i < cutHits.Length; ++i)
                    {
                        BodyPartSurgery hitBodyPartCut = cutHits[i].collider.gameObject.GetComponent<BodyPartSurgery>();
                        if (hitBodyPartCut != null)
                        {
                            Debug.Log("Heureka bitch");
                            Vector3 cutOffset = cutHits[0].point - new Vector2(hitBodyPartCut.gameObject.transform.position.x, hitBodyPartCut.gameObject.transform.position.y);
                            BloodManager.Spurt(hitBodyPartCut, cutOffset);
                            hitBodyPartCut.Detach();
                            break;
                        }
                    }

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
