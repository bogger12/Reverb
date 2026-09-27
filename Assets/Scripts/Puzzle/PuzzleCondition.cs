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
            activateable.Activate(triggersOnce);
        }
        hasTriggered = true;
    }

    public void OnUncompleted()
    {
        if (triggersOnce) return;
        activateable.Deactivate();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, activateable.transform.position - transform.position);
    }
}
