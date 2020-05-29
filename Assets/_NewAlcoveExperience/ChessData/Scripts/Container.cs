using UnityEngine;
using System.Collections;

public class Container : MonoBehaviour
{
    public Move move;
    GameManager manager;
    private ObjectPointer objectPointer;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
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
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && move != null)
        {
            manager.SwapPieces(move);          
        }
    }
}
