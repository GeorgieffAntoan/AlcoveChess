using UnityEngine;
using System.Collections;

public class Overlays : MonoBehaviour
{
    private ObjectPointer objectPointer;

    void Start()
    {
        objectPointer = GameObject.Find("Laser").GetComponent<ObjectPointer>();
    }

    void Update()
    {
        if (objectPointer.go.name.Equals(this.name))
        {
            Action();
        }
    }

    void Action()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
            foreach (GameObject o in objects)
            {
                Destroy(o);
            }
        }
    }
}
