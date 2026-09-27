using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;


public static class Ray
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

    public class SoundRayHit
    {
        public Tone tone;
        public Vector3 hitPoint;
        public float distance;
        public SoundSurface soundSurface;

        public SoundRayHit(Tone tone, Vector3 hitPoint, float distance, SoundSurface soundSurface)
        {
            this.tone = tone;
            this.hitPoint = hitPoint;
            this.distance = distance;
            this.soundSurface = soundSurface;
        }
    }

    public static List<SoundRayHit> RenderLineBounces(Tone tone, LineRenderer lineRenderer, Vector3 firstPoint, Vector3 initialDirection, int numBounces, float maxDistance, LayerMask includeLayers)
    {
        Vector3 direction = initialDirection;
        Vector3 lastPoint = firstPoint;

        List<SoundRayHit> soundRayHits = new List<SoundRayHit>();

        for (int i = 0; i < numBounces; i++)
        {
            if (Physics.Raycast(lastPoint, direction, out RaycastHit hit, maxDistance, includeLayers))
            {
                direction = Vector3.Reflect(direction, hit.normal);
                lastPoint = hit.point + direction * 0.001f;

                hit.transform.TryGetComponent(out SoundSurface surface);
                SoundRayHit thisHit = new SoundRayHit(tone, hit.point, hit.distance, surface);
                soundRayHits.Add(thisHit);
                if (surface != null)
                {
                    surface.SoundRayHit(thisHit, lineRenderer.gameObject);
                    if (!surface.reflectRay) break;
                }

            }
            else
            {
                soundRayHits.Add(new SoundRayHit(tone, lastPoint + direction * maxDistance, hit.distance, null));
                break;
            }
        }

        Vector3[] linePoints = new List<Vector3> { firstPoint }.Concat(soundRayHits.Select(s => s.hitPoint)).ToArray();
        lineRenderer.positionCount = soundRayHits.Count + 1;
        lineRenderer.SetPositions(linePoints);

        return soundRayHits;
    }

    public static List<Vector3> GetClosestPointsOnRay(Vector3 firstPoint, Vector3 fromPosition, List<Vector3> pointsHit)
    {
        float minDistance = float.PositiveInfinity;

        List<Vector3> closestPoints = new List<Vector3>();

        Vector3 lastPos = firstPoint;
        foreach (Vector3 pos in pointsHit)
        {
            Vector3 lineStart = lastPos;
            Vector3 lineEnd = pos;
            Vector3 lineDir = (pos - lastPos).normalized;

            Vector3 v = fromPosition - lineStart;


            Vector3 projected = Vector3.Project(v, lineDir);
            Vector3 thisClosestPoint = lineStart + projected;

            if (Vector3.Dot(lineEnd - lineStart, fromPosition - lineStart) < 0) thisClosestPoint = lineStart;
            if (Vector3.Dot(lineStart - lineEnd, fromPosition - lineEnd) < 0) thisClosestPoint = lineEnd;

            float distance = Vector3.Distance(thisClosestPoint, fromPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
            lastPos = lineEnd;
            closestPoints.Add(thisClosestPoint);
        }
        return closestPoints;
    }


}