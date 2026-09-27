using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Microphone : SoundSurface
{

    public int tonesNeeded = 1 << (int)Ray.Tone.Do; // Do ray


    void Start()
    {
        Color finalColor = Color.white;

        int ratioCount = 0;
        for (int i = 0; i < Enum.GetNames(typeof(Ray.Tone)).Length; i++)
        {
            int currentTone = (1 << i) >> 1;
            Debug.Log(string.Format("current tone: {0}", currentTone));

            if ((tonesNeeded & currentTone) != 0)
            {
                currentTone >>= 1; // Shift to correct for enum
                Color newColor = Ray.toneToColor[(Ray.Tone)currentTone];
                Debug.Log(string.Format("tone: {0} color got: {1}", (Ray.Tone)currentTone, newColor));

                float ratio = 1f / (ratioCount++ + 1);
                Debug.Log(string.Format("{0} x {1}", ratio, newColor));
                Debug.Log(Mathf.Lerp(finalColor.r, newColor.r, ratio));
                Debug.Log(Mathf.Lerp(finalColor.g, newColor.g, ratio));
                Debug.Log(Mathf.Lerp(finalColor.b, newColor.b, ratio));
                finalColor.r = Mathf.Lerp(finalColor.r, newColor.r, ratio);
                finalColor.g = Mathf.Lerp(finalColor.g, newColor.g, ratio);
                finalColor.b = Mathf.Lerp(finalColor.b, newColor.b, ratio);
            }
        }
        Debug.Log(finalColor);
        GetComponent<MeshRenderer>().material.color = finalColor;
    }
    public bool IsSatisfied()
    {
        if (currentHits.Count == 0) return false; // TODO remove
        int currentTones = 0;
        foreach (Ray.SoundRayHit soundRayHit in currentHits)
        {
            currentTones |= 1 << (int)soundRayHit.tone;
        }
        Debug.Log(string.Format("{0}, {1}, {2}", currentTones, tonesNeeded, currentHits.Count));
        return (currentTones & tonesNeeded) == tonesNeeded;
    }

    void OnDrawGizmos()
    {
        if (IsSatisfied())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
