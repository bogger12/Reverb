using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public static class Ray
{
    public class SoundRayHit
    {
        public Vector3 hitPoint;
        public float distance;
        public SoundSurface soundSurface;

        public SoundRayHit(Vector3 hitPoint, float distance, SoundSurface soundSurface)
        {
            this.hitPoint = hitPoint;
            this.distance = distance;
            this.soundSurface = soundSurface;
        }
    }

    public static List<SoundRayHit> RenderLineBounces(LineRenderer lineRenderer, Vector3 firstPoint, Vector3 initialDirection, int numBounces, float maxDistance, LayerMask includeLayers)
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
                if (surface != null) surface.SoundRayHit(hit.point, lineRenderer.gameObject);
                soundRayHits.Add(new SoundRayHit(hit.point, hit.distance, surface));
            }
            else
            {
                soundRayHits.Add(new SoundRayHit(lastPoint + direction * maxDistance, hit.distance, null));
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