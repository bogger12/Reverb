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


    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, activateable.transform.position - transform.position);

        foreach (Microphone mic in GetComponentsInChildren<Microphone>())
        {
            Gizmos.color = mic.IsSatisfied() ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, mic.transform.position);
        }
    }
}
