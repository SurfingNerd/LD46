using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStreetAtmoType
{
	Street,
	Indoors,
	Canal
}

public class Street : MonoBehaviour
{
	[SerializeField]
	public float StreetYOffset = 1.876028f;
	public int streetID;
	private bool rootStreet = true;
	private Street superStreet;
	[HideInInspector]
	public Vector2 size;
	public bool lockable = true;

	[SerializeField]
	public EStreetAtmoType AtmoType;
	
	void Awake(){
		UpdateSubStreetData();
		var boundingBox = GetComponent<BoxCollider2D>();
		if(boundingBox!=null){
			size = boundingBox.size;
			Destroy(boundingBox);
		}
	}
	void Update() {
		if(rootStreet){
			var p = transform.position;
			p.x=SmoothCamera.Parallax(SmoothCamera.camT.position.x,SmoothCamera.camT.position.y,transform.position.y-StreetYOffset);
			transform.position = p;
		}
	}
	private void UpdateSubStreetData(){
		superStreet = transform.parent?.GetComponentInParent<Street>();
		if(superStreet!=null){
			superStreet.UpdateSubStreetData();
			rootStreet = false;
			superStreet = superStreet.superStreet;
			streetID = superStreet.streetID;
		}else{
			rootStreet = true;
			superStreet = this;
		}
	}
	public Street GetSuperStreet(){
		if(superStreet == null) UpdateSubStreetData();
		return superStreet;
	}
}