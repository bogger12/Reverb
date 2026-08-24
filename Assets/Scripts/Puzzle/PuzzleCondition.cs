using UnityEngine;

public abstract class PuzzleCondition : MonoBehaviour
{
    public Activateable activateable;
    public bool triggersOnce = false;

    private bool hasTriggered = false;

    public void OnCompleted()
    {
        if (!triggersOnce || !hasTriggered)
        {
            activateable.Activate();
        }
        hasTriggered = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, activateable.transform.position - transform.position);
    }
}
