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

public class Character : MonoBehaviour {

    public Vector3 CurrentDirection;

    protected Street CurrentStreet;

    [SerializeField]
    protected float MoveSpeed = 2.0f;

    public SpriteAnimator CharacterAnimator;

    // Start is called before the first frame update

    public int GetCurrentStreet()
    {
        StreetSpriteSort sort = this.GetComponent<StreetSpriteSort>();
        return sort.street;
        //return CurrentStreet;
    }

    protected void Start() {
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
    	CurrentStreet = GetComponentInParent<Street>();
    }

    public virtual void SetCurrentDirection(EDirection dir)
    {
        switch (dir)
        {
            case EDirection.Left:
                CurrentDirection.x = -1;
                CharacterAnimator.SetAnimation(EAnimation.Move, true, false);
                break;
            case EDirection.Right:
                CurrentDirection.x = 1;
                CharacterAnimator.SetAnimation(EAnimation.Move, false, false);
                break;
            case EDirection.Up:
                break;
            case EDirection.Down:
                break;
            case EDirection.Neutral:
                CharacterAnimator.SetAnimation(EAnimation.Idle, false, false);
                CurrentDirection.x = 0;
                break;
        }
    }


    public void SetPosition(Vector3 newPosition) {
    	if(CurrentStreet!=null){
    		var delta = newPosition - CurrentStreet.transform.position;
    		delta.x = Mathf.Sign(delta.x)*Mathf.Min(Mathf.Abs(delta.x), CurrentStreet.size.x/2);
    		delta.y = Mathf.Sign(delta.y)*Mathf.Min(Mathf.Abs(delta.y), CurrentStreet.size.y/2);
    		newPosition = CurrentStreet.transform.position + delta;
    	}

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