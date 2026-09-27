using UnityEngine;

public class ItemKiller : MonoBehaviour
{

    public LayerMask killLayers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if ((killLayers & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(other.gameObject);
        }
    }
}
