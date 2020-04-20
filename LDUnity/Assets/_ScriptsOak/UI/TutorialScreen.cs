using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : ScreenBase
{
	private static bool active = false;
	private float y, yInactive;
	private void Start() {
		y = ((RectTransform)transform).anchoredPosition.y;
		yInactive=y-4000;
	}
	private void Update() {
		var p = ((RectTransform)transform).anchoredPosition;
		p.y += ((active?y:yInactive)-p.y)*6*Time.deltaTime;
		((RectTransform)transform).anchoredPosition=p;
	}

	public override void InitScreen() {
		base.InitScreen();
	}
	public static void toggleHelp(){
		active =! active;
	}
}
