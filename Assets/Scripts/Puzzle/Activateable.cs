using UnityEngine;

public abstract class Activateable : MonoBehaviour
{
    public abstract void Activate(bool permanent);
    public abstract void Deactivate();
}
