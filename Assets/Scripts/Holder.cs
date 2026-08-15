using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Holder : MonoBehaviour
{


    public Transform dragToPoint;
    public InputActionAsset Input;

    public float maxDistance = 10f;
    public float pullStrength = 1f;

    private Rigidbody heldObject;


    private Vector3 hitPoint;

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

            if (Input["Interact"].IsPressed())
            {

                if (heldObject == null)
                {
                    AkUnitySoundEngine.PostEvent("Hold_Obj", gameObject);
                    GameObject objectHit = hit.collider.gameObject;
                    heldObject = objectHit.GetComponent<Rigidbody>();
                }

            }
            else
            {
                //AkUnitySoundEngine.StopAll(gameObject);
                AkUnitySoundEngine.ExecuteActionOnEvent("Hold_Obj", AkActionOnEventType.AkActionOnEventType_Stop, gameObject, 600, AkCurveInterpolation.AkCurveInterpolation_Linear);
                heldObject = null;
            }

        }
    }

    void FixedUpdate()
    {
        if (heldObject)
        {

            Vector3 direction = dragToPoint.position - heldObject.transform.position;
            float distance = direction.magnitude;
            AkUnitySoundEngine.SetRTPCValue("Object_Held_Velocity", distance); 
            heldObject.AddForce(direction.normalized * pullStrength, ForceMode.Acceleration);

            float fuck = 0.5f;

            float fucklerp = Mathf.InverseLerp(0, fuck, distance);
            heldObject.linearVelocity *= fucklerp * 0.9f;

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
