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
    public event Action<Material, GameObject> OnSoundReflectEnter;
    public event Action<Material, GameObject> OnSoundReflectExit;


    void OnEnable()
    {

    }

    public void SoundCollide(GameObject fromObject)
    {
        OnSoundCollision?.Invoke(material, fromObject);
        AkUnitySoundEngine.PostEvent(string.Format("Bounce_{0}", material), fromObject); // Bounce_Metal
    }


    public void BeginRaySound(GameObject fromObject)
    {
        OnSoundReflectEnter?.Invoke(material, fromObject);
        // TOSOUND: Begin ray sound hitting wall
    }

    public void EndRaySound(GameObject fromObject)
    {
        OnSoundReflectExit?.Invoke(material, fromObject);
        // TOSOUND: End ray sound hitting wall
    }


}
