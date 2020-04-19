// Smooth towards the target

using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
    public Transform Target;
    public float SmoothTime = 0.3F;
    private Vector3 Velocity = Vector3.zero;
    public static Vector3 targetPosition;
    public static Transform camT;

    private const float yParalaxStep = 7;
    void Start() { camT = Camera.main.transform; }
    void Update()
    {
        if(Target == null)
        {
            if(CharacterPlayer.instance != null)
            {
                Target = CharacterPlayer.instance.gameObject.transform;
            }
        }
        else
        {
            transform.position = (targetPosition = Vector3.SmoothDamp(targetPosition, Target.TransformPoint(new Vector3(0, 0, -10)), ref Velocity, SmoothTime));
        }
    }
    public static float Parallax(float iX, float iY, float tY)
    {
        return iX * (1 - Mathf.Pow(2, (iY - tY) / yParalaxStep));
    }
}