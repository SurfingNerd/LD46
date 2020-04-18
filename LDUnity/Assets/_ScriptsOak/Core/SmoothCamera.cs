// Smooth towards the target

using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
    public Transform Target;
    public float SmoothTime = 0.3F;
    private Vector3 Velocity = Vector3.zero;
    public Vector3 Offset = Vector3.zero;

    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = Target.TransformPoint(new Vector3(0, 0, -10)) + Offset;

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref Velocity, SmoothTime);
    }
}