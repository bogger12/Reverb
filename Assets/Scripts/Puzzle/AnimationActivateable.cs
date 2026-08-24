

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationActivateable : Activateable
{
    private static readonly int ActivatedHash = Animator.StringToHash("Activated");
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Activate()
    {
        animator.SetBool(ActivatedHash, true);
    }
    public override void Deactivate()
    {
        animator.SetBool(ActivatedHash, false);
    }

}