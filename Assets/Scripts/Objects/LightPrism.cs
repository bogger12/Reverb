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

    Quaternion rayRotation;
    Vector3 rayFromPosition;

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
            soundRayChildren[i].transform.rotation = rayRotation * Quaternion.AngleAxis(startAngle + splitAngleDegrees * i, transform.up);
            soundRayChildren[i].transform.position = rayFromPosition;
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


    public override void SoundRayHit(Ray.SoundRayHit soundRayHit, GameObject fromObject)
    {
        hitLastFrame = true;
        EnableSoundRays();
        // raycast backwards to get surface

        Vector3 rayCastFrom = transform.position - (soundRayHit.hitPoint - transform.position) * 2;

        RaycastHit[] hits = Physics.RaycastAll(rayCastFrom, (transform.position - rayCastFrom).normalized, (transform.position - rayCastFrom).magnitude);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == transform)
            {
                // is this object
                rayFromPosition = hit.point;
                rayRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            }
        }
        ;
    }

}
