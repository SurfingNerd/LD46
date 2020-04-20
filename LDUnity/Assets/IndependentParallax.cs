using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentParallax : MonoBehaviour {
	public float x = 0, amount = 0;
	private Street myStreet;
	void Start() {
		myStreet = GetComponentInParent<Street>();
		x=transform.position.x;
	}
	void Update() {
		var p = transform.position;
		p.x=x+SmoothCamera.Parallax(SmoothCamera.camT.position.x,SmoothCamera.camT.position.y,myStreet.transform.position.y-myStreet.StreetYOffset-amount);
		transform.position = p;
	}
}
