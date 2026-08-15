using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SoundSurface : MonoBehaviour
{
    public enum Material
    {
        Metal,
        Wood,
        Concrete,
        Water
    }

    public Material material;

    public void OnSoundCollision(Collision collision)
    {
        AkUnitySoundEngine.PostEvent(string.Format("Bounce_{0}", material), collision.thisGameObject); // Bounce_Metal
    }
}
