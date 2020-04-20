// Smooth towards the target

using UnityEngine;
using System.Collections;
using UnityEngine.U2D;

public class SmoothCamera : MonoBehaviour
{
	public static Transform Target;
	public float SmoothTime = 0.3F;
	private Vector3 Velocity = Vector3.zero;
	public static Vector3 targetPosition;
	public static Transform camT;
	public static Camera cam;
	public static Vector2 lockSize;
	public static Vector3 lockPos;
	public static bool locked;
	private const float yParalaxStep = 7;
	public float yOff=1;
	// public float minSize = 6.5f;
	public PixelPerfectCamera ppc;
	public float zoomTimeMult = 30;
	void Start() {
		camT = transform;
		cam = GetComponent<Camera>();
		ppc = GetComponent<PixelPerfectCamera>();
	}
	private float k = 100;
	void Update() {
		var delta = (locked?200.0f/(cam.aspect<(lockSize.x+2)/(lockSize.y+2)?lockSize.x:lockSize.y):16)-k;
		k+=Mathf.Sign(delta)*Mathf.Min(Mathf.Abs(delta),Time.deltaTime*zoomTimeMult);
		ppc.assetsPPU = (int)k;
		transform.position = (targetPosition = Vector3.SmoothDamp(targetPosition, locked?lockPos+Target.TransformPoint(new Vector3(0, 0, -10)):Target.TransformPoint(new Vector3(0, 2.25f+yOff*Target.position.y, -10)), ref Velocity, SmoothTime));
	}
	public static float Parallax(float iX, float iY, float tY) {
		return iX * (1 - Mathf.Pow(2, (iY - tY) / yParalaxStep));
	}
}