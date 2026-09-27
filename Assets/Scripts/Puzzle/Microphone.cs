using System.Collections.Generic;
using UnityEngine;

public class Microphone : SoundSurface
{

    public int tonesNeeded = 1 << (int)Ray.Tone.Do; // Do ray


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsSatisfied()
    {
        if (currentHits.Count == 0) return false; // TODO remove
        Debug.Log(currentHits.ToArray().ToString());
        int currentTones = 0;
        foreach (Ray.SoundRayHit soundRayHit in currentHits)
        {
            currentTones |= 1 << (int)soundRayHit.tone;
        }
        Debug.Log(string.Format("{0}, {1}", currentTones, tonesNeeded));
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
