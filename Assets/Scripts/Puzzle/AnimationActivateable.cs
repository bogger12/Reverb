

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

    public override void Activate(bool permanent)
    {
        animator.SetBool(ActivatedHash, true);
        if (permanent) AkUnitySoundEngine.PostEvent("PuzzleComplete", gameObject);
        else AkUnitySoundEngine.PostEvent("PuzzleCompleteSmall", gameObject);
    }
    public override void Deactivate()
    {
        animator.SetBool(ActivatedHash, false);
    }

}