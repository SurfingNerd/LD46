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
    float MoveSpeed = 2.0f;

    // Start is called before the first frame update

    void Start()
    {
        InitCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }

    public virtual void MoveCharacter()
    {
        gameObject.transform.position += CurrentDirection * Time.deltaTime * MoveSpeed;
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
}