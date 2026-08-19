using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Holder : MonoBehaviour
{


    public Transform dragToPoint;
    public InputActionAsset Input;

    [Header("Pull")]
    public float maxDistance = 10f;
    public float pullStrength = 1f;

    [Header("Restitution Dampening")]
    public float restitutionRadius = 0.8f;
    public float restitutionStrength = 0.8f;
    public float dampeningCoefficient = 0.95f;


    private Rigidbody heldObject;
    private Vector3 hitPoint;

    private bool heldObjectGravity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        hitPoint = Vector3.zero;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance))
        {
            hitPoint = hit.point;

            if (Input["Hold"].IsPressed())
            {

                if (heldObject == null)
                {
                    AkUnitySoundEngine.PostEvent("Grab_Obj", gameObject);
                    AkUnitySoundEngine.PostEvent("Hold_Obj", gameObject);
                    GameObject objectHit = hit.collider.gameObject;
                    if (objectHit.TryGetComponent(out Rigidbody rb))
                    {
                        heldObject = rb;
                        heldObjectGravity = heldObject.useGravity;
                        heldObject.useGravity = false;
                    }
                }

            }
            else
            {
                //AkUnitySoundEngine.StopAll(gameObject);
                AkUnitySoundEngine.ExecuteActionOnEvent("Hold_Obj", AkActionOnEventType.AkActionOnEventType_Stop, gameObject, 600, AkCurveInterpolation.AkCurveInterpolation_Linear);
                if (heldObject) heldObject.useGravity = heldObjectGravity;
                heldObject = null;
            }

        }
    }

    void FixedUpdate()
    {
        if (heldObject)
        {

            Vector3 direction = dragToPoint.position - heldObject.position;
            float distance = direction.magnitude;
            AkUnitySoundEngine.SetRTPCValue("Object_Held_Velocity", distance);

            Vector3 pullForce = distance * pullStrength * direction.normalized;
            heldObject.AddForce(pullForce, ForceMode.Acceleration);

            // Restitution (dampen closer to target)
            float restitution = Mathf.InverseLerp(0, restitutionRadius, distance) * restitutionStrength + (1 - restitutionStrength);
            heldObject.linearVelocity *= restitution * dampeningCoefficient;

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (hitPoint != Vector3.zero)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(hitPoint, 0.1f);
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireCube(dragToPoint.position, Vector3.one * 0.2f);

    }

}
