using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMeMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody>().IsSleeping()) gameObject.GetComponent<Rigidbody>().isKinematic = false;
        else gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "White") Destroy(gameObject);

    }
}
