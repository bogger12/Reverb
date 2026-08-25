using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaySoundListener : MonoBehaviour
{


    public List<SoundRay> soundRays;

    private List<List<Vector3>> closestPoints;

    void OnEnable()
    {
        soundRays = GameObject.FindObjectsByType<SoundRay>(FindObjectsSortMode.InstanceID).ToList<SoundRay>();
    }

    void Update()
    {
        closestPoints = soundRays.Select(sr => sr.GetClosestPointsOnRay(transform.position)).ToList();
        foreach (List<Vector3> points in closestPoints)
        {
            foreach (Vector3 point in points)
            {
                float distance = Vector3.Distance(transform.position, point);
                // TOSOUND: play sound with volume based on distance here (continuous sound, this updates every frame)
            }
        }
    }

    void OnDrawGizmos()
    {
        if (closestPoints.Count == 0) return;
        foreach (List<Vector3> points in closestPoints)
        {
            foreach (Vector3 point in points)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(point, 0.5f);
            }
        }
    }
}