using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutter : MonoBehaviour {
	private LineRenderer lr;
	void Start() {
		lr = GetComponent<LineRenderer>();
	}
	/*<2 click>
	bool lineOn = false;
	void Update() {
		if (Input.GetMouseButtonDown(0)){
			lr.enabled = lineOn^=true;
			if(lineOn)	lr.SetPosition(0,getMouseWorldPos());
		}
		if(lineOn)	lr.SetPosition(1,getMouseWorldPos());
	}
	</2 click>*/

	//<click & drag>
	void Update() {
		if (Input.GetMouseButtonDown(0)){
			lr.SetPosition(0,getMouseWorldPos());
			lr.enabled = true;
		}
		if(Input.GetMouseButtonUp(0)){
			lr.enabled = false;
		}
		
		if(lr.enabled)	lr.SetPosition(1,getMouseWorldPos());
	}
	//</click & drag>
	public static Vector3 getMouseWorldPos(){
		var p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		p.z=0;
		return p;
	}
}
