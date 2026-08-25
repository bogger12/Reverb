using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

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

    private List<Vector3> pointsHit = new List<Vector3>();
    private List<SoundSurface> surfacesHit = new List<SoundSurface>();
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

        Vector3 direction = transform.forward;
        Vector3 lastPoint = emitFromPoint.position;

        pointsHit = new List<Vector3> { lastPoint };
        totalDistance = 0;
        List<SoundSurface> lastSurfacesHit = surfacesHit;
        surfacesHit = new List<SoundSurface>();

        for (int i = 0; i < strength; i++)
        {
            if (Physics.Raycast(lastPoint, direction, out RaycastHit hit, maxDistance, includeLayers))
            {
                pointsHit.Add(hit.point);
                direction = Vector3.Reflect(direction, hit.normal);

                lastPoint = hit.point + direction * 0.001f;
                totalDistance += hit.distance;
                if (hit.transform.TryGetComponent(out SoundSurface surface))
                {
                    surfacesHit.Add(surface);
                    if (!lastSurfacesHit.Contains(surface))
                    {
                        surface.BeginRaySound(surface.gameObject);
                    }
                }
            }
            else
            {
                pointsHit.Add(lastPoint + direction * maxDistance);
                break;
            }
        }
        foreach (SoundSurface surface in lastSurfacesHit) // No dictionary here cus I don't care B)
        {
            if (!surfacesHit.Contains(surface))
            {
                surface.EndRaySound(surface.gameObject);
            }
        }

        lineRenderer.positionCount = pointsHit.Count;
        lineRenderer.SetPositions(pointsHit.ToArray());

        // TOSOUND: Update audio here

        // totalDistance = total length of ray
        // surfacesHit = surfaces hit - each has material
        // tone = laser colour/sound tone -> use nameof(tone)
        string toneName = nameof(tone); // can use this for event calling based on color/tone of ray

    }

    public override void Activate()
    {
        lineRenderer.enabled = true;
    }

    public override void Deactivate()
    {
        lineRenderer.enabled = false;
    }

    public List<Vector3> GetClosestPointsOnRay(Vector3 position)
    {
        float minDistance = float.PositiveInfinity;
        Vector3 closestPoint = Vector3.zero;

        List<Vector3> closestPoints = new List<Vector3>();

        Vector3 lastPos = emitFromPoint.position;
        foreach (Vector3 pos in pointsHit)
        {
            Vector3 lineStart = lastPos;
            Vector3 lineEnd = pos;
            Vector3 lineDir = (pos - lastPos).normalized;

            Vector3 v = position - lineStart;


            Vector3 projected = Vector3.Project(v, lineDir);
            Vector3 thisClosestPoint = lineStart + projected;

            // TODO: Clamp closest between start and end

            float distance = Vector3.Distance(closestPoint, position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = thisClosestPoint;
            }
            lastPos = lineEnd;
            closestPoints.Add(thisClosestPoint);
        }
        return closestPoints;
    }

    void OnDrawGizmos()
    {
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

