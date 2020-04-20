using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Alley : MonoBehaviour, IInteractable
{

	//[SerializeField]
	Street CurrentStreet;

	[SerializeField]
	Alley TargetAlley;

	[SerializeField]
	Sprite IconInteract;

	public bool trackOtherAlley = false;
	// Start is called before the first frame update
	void Start(){
		CurrentStreet = gameObject.GetComponentInParent<Street>();
	}
	void Update(){
		if(trackOtherAlley){
			var p = transform.position;
			p.x = TargetAlley.transform.position.x;
			transform.position = p;
		}
	}
	public void Interact()
	{
		StreetManager.Instance.TransitionStreet(this);

		AudioManager.Instance.PlaySoundOneShot(AudioManager.Instance.ClipsStairs[Random.Range(0, AudioManager.Instance.ClipsStairs.Count)]);
	}


	public Street GetCurrentStreet()
	{
		return CurrentStreet;
	}

	public Alley GetTargetAlley()
	{
		return TargetAlley;
	}

	public Sprite GetInteractIcon()
	{
		return IconInteract;
	}

	public Vector3 GetPosition()
	{
		return gameObject.transform.position;
	}

    public EPlayerAction GetPlayerActionType()
    {
        return EPlayerAction.Transition;
    }

    public int GetStreetSpriteSortIndex()
    {
        StreetSpriteSort sort = GetComponent<StreetSpriteSort>();

        if (sort != null)
        {
            return sort.street;
        }
        else
        {
            return 0;
        }
    }

}


// camY = lower street.y+StreetYOffset+cam.offset.y

// camX = lower alley.x-lower street.x

// my street.x = SmoothCamera.Parallax(camT.x,camT.y,my street.y-StreetYOffset);

// lower street.x = SmoothCamera.Parallax(camT.x,camT.y,lower street.y-StreetYOffset);



// public static float Parallax(float iX, float iY, float tY){
// 	return iX*(1-Mathf.Pow(2,(iY-tY)/yParalaxStep));
// }
