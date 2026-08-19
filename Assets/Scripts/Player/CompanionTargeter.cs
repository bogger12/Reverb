using UnityEngine;
using UnityEngine.AI;

public class CompanionTargeter : MonoBehaviour
{

    public Transform playerTarget;

    public NavMeshAgent companionNavMeshAgent;

    private Transform currentTarget;

    private CompanionWaitZone currentWaitZone;


    private float initialStoppingDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTarget = playerTarget;
        initialStoppingDistance = companionNavMeshAgent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {

        // If we are in wait zone, find closest wait point from player
        if (currentWaitZone)
        {
            Transform closestWaitPoint = currentWaitZone.GetClosestWaitPoint(transform);
            if (closestWaitPoint != currentTarget) currentTarget = closestWaitPoint;
        }

        // Update destination of nav agent when target changes
        if (companionNavMeshAgent.destination != currentTarget.position)
        {
            companionNavMeshAgent.SetDestination(currentTarget.position);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ON TRIGGER ENTER");
        Debug.Log(other);
        Debug.Log(other.TryGetComponent(out CompanionWaitZone w));
        if (other.CompareTag("CompanionWaitZone") && other.TryGetComponent(out CompanionWaitZone waitZone))
        {
            currentTarget = waitZone.GetClosestWaitPoint(companionNavMeshAgent);
            currentWaitZone = waitZone;
            companionNavMeshAgent.stoppingDistance = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("ON TRIGGER EXIT");

        if (other.CompareTag("CompanionWaitZone"))
        {
            currentWaitZone = null;
            currentTarget = playerTarget;
            companionNavMeshAgent.stoppingDistance = initialStoppingDistance;
        }
    }

}
