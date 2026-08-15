using UnityEngine;
using UnityEngine.InputSystem;

public class SoundGun : MonoBehaviour
{
    public Transform shootFrom;
    public float projectileSpeed = 1f;

    public SoundProjectile soundProjectile;

    public InputActionAsset Input;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input["Attack"].WasPressedThisFrame()) OnShoot();
    }

    void OnShoot()
    {
        SoundProjectile newProjectile = GameObject.Instantiate(soundProjectile, shootFrom.position, Quaternion.identity);
        newProjectile.speed = projectileSpeed;
        newProjectile.direction = transform.forward;
    }
}

