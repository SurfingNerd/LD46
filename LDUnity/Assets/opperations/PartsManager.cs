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

		Slice();
	}
	public void Slice(){//Vector3 slice){ //offset.xy, rot
		Debug.Log("fsdghfdhjf");
		var mask = new GameObject(){name="mask"};
		mask.transform.parent = transform;
		var sm = mask.AddComponent<SpriteMask>();
		sm.isCustomRangeActive = true;
		sm.backSortingLayerID = sm.frontSortingLayerID = sr.sortingLayerID;
		sm.frontSortingOrder = 100;//sr.sortingOrder;
		sm.backSortingOrder = 99;//sr.sortingOrder-1;
	}
}
