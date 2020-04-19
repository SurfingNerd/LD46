// Smooth towards the target

using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
	public Transform Target;
	public float SmoothTime = 0.3F;
	private Vector3 Velocity = Vector3.zero;
	public Vector3 Offset = Vector3.zero;
	private Vector3 targetPosition;
	public static float xOff;


	private const float yParalaxStep = 7;

	void Update(){
		Offset.x = xOff;
		transform.position = (targetPosition = Vector3.SmoothDamp(targetPosition, Target.TransformPoint(new Vector3(0, 0, -10)), ref Velocity, SmoothTime));//+Offset;
	}
	public static float Parallax(float iX, float iY, float tY){
		return iX*(1-Mathf.Pow(2,(iY-tY)/yParalaxStep));
	}
}