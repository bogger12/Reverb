using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class SoundRay : Activateable
{
    public enum Tone
    {
        Do,
        Re,
        Mi,
        Fa,
        Sol,
        La,
        Ti
    }

    private LineRenderer lineRenderer;
    public int strength = 3; // Max bounces
    public Tone tone = Tone.Do;

    public float maxDistance = 200f;

    public Transform emitFromPoint;

    public bool startsActive = true;

    public LayerMask includeLayers;

    private List<Ray.SoundRayHit> lastSoundRayHits;
    // private List<SoundSurface> surfacesHit = new List<SoundSurface>();
    private float totalDistance = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = startsActive;
    }

    // Update is called once per frame
    void Update()
    {

        if (lineRenderer.enabled)
        {
            RenderRay();
        }

        // List<SoundRayHit> lastSurfacesHit = surfacesHit;

        // foreach (SoundSurface surface in lastSurfacesHit) // No dictionary here cus I don't care B)
        // {
        // if (!lastSurfacesHit.Contains(surface))
        // {
        //     surface.BeginRaySound(surface.gameObject);
        // }
        //     if (!surfacesHit.Contains(surface))
        //     {
        //         surface.EndRaySound(surface.gameObject);
        //     }
        // }


        // TOSOUND: Update audio here

        // totalDistance = total length of ray
        // surfacesHit = surfaces hit - each has material
        // tone = laser colour/sound tone -> use nameof(tone)
        string toneName = nameof(tone); // can use this for event calling based on color/tone of ray

    }

    public void RenderRay()
    {
        lastSoundRayHits = Ray.RenderLineBounces(lineRenderer, emitFromPoint.position, transform.forward, strength, maxDistance, includeLayers);

    }

    public override void Activate()
    {
        lineRenderer.enabled = true;
    }

    public override void Deactivate()
    {
        lineRenderer.enabled = false;
    }

    public List<Vector3> GetClosestPointsOnRay(Vector3 fromPosition)
    {
        return Ray.GetClosestPointsOnRay(emitFromPoint.position, fromPosition, lastSoundRayHits.Select(s => s.hitPoint).ToList());
    }

    void OnDrawGizmos()
    {
        if (lineRenderer == null && !startsActive) return;
        Gizmos.DrawRay(emitFromPoint.position, transform.forward * 1f);
        if (!lineRenderer || !lineRenderer.enabled) return;
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        for (int i = 1; i < lineRenderer.positionCount; i++)
        {
            Gizmos.DrawWireSphere(positions[i], 0.5f);
        }

    }
}

