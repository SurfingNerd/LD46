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
				lastPos = targetPos = transform.position;
				// lastRot = transform.eulerAngles;
			}
		}
	}
	public PartsManager Slice(Vector3 slice){ //offset.xy, rot
		var mask = new GameObject(){name="mask"};
		mask.transform.parent = transform;

		mask.transform.position = new Vector3(slice.x,slice.y,0);
		mask.transform.eulerAngles = new Vector3(0,0,slice.z);

		PartsManager outPart = null;
		if(root){
			outPart = Instantiate(gameObject,transform.position,transform.rotation).GetComponent<PartsManager>();

			outPart.gameObject.name = "bodypart";
			outPart.root = false;
			outPart.sprite = sprite;

			outPart.init0 = true;
			// outPart.lastPos = transform.position+(transform.position-new Vector3(slice.x,slice.y,0)).normalized*0.2f;
			// outPart.lastRot = new Vector3(0,0,Random.value);

			outPart.posOffset = new Vector3(slice.x-transform.position.x,slice.y-transform.position.y,0);

			outPart.Start();
			slice.z+=180;
			outPart.Slice(slice);
		}

		var sm = mask.AddComponent<SpriteMask>();
		setupMask(sm,sr.sortingOrder,sr.sortingLayerID);
		sm.sprite = Resources.Load<Sprite>("mask");

		return outPart;
	}

	private void setupMask(SpriteMask sm, int order, int layerID){
		sm.isCustomRangeActive = true;
		sm.frontSortingLayerID = sm.backSortingLayerID = layerID;
		sm.frontSortingOrder = order;
		sm.backSortingOrder = order-1;
	}

	public Vector3 lastPos, targetPos, posOffset;//, lastRot;
	public bool init0 = true;
	const float drag = 0.8f;
	void FixedUpdate(){
		setPos(getPos()+(targetPos-getPos())*0.1f);
		var delta = getPos() - lastPos;
		setPos(getPos()+delta);
		lastPos=getPos()-delta*drag;

		// delta = transform.eulerAngles - lastRot;
		// transform.eulerAngles+=delta;
		// lastRot=transform.eulerAngles-delta*drag;
	}
	public Vector3 getPos(){
		return transform.position+posOffset;
	}
	public void setPos(Vector3 pos){
		transform.position=pos-posOffset;
	}

}
