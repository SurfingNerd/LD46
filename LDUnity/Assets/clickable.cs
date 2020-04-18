using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickable : MonoBehaviour{
	public string tag;
	public static string clickedTag;
	void OnMouseDown(){	clickedTag = tag; }
	void LateUpdate(){ clickedTag = null; }
}
