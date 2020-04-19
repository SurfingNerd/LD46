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


	// Start is called before the first frame update
	void Start(){
		CurrentStreet = gameObject.GetComponentInParent<Street>();
	}

	void Update(){
		if(!Application.isPlaying){
			var street = GetComponentInParent<Street>();
			float y = street.transform.position.y+street.StreetYOffset;
			street = TargetAlley.gameObject.GetComponentInParent<Street>();
			var iPosY = street.transform.position.y+street.StreetYOffset;
			var iPosX = TargetAlley.transform.position.x;
			if(y>iPosY){
				var t = transform.position;
				t.x = -SmoothCamera.Parallax(iPosX,y,iPosY);
				transform.position = t;
			}
		}
	}


	public void Interact()
	{
		StreetManager.Instance.TransitionStreet(this);
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
}


// camY = lower street.y+StreetYOffset+cam.offset.y

// camX = lower alley.x-lower street.x

// my street.x = SmoothCamera.Parallax(camT.x,camT.y,my street.y-StreetYOffset);

// lower street.x = SmoothCamera.Parallax(camT.x,camT.y,lower street.y-StreetYOffset);



// public static float Parallax(float iX, float iY, float tY){
// 	return iX*(1-Mathf.Pow(2,(iY-tY)/yParalaxStep));
// }
