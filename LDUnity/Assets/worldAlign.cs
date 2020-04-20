using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class worldAlign : MonoBehaviour {
	private BoxCollider2D bc;
	private void Start(){
		bc = GetComponent<BoxCollider2D>();
		UpdatePos();
	}
	private void Update() {
		if(!Application.isPlaying) UpdatePos();
	}
	private void UpdatePos() {
		var street = GetComponentInParent<Street>();
		var p = transform.position;
		p.y=street.transform.position.y+street.StreetYOffset+bc.size.y/2-bc.offset.y;
		transform.position = p;
	}
}
