using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetSpriteSort : MonoBehaviour {
	private SpriteRenderer sr;

	public int initStreet;
	private int _street;
	public int street{  
		get { return _street; }
		set { 
			_street = value; 
			Recalc();
		}
	}

	public SortLayer initLayer;
	private SortLayer _layer;
	public SortLayer layer{  
		get { return _layer; }
		set { 
			_layer = value; 
			Recalc();
		}
	}

	void Start() { 
		sr = GetComponent<SpriteRenderer>();
		_street = initStreet;
		_layer = initLayer;
		Recalc();
	}
	private void Recalc() {
		sr.sortingLayerID = 0;
		sr.sortingOrder = 1000*_street+(int)_layer;
	}
}
public enum SortLayer {
	BACKGROUND, BUILDING_INTERNAL, BUILDING_FRONT, BUILDING_FACADE, ENVIRONMENT, OBJECTS, PLAYER
}