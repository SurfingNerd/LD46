using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsManager : MonoBehaviour{
	public bool root;
	public Sprite sprite;
	private SpriteRenderer sr;
	private static int layerIndex = 99;
	void Start() {
		if((sr = GetComponent<SpriteRenderer>()) == null) sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = sprite;
		sr.sortingLayerName = "parts";
		sr.sortingOrder = ++layerIndex;

		sr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
	}
	public void Slice(Vector3 slice){ //offset.xy, rot
		var mask = new GameObject(){name="mask"};
		mask.transform.parent = transform;

		mask.transform.position = new Vector3(slice.x,slice.y,0);
		mask.transform.eulerAngles = new Vector3(0,0,slice.z);

		var sm = mask.AddComponent<SpriteMask>();
		sm.isCustomRangeActive = true;
		sm.frontSortingLayerID = sm.backSortingLayerID = sr.sortingLayerID;
		sm.frontSortingOrder = sr.sortingOrder;
		sm.backSortingOrder = sr.sortingOrder-1;
		sm.sprite = Resources.Load<Sprite>("mask");
	}
}
