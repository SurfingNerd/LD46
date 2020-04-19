using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
	[SerializeField]
	public float StreetYOffset = 1.876028f;
	public int streetID;

	void Update()
	{
		var p = transform.position;
		p.x=SmoothCamera.Parallax(SmoothCamera.camT.position.x,SmoothCamera.camT.position.y,transform.position.y-StreetYOffset);
		transform.position = p;
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}


