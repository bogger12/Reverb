using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoundProjectile : MonoBehaviour
{
    private new Rigidbody rigidbody;

    public float speed;
    public Vector3 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.linearVelocity = speed * direction;
    }

    // Update is called once per frame
    void Update()
    {
    }


    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent(out SoundSurface soundSurface))
        {
            soundSurface.SoundCollide(soundSurface.material, this.gameObject);
        }
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        rigidbody.linearVelocity = speed * direction;
    }
}
