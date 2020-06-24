using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using Photon;
using UnityEngine.Scripting;
using System;

public class GameManager : Photon.PunBehaviour
{
    AlphaBeta ab = new AlphaBeta();
    private bool _kingDead = false;
    float timer = 0;
    Board _board;
    public GameObject endPanel;
    public GameObject me;
    public AudioSource playMe;
    public GameObject startPanel;
    PhotonView photonView2;
    PhotonView photonView13;
    public GameObject chessPieceWhite;
    public GameObject chessPieceBlack;
    Tile firstTile;
    Tile secondTile;
    [SerializeField] private float maxTimeBetweenGarbageCollections = 15f;
    private float _timeSinceLastGarbageCollection;


    //  GameObject laser;
    public bool sngP = true;

    void Start()
    {
        OVRManager.display.displayFrequency = 72.0f;
        _board = Board.Instance;
        _board.SetupBoard();
        playMe.Play();
        //  laser = GameObject.Find("Laser");
        //set my team
        if (PhotonNetwork.isMasterClient)
            myTeam = "WHITE"; //host is always white
        else
            myTeam = "BLACK";
        startPanel.SetActive(true);
        playerTurn = true;
    }

    public int requiredPlayers = 2;
    public string myTeam = "";

    //Room custom property changed (turn change)
    public void OnPhotonCustomRoomPropertiesChanged(Hashtable changedProps)
    {
        if (changedProps.ContainsKey("TURN"))
        {
            //turn string is equal to the team whose turn it is
            string turn = (string)changedProps["TURN"];
            //run the start turn function
            StartTurn(turn);
        }
    }

    void StartTurn(string team)
    {
        //it is my turn
        if (team == myTeam)
        {
            //i am white team
            if (myTeam == "WHITE")
            {
                GameObject.Find("WhitePieces").layer = LayerMask.NameToLayer("PhysicsInteractable");
                foreach (Transform child in GameObject.Find("WhitePieces").GetComponentsInChildren<Transform>(true))
                {
                    child.gameObject.layer = LayerMask.NameToLayer("PhysicsInteractable");
                }
                GameObject.Find("BlackPieces").layer = LayerMask.NameToLayer("Ignore Raycast");
                foreach (Transform child in GameObject.Find("BlackPieces").GetComponentsInChildren<Transform>(true))
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
            }
            else if (myTeam == "BLACK")
            {
                GameObject.Find("WhitePieces").layer = LayerMask.NameToLayer("Ignore Raycast");
                foreach (Transform child in GameObject.Find("WhitePieces").GetComponentsInChildren<Transform>(true))
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
                GameObject.Find("BlackPieces").layer = LayerMask.NameToLayer("PhysicsInteractable");
                foreach (Transform child in GameObject.Find("BlackPieces").GetComponentsInChildren<Transform>(true))
                {
                    child.gameObject.layer = LayerMask.NameToLayer("PhysicsInteractable");
                }
            }
        }
        //it is not my turn
        else
        {
            GameObject.Find("WhitePieces").layer = LayerMask.NameToLayer("Ignore Raycast");
            foreach (Transform child in GameObject.Find("WhitePieces").GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
            GameObject.Find("BlackPieces").layer = LayerMask.NameToLayer("Ignore Raycast");
            foreach (Transform child in GameObject.Find("BlackPieces").GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
        }
    }

    //call this function for the local player when from a gameplay logic script,
    //like when a player moves his chess piece or something
    public void EndTurn()
    {
        //set the TURN room property to be the other players team,
        //this will trigger the StartTurn function call in OnRoomPropertiesUpdated,
        //starting the next turn
        string otherTeam = (myTeam == "WHITE") ? "BLACK" : "WHITE";
        Hashtable props = new Hashtable { { "TURN", myTeam } };
        PhotonNetwork.room.SetCustomProperties(props);
    }

    void Update()
    {
        if (_kingDead)
        {
            endPanel.SetActive(true);
            me.SetActive(false);
        }
        if (!playerTurn && timer < 2)
        {
            timer += Time.deltaTime;
        }
        else if (!playerTurn && timer >= 2)
        {
            if (sngP)
            {
                Move move = ab.GetMove();
                _DoAIMove(move);
            }
            timer = 0;
        }
        _timeSinceLastGarbageCollection += Time.unscaledDeltaTime;
        if (_timeSinceLastGarbageCollection > maxTimeBetweenGarbageCollections)
        {
            GC.Collect();
            _timeSinceLastGarbageCollection = 0f;
        }
      // if (firstTile.CurrentPiece.position.x == secondTile.CurrentPiece.position.x) Destroy(secondTile.CurrentPiece.gameObject);
        if (PhotonNetwork.isMasterClient)
            myTeam = "WHITE"; //host is always white
        else
            myTeam = "BLACK";
    }

    public bool playerTurn = true;

    void _DoAIMove(Move move)
    {
        Tile firstPosition = move.firstPosition;
        Tile secondPosition = move.secondPosition;

        if (secondPosition.CurrentPiece && secondPosition.CurrentPiece.Type == Piece.pieceType.KING)
        {
            SwapPieces(move);
            _kingDead = true;
        }
        else
        {
            SwapPieces(move);
        }
    }

    public void Click()
    {
    }

    public override void OnOwnershipRequest(object[] viewAndPlayer)
    {
        PhotonView view = viewAndPlayer[0] as PhotonView;
        PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;

        photonView.TransferOwnership(requestingPlayer);
    }

    [PunRPC]
    public void SwapPieces(Move move)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject o in objects)
        {
            Destroy(o);
        }
    
         firstTile = move.firstPosition;
         secondTile = move.secondPosition;

        firstTile.CurrentPiece.MovePiece(new int2(move.secondPosition.Position.x, move.secondPosition.Position.y));

        if (secondTile.CurrentPiece != null)
        {
            if (secondTile.CurrentPiece.Type == Piece.pieceType.KING)
                _kingDead = true;
            Destroy(secondTile.CurrentPiece.gameObject);

        }
        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.position = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;
      //  EndTurn();
        if (sngP)    playerTurn = !playerTurn;

        //  Destroy(secondTile.CurrentPiece.gameObject);
        if (!sngP)
        {
            photonView2 = GetComponent<PhotonView>();
            photonView2.RPC("Turn", PhotonTargets.AllBuffered, playerTurn);
        }
    }



    [PunRPC]
    public void Turn(bool a)
    {
        a = !a;
        return;
    } 

    public void SinglePlayer()
    {
        photonView2 = GetComponent<PhotonView>();
        photonView2.RPC("Single", PhotonTargets.AllBuffered);
    }


    [PunRPC]
    public void Single()
    {
        sngP = true;
        startPanel.SetActive(false);
        //  chessPieceBlack.SetActive(true);
        //  chessPieceWhite.SetActive(true);
      //  PhotonNetwork.Instantiate("Board", new Vector3(3.61f, 0f, -3.29f), Quaternion.identity, 1);
    }

    public void MultiPlayer()
    {
        photonView2 = GetComponent<PhotonView>();
        photonView2.RPC("Multi", PhotonTargets.AllBuffered);
    }
    [PunRPC]
    public void Multi()
    {
        sngP = false;
        startPanel.SetActive(false);
        //chessPieceBlack.SetActive(true);
        // chessPieceWhite.SetActive(true);
     //   PhotonNetwork.Instantiate("Board", new Vector3(3.61f, 0f, -3.29f), Quaternion.identity, 1);

    }
}
