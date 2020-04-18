using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutter : MonoBehaviour {
	public PartsManager currentBody;
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
	Vector3 src;
	void Update() {
		if (Input.GetMouseButtonDown(0)){
			lr.SetPosition(0,src=getMouseWorldPos());
			lr.enabled = true;
		}
		if(Input.GetMouseButtonUp(0)){
			lr.enabled = false;
			//<slice>
			var k = getMouseWorldPos();

			var rp = (src+k)*.5f-currentBody.transform.position;
			k.z = Mathf.Atan2(src.y-k.y, src.x-k.x)*Mathf.Rad2Deg;
			if(Mathf.DeltaAngle(Mathf.Atan2(rp.y, rp.x)*Mathf.Rad2Deg,k.z)>0)k.z+=180;

			k.z += 90;
			currentBody.Slice(k);
			//</slice>
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
