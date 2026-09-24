using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LightPrism : SoundSurface
{

    public int linesToSplit = 2;
    public float splitAngleDegrees = 20;

    public GameObject lineChild;

    private SoundRay[] soundRayChildren;

    bool hitLastFrame = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!lineChild || !lineChild.TryGetComponent(out LineRenderer _) || !lineChild.TryGetComponent(out SoundRay _))
        {
            Debug.LogError("LightPrism needs a child with LineRenderer and SoundRay assigned");
        }

        // create line renderer children, assign to array
        for (int i = 1; i < linesToSplit; i++)
        {
            Instantiate(lineChild, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        }
        soundRayChildren = new List<SoundRay> { lineChild.GetComponent<SoundRay>() }.Concat(gameObject.GetComponentsInChildren<SoundRay>()).ToArray();

    }

    // Update is called once per frame
    void Update()
    {
        if (!hitLastFrame)
        {
            DisableSoundRays();
        }
        float startAngle = -splitAngleDegrees * (Mathf.Floor(linesToSplit / 2) + (linesToSplit % 2 == 0 ? 0.5f : 0f));

        for (int i = 0; i < soundRayChildren.Length; i++)
        {
            soundRayChildren[i].transform.rotation = gameObject.transform.rotation * Quaternion.AngleAxis(startAngle + splitAngleDegrees * i, transform.up);
        }

        hitLastFrame = false;
    }


    void EnableSoundRays()
    {
        foreach (SoundRay soundRay in soundRayChildren)
        {
            soundRay.Activate();
        }
    }

    void DisableSoundRays()
    {
        foreach (SoundRay soundRay in soundRayChildren)
        {
            soundRay.Deactivate();
        }
    }


    public override void SoundRayHit(Vector3 hitPoint, GameObject fromObject)
    {
        hitLastFrame = true;
        EnableSoundRays();
    }

}
