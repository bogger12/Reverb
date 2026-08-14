using System.IO;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Raycast : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public int maxBounces = 3;

    public float maxDistance = 200f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = transform.position + transform.up * 0.05f + transform.right * 0.15f;
        Vector3 direction = transform.forward;

        List<Vector3> points = new List<Vector3>();
        points.Add(origin);

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
            {
                points.Add(hit.point);
                direction = Vector3.Reflect(direction, hit.normal);

                origin = hit.point + direction * 0.001f;
            }
            else
            {
                points.Add(origin + direction * maxDistance);
                break;
            }
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

    }

    void OnDrawGizmos()
    {
        if (!lineRenderer) return;
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        for (int i = 1; i < lineRenderer.positionCount; i++)
        {
            Gizmos.DrawWireSphere(positions[i], 0.5f);
        }

    }
}

