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
	public static Vector2 lockSize;
	public static bool locked;
	private const float yParalaxStep = 7;
	public float yOff=1;
	// public float minSize = 6.5f;
	public PixelPerfectCamera ppc;
	void Start() { camT = Camera.main.transform; }
	void Update() {
		// ppc.assetsPPU = Mathf.Max(1,1/camera.ratio)*
		transform.position = (targetPosition = Vector3.SmoothDamp(targetPosition, Target.TransformPoint(new Vector3(0, locked?0:2.25f+yOff*Target.position.y, -10)), ref Velocity, SmoothTime));
	}
	public static float Parallax(float iX, float iY, float tY) {
		return iX * (1 - Mathf.Pow(2, (iY - tY) / yParalaxStep));
	}
}