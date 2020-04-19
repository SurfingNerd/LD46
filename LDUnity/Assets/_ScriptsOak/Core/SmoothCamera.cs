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
	public static bool locked;
	private const float yParalaxStep = 7;
	public float yOff=1;
	// public float minSize = 6.5f;
	public PixelPerfectCamera ppc;
	void Start() {
		camT = transform;
		cam = GetComponent<Camera>();
		ppc = GetComponent<PixelPerfectCamera>();
	}
	void Update() {
		float k = locked?200.0f/(cam.aspect<lockSize.x/lockSize.y?lockSize.x:lockSize.y):16;
		ppc.assetsPPU = (int)k;
		transform.position = (targetPosition = Vector3.SmoothDamp(targetPosition, Target.TransformPoint(new Vector3(0, locked?0:2.25f+yOff*Target.position.y, -10)), ref Velocity, SmoothTime));
	}
	public static float Parallax(float iX, float iY, float tY) {
		return iX * (1 - Mathf.Pow(2, (iY - tY) / yParalaxStep));
	}
}