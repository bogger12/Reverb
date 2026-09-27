using System;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;


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
            int currentToneFlag = (1 << i);
            Debug.Log(string.Format("current tone: {0}", currentToneFlag));

            if ((tonesNeeded & currentToneFlag) != 0) // If currentTone is any of the tones needed
            {
                Ray.Tone currentTone = (Ray.Tone)Mathf.FloorToInt(Mathf.Log(currentToneFlag, 2));
                Color newColor = Ray.toneToColor[(Ray.Tone)currentTone];

                float ratio = 1f / (ratioCount++ + 1);
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
