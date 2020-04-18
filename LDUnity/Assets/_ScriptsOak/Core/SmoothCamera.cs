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
    public static bool lockX;

    void Update()
    {
        // Define a target position above and behind the target transform
        if(lockX){
        	var t = Target.TransformPoint(new Vector3(0, 0, -10)) + Offset;
        	t.x = targetPosition.x;
        	targetPosition = t;
        	if(Mathf.Abs(transform.position.y-targetPosition.y)<0.2f) lockX = false;
        }else targetPosition = Target.TransformPoint(new Vector3(0, 0, -10)) + Offset;
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref Velocity, SmoothTime);
    }
}