using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SoundSurface : MonoBehaviour
{
    public enum Material
    {
        Metal,
        Concrete,
        Brick,
        Rock
    }

    public Material material;

    public bool reflectRay = true;

    public event Action<Material, GameObject> OnSoundCollision;


    public List<Ray.SoundRayHit> currentHits = new List<Ray.SoundRayHit>();

    void OnEnable()
    {

    }

    void Update()
    {
        currentHits = new List<Ray.SoundRayHit>();
    }

    public void SoundCollide(GameObject fromObject)
    {
        OnSoundCollision?.Invoke(material, fromObject);
        AkUnitySoundEngine.PostEvent(string.Format("Bounce_{0}", material), fromObject); // Bounce_Metal
    }

    public virtual void SoundRayHit(Ray.SoundRayHit soundRayHit, GameObject fromObject)
    {
        currentHits.Add(soundRayHit);
    }

}
