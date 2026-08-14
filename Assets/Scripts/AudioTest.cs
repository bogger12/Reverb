using UnityEngine;

public class AudioTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AkUnitySoundEngine.PostEvent("debstart", gameObject);
            print("click");
        }
    }
}
