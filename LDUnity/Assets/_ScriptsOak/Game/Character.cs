using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirection
{
    Left,
    Right,
    Up,
    Down,
    Neutral,
}

public class Character : MonoBehaviour
{

    public Vector3 CurrentDirection;

    [SerializeField]
    protected float MoveSpeed = 2.0f;

    // Start is called before the first frame update

    void Start()
    {
        InitCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        Tick();
    }

    public virtual void MoveCharacter()
    {
        SetPosition(gameObject.transform.position + CurrentDirection * Time.deltaTime * MoveSpeed);
    }

    public virtual void InitCharacter()
    {

    }

    public void SetCurrentDirection(EDirection dir)
    {
        switch (dir)
        {
            case EDirection.Left:
                CurrentDirection.x = -1;
                break;
            case EDirection.Right:
                CurrentDirection.x = 1;
                break;
            case EDirection.Up:
                break;
            case EDirection.Down:
                break;
            case EDirection.Neutral:
                CurrentDirection.x = 0;
                break;
        }
    }


    public void SetPosition(Vector3 newPosition)
    {
        gameObject.transform.position = newPosition;
    }

    public virtual void Tick()
    {

    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
}