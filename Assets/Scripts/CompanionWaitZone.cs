using UnityEngine;
using UnityEngine.AI;

public class CompanionWaitZone : MonoBehaviour
{
    public Transform[] waitPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public Transform GetClosestWaitPoint(NavMeshAgent navMeshAgent)
    {
        if (waitPoints.Length == 0) return null;

        float GetPathDistance(NavMeshPath path)
        {
            float totalDistance = 0f;
            for (int i = 1; i < path.corners.Length; i++)
            {
                totalDistance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
            return totalDistance;
        }

        float GetDistance(Transform t)
        {
            NavMeshPath path = new();
            navMeshAgent.CalculatePath(t.position, path);
            return GetPathDistance(path);
        }

        Transform closest = waitPoints[0];
        float closestDistance = GetDistance(waitPoints[0]);
        for (int i = 1; i < waitPoints.Length; i++)
        {
            float distance = GetDistance(waitPoints[i]);
            if (distance < closestDistance)
            {
                closest = waitPoints[i];
                closestDistance = distance;
            }
        }
        return closest;
    }

    public Transform GetClosestWaitPoint(Transform transform)
    {
        float GetDistance(Transform t)
        {
            return (t.position - transform.position).magnitude;
        }

        Transform closest = waitPoints[0];
        float closestDistance = GetDistance(waitPoints[0]);
        for (int i = 1; i < waitPoints.Length; i++)
        {
            float distance = GetDistance(waitPoints[i]);
            if (distance < closestDistance)
            {
                closest = waitPoints[i];
                closestDistance = distance;
            }
        }
        return closest;
    }
}
