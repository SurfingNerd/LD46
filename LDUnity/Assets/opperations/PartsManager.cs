using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsManager : MonoBehaviour{
	public bool root = false;
	public Sprite sprite;
	private SpriteRenderer sr;
	private static int layerIndex = 99;
	private bool started = false;
	void Start() {
		if(!started){	started = true;
			sr = GetComponent<SpriteRenderer>();
			sr.sprite = sprite;
			sr.sortingLayerName = "parts";
			sr.sortingOrder = ++layerIndex;
			sr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

			foreach(Transform child in transform){
				var sm = child.GetComponent<SpriteMask>();
				if(sm != null) setupMask(sm,sr.sortingOrder,sr.sortingLayerID);
			}

			if(init0){
				lastPos = transform.position;
				lastRot = transform.eulerAngles;
			}
		}
	}
	public void Slice(Vector3 slice){ //offset.xy, rot
		var mask = new GameObject(){name="mask"};
		mask.transform.parent = transform;

		mask.transform.position = new Vector3(slice.x,slice.y,0);
		mask.transform.eulerAngles = new Vector3(0,0,slice.z);

		if(root){
			var part = Instantiate(gameObject,transform.position,transform.rotation).GetComponent<PartsManager>();

			part.gameObject.name = "bodypart";
			part.root = false;
			part.sprite = sprite;

			part.init0 = false;
			part.lastPos = transform.position+(transform.position-new Vector3(slice.x,slice.y,0)).normalized*0.25f;

			part.Start();
			slice.z+=180;
			part.Slice(slice);
		}

		var sm = mask.AddComponent<SpriteMask>();
		setupMask(sm,sr.sortingOrder,sr.sortingLayerID);
		sm.sprite = Resources.Load<Sprite>("mask");
	}

	private void setupMask(SpriteMask sm, int order, int layerID){
		sm.isCustomRangeActive = true;
		sm.frontSortingLayerID = sm.backSortingLayerID = layerID;
		sm.frontSortingOrder = order;
		sm.backSortingOrder = order-1;
	}

	public Vector3 lastPos, lastRot;
	public bool init0 = true;
	const float drag = 0.2f;
	void FixedUpdate(){
		var delta = transform.position - lastPos;
		transform.position+=delta;
		lastPos=transform.position-delta*(1-drag);
	}

}
