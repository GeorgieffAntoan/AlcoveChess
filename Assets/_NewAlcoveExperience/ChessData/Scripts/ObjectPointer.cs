using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPointer : MonoBehaviour
{
  /*  public GameObject pointer;

    public GameObject go;

    RaycastHit hit;



    void Start()
    {
        GameObject go = null;
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider != null)
            {
                go = hit.transform.gameObject;
                //    go.tag = "tagged
                //  if (go.name=="MoveCube(Clone)" || go.name=="CurrentPiece(Clone)")go.tag = "Highlight";
                Debug.Log(go);
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && go.name == "MoveCube(Clone)")
                {
                    go.tag = "tagged";
                    GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
                    foreach (GameObject o in objects)
                    {
                        Destroy(o);
                    }
                    go.tag = "Highlight";
                    GetPointedGo().GetComponent<Piece>()?.Action();
                    }
            }
        }
        

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            GetPointedGo().GetComponent<Piece>()?.Action();
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
            foreach (GameObject o in objects)
            {
                Destroy(o);
            }
        }
    }
    public GameObject GetPointedGo()
    {
        return go;
    }  */
}
