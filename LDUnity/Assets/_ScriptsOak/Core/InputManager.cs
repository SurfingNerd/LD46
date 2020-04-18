using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : ManagerBase
{
    public static InputManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void InitManager()
    {
        base.InitManager();
    }

    private void Update()
    {
        //HandleTouchInput();
        //HandleMouseInput();


        //if (Input.GetKeyDown("c"))
        //{
        //    SaveGameManager.instance.ClearSave();
        //}

        if (CharacterPlayer.instance != null)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                CharacterPlayer.instance.TryEnterAlley();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                CharacterPlayer.instance.TryEnterAlley();
            }

            if(Input.GetKeyDown(KeyCode.F))
            {
                CharacterPlayer.instance.TryPickupBodyPart();
                CharacterPlayer.instance.TryStabNPC();
            }

            if (Input.GetKey(KeyCode.A))
            {
                CharacterPlayer.instance.SetCurrentDirection(EDirection.Left);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                CharacterPlayer.instance.SetCurrentDirection(EDirection.Right);
            }
            else
            {
                CharacterPlayer.instance.SetCurrentDirection(EDirection.Neutral);
            }
        }
    }

    public void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                HandleFingerPressed(touch.position);
            }

            if (touch.phase == TouchPhase.Began)
            {
                HandleFingerDown(touch.position);
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                HandleFingerUp(touch.position);
            }
        }
    }

    public void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleFingerDown(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            HandleFingerPressed(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleFingerUp(Input.mousePosition);
        }
    }

    public void HandleFingerUp(Vector3 fingerPosition)
    {
        RaycastHit2D hit;

        //int layerMask = 1 << LayerMask.NameToLayer("Booty");
        //layerMask = ~layerMask;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(fingerPosition), Vector2.zero, Mathf.Infinity);
        if (hit.collider != null)
        {

        }
    }

    public void HandleFingerPressed(Vector3 fingerPosition)
    {
        RaycastHit2D hit;

        //int layerMask = 1 << LayerMask.NameToLayer("Booty");
        //layerMask = ~layerMask;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(fingerPosition), Vector2.zero, Mathf.Infinity);
        if (hit.collider != null)
        {

        }
    }

    public void HandleFingerDown(Vector3 fingerPosition)
    {
        RaycastHit2D hit;

        //int layerMask = 1 << LayerMask.NameToLayer("Booty");
        //layerMask = ~layerMask;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(fingerPosition), Vector2.zero, Mathf.Infinity);
        if (hit.collider != null)
        {

        }
    }


}
