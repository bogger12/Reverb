using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MicrophoneRayHitCondition : PuzzleCondition
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentsInChildren<Microphone>().All(mic => mic.IsSatisfied())) OnCompleted();

        // Maybe play some nice completed sound
    }
}
