﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutter : MonoBehaviour {
	public PartsManager currentBody, heldPart;
	private LineRenderer lr;
	void Start() {
		lr = GetComponent<LineRenderer>();
	}
	
	Vector3 src;
	int phase = 0;

	void Update() {
		if (Input.GetMouseButtonDown(0)){
			phase = (phase+1)%3;
			switch (phase){
				case 0: 
					if(heldPart!=null){
						heldPart.targetPos = heldPart.getPos();
						heldPart = null;
					}
					break;
				case 1:
					lr.enabled = true;
					lr.SetPosition(0,src=getMouseWorldPos());
					break;
				case 2:
					lr.enabled = false;
					//<slice>
					var k = getMouseWorldPos();
					var rp = (src+k)*.5f-currentBody.transform.position;
					float rot = Mathf.Atan2(src.y-k.y, src.x-k.x)*Mathf.Rad2Deg;
					if(Mathf.DeltaAngle(Mathf.Atan2(rp.y, rp.x)*Mathf.Rad2Deg,rot)>0)rot+=180;
					k=(src+k)*.5f;
					k.z=rot+90;
					heldPart=currentBody.Slice(k);
					//</slice>
					break;
			}
		}
		if(lr.enabled) lr.SetPosition(1,getMouseWorldPos());
		if(heldPart!=null) heldPart.targetPos = getMouseWorldPos();
	}
	void FixedUpdate() {}
	
	// void Update() {
	// 	if (Input.GetMouseButtonDown(0)){
	// 		lr.SetPosition(0,src=getMouseWorldPos());
	// 		lr.enabled = true;
	// 	}
	// 	if(Input.GetMouseButtonUp(0)){
	// 		lr.enabled = false;
	// 		//<slice>
	// 		var k = getMouseWorldPos();
	// 		var rp = (src+k)*.5f-currentBody.transform.position;
	// 		float rot = Mathf.Atan2(src.y-k.y, src.x-k.x)*Mathf.Rad2Deg;
	// 		if(Mathf.DeltaAngle(Mathf.Atan2(rp.y, rp.x)*Mathf.Rad2Deg,rot)>0)rot+=180;
	// 		k=(src+k)*.5f;
	// 		k.z=rot+90;
	// 		currentBody.Slice(k);
	// 		//</slice>
	// 	}
		
	// 	if(lr.enabled)	lr.SetPosition(1,getMouseWorldPos());
	// }



	public static Vector3 getMouseWorldPos(){
		var p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		p.z=0;
		return p;
	}
}