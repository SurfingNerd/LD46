using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour {
	private static int spurtCount = 0;
	private static PartsManager target; 
	private ParticleSystem ps;
	void Start() {	ps=GetComponent<ParticleSystem>(); }
	void FixedUpdate() {
		if(spurtCount>0){
			transform.position = target.getPos();
			ps.Emit(spurtCount==tsc?60:(int)(spurtCount*Random.value*0.0023f));
			spurtCount--;
		}
	}
	const int tsc = 600;
	public static void Spurt(PartsManager _target) {
		spurtCount = tsc;
		target = _target;
	}
	public static void KillSpurt() {	spurtCount = 0;	}
}
