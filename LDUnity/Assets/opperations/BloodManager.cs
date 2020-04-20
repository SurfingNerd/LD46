using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour {
	private static int spurtCount = 0;
	private static BodyPartSurgery target;
	private static Vector3 offset;
	private ParticleSystem ps;
	void Start() {	ps=GetComponent<ParticleSystem>(); }
	void FixedUpdate() {
		if(spurtCount>0){
			transform.position = target.SpurtMarker.gameObject.transform.position;
			ps.Emit(spurtCount==tsc?60:(int)(spurtCount*Random.value*0.0023f));
			spurtCount--;
		}
	}
	const int tsc = 600;
	public static void Spurt(BodyPartSurgery _target, Vector3 _cutoffset) {
		spurtCount = tsc;
		target = _target;
		offset = _cutoffset;
	}
	public static void KillSpurt() {	spurtCount = 0;	}
}
