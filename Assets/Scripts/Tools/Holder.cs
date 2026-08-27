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

    [Header("Rotation")]

    public float rotateSensitivity = 1f;
    public float rotationFollow = 0.2f;

    private Rigidbody heldBody;
    private Vector3 hitPoint;
    private Quaternion targetBodyRotation = Quaternion.identity;

    private bool heldBodyGravity;

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

                if (heldBody == null)
                {
                    AkUnitySoundEngine.PostEvent("Grab_Obj", gameObject);
                    AkUnitySoundEngine.PostEvent("Hold_Obj", gameObject);
                    GameObject objectHit = hit.collider.gameObject;
                    if (objectHit.TryGetComponent(out Rigidbody rb))
                    {
                        heldBody = rb;
                        heldBodyGravity = heldBody.useGravity;
                        heldBody.useGravity = false;
                        targetBodyRotation = heldBody.rotation;
                    }
                }

            }
            else
            {
                //AkUnitySoundEngine.StopAll(gameObject);
                AkUnitySoundEngine.ExecuteActionOnEvent("Hold_Obj", AkActionOnEventType.AkActionOnEventType_Stop, gameObject, 600, AkCurveInterpolation.AkCurveInterpolation_Linear);
                if (heldBody)
                {
                    heldBody.useGravity = heldBodyGravity;

                    // (Quaternion.Inverse(heldBody.rotation) * targetBodyRotation * dragToPoint.rotation).ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);
                    // Vector3 angularDisplacement = rotationAxis * angleInDegrees * Mathf.Deg2Rad;
                    // heldBody.angularVelocity = angularDisplacement / (Time.deltaTime * 10);
                    // Debug.Log(heldBody.angularVelocity);
                }
                heldBody = null;
            }

        }
    }

    void FixedUpdate()
    {
        if (heldBody)
        {

            Vector3 direction = dragToPoint.position - heldBody.position;
            float distance = direction.magnitude;
            AkUnitySoundEngine.SetRTPCValue("Object_Held_Velocity", distance);

            Vector3 pullForce = distance * pullStrength * direction.normalized;
            heldBody.AddForce(pullForce, ForceMode.Acceleration);

            // Restitution (dampen closer to target)
            float restitution = Mathf.InverseLerp(0, restitutionRadius, distance) * restitutionStrength + (1 - restitutionStrength);
            heldBody.linearVelocity *= restitution * dampeningCoefficient;

            if (Input["Crouch"].IsPressed())
            {
                Vector2 lookChange = Input["Look"].ReadValue<Vector2>() * rotateSensitivity;

                Quaternion xQuaternion = Quaternion.AngleAxis(lookChange.x, dragToPoint.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(lookChange.y, dragToPoint.right);
                targetBodyRotation = xQuaternion * yQuaternion * targetBodyRotation;

            }
            float restitutionPower = Quaternion.Angle(heldBody.rotation, targetBodyRotation) / 180f;
            float increase = Mathf.Lerp(rotationFollow, 1, restitutionPower * restitutionPower);
            // Rotate towards target rotation
            heldBody.MoveRotation(Quaternion.Slerp(heldBody.rotation, targetBodyRotation, increase));
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

        if (heldBody)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(heldBody.position, targetBodyRotation * Vector3.up);
            Gizmos.color = Color.green;

            Gizmos.DrawRay(heldBody.position, heldBody.rotation * Vector3.up);
        }

    }

}
