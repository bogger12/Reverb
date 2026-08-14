using UnityEngine;
using UnityEngine.InputSystem;


public class Holder : MonoBehaviour
{


    public Transform dragToPoint;
    public InputActionAsset Input;

    public float maxDistance = 10f;
    public float pullStrength = 1f;

    private Rigidbody heldObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input["Interact"].IsPressed())
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance))
            {
                Debug.Log("ray hit at");

                GameObject objectHit = hit.collider.gameObject;
                heldObject = objectHit.GetComponent<Rigidbody>();
                Debug.Log(heldObject);
            }
        }
    }

    void FixedUpdate()
    {
        if (heldObject)
        {
            Vector3 direction = (heldObject.transform.position - dragToPoint.position).normalized;
            heldObject.AddForce(direction * pullStrength);
        }
    }
}
