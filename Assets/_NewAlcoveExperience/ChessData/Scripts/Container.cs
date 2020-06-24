using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

using System.Collections;

public class Container : MonoBehaviour
{
    public Move move;
    GameManager manager;
    private ObjectPointer objectPointer;
    private ObjectPointer objectPointer1;


    PhotonView photonView;

    void Start()
    {
       manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
       objectPointer = GameObject.Find("Laser").GetComponent<ObjectPointer>();
        objectPointer1 = GameObject.Find("Laser").GetComponent<ObjectPointer>();

    }

    void Update()
    {   
    }
    public void Action()
    {
        manager.SwapPieces(move);

     // photonView = GetComponent<PhotonView>();
     // photonView.RPC("SwapPieces", PhotonTargets.AllBuffered, move);
    }
    
    public void SwapMe()
    {
        manager.SwapPieces(move);
    }
}
