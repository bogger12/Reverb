using System;
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

    public event Action<Material, GameObject> OnSoundCollision;
    void OnEnable()
    {
        OnSoundCollision += PlaySound;
    }

    public void SoundCollide(Material material, GameObject fromObject)
    {
        OnSoundCollision.Invoke(material, fromObject);
    }

    private static void PlaySound(Material material, GameObject gameObject)
    {
        AkUnitySoundEngine.PostEvent(string.Format("Bounce_{0}", material), gameObject); // Bounce_Metal
    }
}
