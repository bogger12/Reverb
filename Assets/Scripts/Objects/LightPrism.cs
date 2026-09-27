using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LightPrism : SoundSurface
{

    public const int linesToSplit = 2;
    public float splitAngleDegrees = 20;

    public GameObject lineChild;

    private SoundRay[] soundRayChildren;

    Quaternion rayRotation;
    Vector3 rayFromPosition;

    Dictionary<int, SoundRay[]> fromObjectToLineChildren = new Dictionary<int, SoundRay[]>();

    List<int> fromObjectStayingThisFrame = new List<int>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!lineChild || !lineChild.TryGetComponent(out LineRenderer _) || !lineChild.TryGetComponent(out SoundRay _))
        {
            Debug.LogError("LightPrism needs a child with LineRenderer and SoundRay assigned");
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (int objectId in fromObjectToLineChildren.Keys.ToList())
        {
            if (!fromObjectStayingThisFrame.Contains(objectId))
            {
                foreach (SoundRay sr in fromObjectToLineChildren[objectId])
                {
                    Destroy(sr.gameObject);
                }
                fromObjectToLineChildren.Remove(objectId);
            }
        }
        fromObjectStayingThisFrame = new List<int>();
    }

    public override void SoundRayHit(Ray.SoundRayHit soundRayHit, GameObject fromObject)
    {
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

        // Create or sustain children

        float startAngle = -splitAngleDegrees * (Mathf.Floor(linesToSplit / 2) + (linesToSplit % 2 == 0 ? 0.5f : 0f) - 1f);

        Debug.DrawRay(rayFromPosition, rayRotation * Vector3.forward);

        int fromObjectInstanceId = fromObject.GetInstanceID();
        if (fromObjectToLineChildren.ContainsKey(fromObjectInstanceId))
        {
            fromObjectStayingThisFrame.Add(fromObjectInstanceId);

            for (int i = 0; i < linesToSplit; i++)
            {
                fromObjectToLineChildren[fromObjectInstanceId][i].transform.SetPositionAndRotation(rayFromPosition, rayRotation * Quaternion.AngleAxis(startAngle + splitAngleDegrees * i, Vector3.up));
            }
        }
        else
        {
            SoundRay[] soundRaysToAdd = new SoundRay[linesToSplit];
            // create line renderer children, assign to array
            for (int i = 0; i < linesToSplit; i++)
            {
                GameObject newLineObject = Instantiate(lineChild, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
                SoundRay soundRay = newLineObject.GetComponent<SoundRay>();
                soundRaysToAdd[i] = soundRay;
                soundRay.tone = soundRayHit.tone;

                soundRay.transform.SetPositionAndRotation(rayFromPosition, rayRotation * Quaternion.AngleAxis(startAngle + splitAngleDegrees * i, Vector3.up));
            }
            fromObjectToLineChildren.Add(fromObjectInstanceId, soundRaysToAdd);
            fromObjectStayingThisFrame.Add(fromObjectInstanceId);
        }

    }

}
