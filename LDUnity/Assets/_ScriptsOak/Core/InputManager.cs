﻿using System.Collections;
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

        if(Input.GetKeyDown(KeyCode.H))
        {
            IntroManager.instance.SkipIntro();
        }

        if (CharacterPlayer.instance != null && !CharacterPlayer.instance.IsCaught() && IntroManager.instance != null && IntroManager.instance.bIntroDone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CharacterPlayer.instance.StartInteraction();
            }

            if (Input.GetKey(KeyCode.E))
            {
                CharacterPlayer.instance.ProgressInteraction();
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                CharacterPlayer.instance.SetJustFinishedAction(false);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                CharacterPlayer.instance.DropCorpse();
            }

            if (CharacterPlayer.instance.IsHiding())
            {
                CharacterPlayer.instance.SetCurrentDirection(EDirection.Neutral);

            }
            else
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    CharacterPlayer.instance.SetCurrentDirection(EDirection.Left);
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    CharacterPlayer.instance.SetCurrentDirection(EDirection.Right);
                }
                else
                {
                    CharacterPlayer.instance.SetCurrentDirection(EDirection.Neutral);
                }
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
