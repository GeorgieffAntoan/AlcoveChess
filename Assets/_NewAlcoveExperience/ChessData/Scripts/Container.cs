using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

using System.Collections;

public class Container : MonoBehaviour
{
    public Move move;
    GameManager manager;
    private ObjectPointer objectPointer;
    PhotonView photonView;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
       objectPointer = GameObject.Find("Laser").GetComponent<ObjectPointer>();
    }

    void Update()
    {
       // if (objectPointer.go.name.Equals(this.name))
     //   {
        //    Action();
       // }
    }
    public void Action()
    {
        Debug.Log("clicking");
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))// && move != null)
        {
            Debug.Log("should move");
            manager.SwapPieces(move);          
        }
    }
}
