using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[RequireComponent(typeof(Rigidbody))]

public class SoundProjectile : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private List<string> patternofsound = new List<string>();

    

    public float speed;
    public Vector3 direction;
    public int collicount;
    public string patternhold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.linearVelocity = speed * direction;
        collicount = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }


    void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.TryGetComponent(out SoundSurface soundSurface))
        {
            soundSurface.OnSoundCollision(collision);
            //patternhold = soundSurface.material.ToString();
            patternofsound.Add(soundSurface.material.ToString());
            print(string.Join(", ", patternofsound));

            if (string.Join(", ", patternofsound) == "Concrete, Wood, Metal")
            {
                print("u did it");
            }

        }
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        rigidbody.linearVelocity = speed * direction;

      


    }
    
}
