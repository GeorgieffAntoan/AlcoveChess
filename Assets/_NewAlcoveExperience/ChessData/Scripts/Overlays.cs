using UnityEngine;
using System.Collections;

public class Overlays : MonoBehaviour
{
    private ObjectPointer objectPointer;
    private ObjectPointer objectPointer1;
    public PhotonView photonView;


    void Start()
    {
        objectPointer = GameObject.Find("Laser").GetComponent<ObjectPointer>();
        objectPointer1 = GameObject.Find("Laser").GetComponent<ObjectPointer>();
    }
    
    public void Action()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject o in objects)
        {
           Destroy(o);
        }
    }

    public void Action2()
    {
       
    }
}
