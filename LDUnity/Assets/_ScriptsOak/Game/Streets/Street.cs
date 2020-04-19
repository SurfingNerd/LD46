using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
	[SerializeField]
	public float StreetYOffset = 1.876028f;
	public int streetID;
	private bool rootStreet = true;
	private Street superStreet;
	void Start(){
		UpdateSubStreetData();
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