using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    private static int spurtCount = 0;
    private static BodyPartSurgery target;
    private static Vector3 offset;
    private ParticleSystem ps;

    private static Vector3 lastPos;
    void Start() { ps = GetComponent<ParticleSystem>(); }
    void FixedUpdate()
    {
        if (spurtCount > 0)
        {
            if (target != null)
            {
                transform.position = target.SpurtMarker.gameObject.transform.position;
            }
            else
            {
                transform.position = lastPos;
            }
            ps.Emit(spurtCount == tsc ? 60 : (int)(spurtCount * Random.value * 0.0023f));
            spurtCount--;
        }
    }
    const int tsc = 600;
    public static void Spurt(BodyPartSurgery _target, Vector3 _cutoffset)
    {
        spurtCount = tsc;
        target = _target;
        offset = _cutoffset;
        lastPos = target.SpurtMarker.gameObject.transform.position;
    }
    public static void KillSpurt() { spurtCount = 0; }
}
