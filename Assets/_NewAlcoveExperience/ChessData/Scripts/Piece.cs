using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class Piece : MonoBehaviour
{
    public enum pieceType { KING, QUEEN, BISHOP, ROOK, KNIGHT, PAWN, UNKNOWN = -1 };
    public enum playerColor { BLACK, WHITE, UNKNOWN = -1 };

    private ObjectPointer objectPointer;

    [SerializeField] private pieceType _type = pieceType.UNKNOWN;
    [SerializeField] private playerColor _player = playerColor.WHITE;

    public pieceType Type
    {
        get { return _type; }
    }
    public playerColor Player
    {
        get { return _player; }
    }
   
    public Sprite pieceImage = null;
    public int2 position;
    private Vector3 moveTo;
    private GameManager manager;

    private MoveFactory factory = new MoveFactory(Board.Instance);
    private List<Move> moves = new List<Move>();

    private bool _hasMoved = false;
    public bool HasMoved
    {
        get { return _hasMoved; }
        set { _hasMoved = value; }
    }

    [PunRPC]
    public void Action()
    {
       // if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
       // {
            moves.Clear();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
            foreach (GameObject o in objects)
            {
                Destroy(o);
            }
            moves = factory.GetMoves(this, position);
            foreach (Move move in moves)
            {
                if (move.pieceKilled == null)
                {
                    GameObject instance = Instantiate(Resources.Load("MoveCube")) as GameObject;
                    int2 offset = position - move.secondPosition.Position;
                    instance.transform.position = (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z));
                    instance.transform.localScale = transform.lossyScale;
                    instance.transform.rotation = transform.rotation;
                    instance.GetComponent<Container>().move = move;
                    manager.EndTurn();

                }
                else if (move.pieceKilled != null)
                {
                    GameObject instance = Instantiate(Resources.Load("KillCube")) as GameObject;
                    int2 offset = position - move.secondPosition.Position;
                    instance.transform.localScale = transform.lossyScale;
                    instance.transform.rotation = transform.rotation;
                    instance.transform.position = (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z));
                    instance.GetComponent<Container>().move = move;
                    manager.EndTurn();

                }
            }
            GameObject i = Instantiate(Resources.Load("CurrentPiece")) as GameObject;
            i.transform.position = this.transform.position;
            i.transform.localScale = transform.lossyScale;
            i.transform.rotation = transform.rotation;
      //  }
        /*    if (!manager.sngP)
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && _player2 == playerColor.BLACK)
                {
                    moves.Clear();
                    GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
                    foreach (GameObject o in objects)
                    {
                        Destroy(o);
                    }
                    moves = factory.GetMoves(this, position);
                    foreach (Move move in moves)
                    {
                        if (move.pieceKilled == null)
                        {
                            GameObject instance = Instantiate(Resources.Load("MoveCube")) as GameObject;
                            int2 offset = position - move.secondPosition.Position;
                            instance.transform.position = (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z));
                            instance.transform.localScale = transform.lossyScale;
                            instance.transform.rotation = transform.rotation;
                            instance.GetComponent<Container>().move = move;
                        }
                        else if (move.pieceKilled != null)
                        {
                            GameObject instance = Instantiate(Resources.Load("KillCube")) as GameObject;
                            int2 offset = position - move.secondPosition.Position;
                            instance.transform.localScale = transform.lossyScale;
                            instance.transform.rotation = transform.rotation;
                            instance.transform.position = (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z));
                            instance.GetComponent<Container>().move = move;
                        }
                    }
                    GameObject i = Instantiate(Resources.Load("CurrentPiece")) as GameObject;
                    i.transform.position = this.transform.position;
                    i.transform.localScale = transform.lossyScale;
                    i.transform.rotation = transform.rotation;
                }
            }
        } */
    }

    public void MovePiece(int2 pos)
    {
        int2 offset = position - pos;     
        moveTo = transform.localPosition + new Vector3(-offset.x, 0, offset.y);
    }

    void Start()
    {
        transform.localPosition = new Vector3(position.x, 0, -position.y);
        moveTo = this.transform.localPosition;
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
       objectPointer = GameObject.Find("Laser").GetComponent<ObjectPointer>();
 
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(this.transform.localPosition, moveTo, 3 * Time.deltaTime);
        if (/*objectPointer != null && objectPointer.go == gameObject &&*/ OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            PhotonView photonView = GetComponent<PhotonView>();
          //  photonView.RPC("Action", PhotonTargets.AllBuffered);
         //   Action();
        }
    }
}
