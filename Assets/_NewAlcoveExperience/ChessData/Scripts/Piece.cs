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
    private ObjectPointer objectPointer1;


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
    PhotonView photonView;
    private MoveFactory factory = new MoveFactory(Board.Instance);
    private List<Move> moves = new List<Move>();
    int counter = 100;

    private bool _hasMoved = false;
    public bool HasMoved
    {
        get { return _hasMoved; }
        set { _hasMoved = value; }
    }
    [PunRPC]
    public void Action2()
    {
        if (_player == playerColor.WHITE && PhotonNetwork.isMasterClient)
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
                    //GameObject instance = PhotonNetwork.Instantiate("MoveCube", (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z)), transform.rotation, 1);

                    instance.transform.position = (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z));
                    instance.transform.localScale = transform.lossyScale / 1.01f;
                    instance.transform.rotation = transform.rotation;
                    instance.GetComponent<Container>().move = move;
                    PhotonView[] nViews = instance.GetComponents<PhotonView>();
                    nViews[0].viewID = counter;
                    counter++;
                    //  manager.EndTurn();

                }
                else if (move.pieceKilled != null)
                {
                    GameObject instance = Instantiate(Resources.Load("KillCube")) as GameObject;
                    int2 offset = position - move.secondPosition.Position;
                    //   GameObject instance = PhotonNetwork.Instantiate("KillCube", (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z)), transform.rotation, 1);

                    instance.transform.localScale = transform.lossyScale / 1.01f;
                    instance.transform.rotation = transform.rotation;
                    instance.transform.position = (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z));
                    instance.GetComponent<Container>().move = move;
                    PhotonView[] nViews4 = instance.GetComponents<PhotonView>();
                    nViews4[0].viewID = counter;
                    counter++;
                    //   manager.EndTurn();


                }
            }
            GameObject i = Instantiate(Resources.Load("CurrentPiece")) as GameObject;
            //   GameObject i = PhotonNetwork.Instantiate("CurrentPiece", this.transform.position, transform.rotation, 1);

            i.transform.position = this.transform.position;
            i.transform.localScale = transform.lossyScale / 1.01f;
            i.transform.rotation = transform.rotation;
            PhotonView[] nViews6 = i.GetComponents<PhotonView>();
            nViews6[0].viewID = counter;
  
        }
        if (!manager.sngP && !PhotonNetwork.isMasterClient && _player == playerColor.BLACK)
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
                    //GameObject instance = PhotonNetwork.Instantiate("MoveCube", (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z)), transform.rotation, 1);
                    instance.transform.position = (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z));
                    instance.transform.localScale = transform.lossyScale / 1.01f;
                    instance.transform.rotation = transform.rotation;
                    instance.GetComponent<Container>().move = move;
                    manager.EndTurn();
                    PhotonView[] nViews2 = instance.GetComponents<PhotonView>();
                    nViews2[0].viewID = counter;
                    counter++;

                }
                else if (move.pieceKilled != null)
                {
                    GameObject instance = Instantiate(Resources.Load("KillCube")) as GameObject;
                    int2 offset = position - move.secondPosition.Position;
                    //  GameObject instance = PhotonNetwork.Instantiate("KillCube", (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z)), transform.rotation, 1);
                    instance.transform.localScale = transform.lossyScale / 1.01f;
                    instance.transform.rotation = transform.rotation;
                    instance.transform.position = (transform.position + new Vector3(offset.x * transform.lossyScale.x, 0, -offset.y * transform.lossyScale.z));
                    instance.GetComponent<Container>().move = move;
                    manager.EndTurn();
                    PhotonView[] nViews3 = instance.GetComponents<PhotonView>();
                    nViews3[0].viewID = counter;
                    counter++;

                }
            }
            GameObject i = Instantiate(Resources.Load("CurrentPiece")) as GameObject;
            //   GameObject i = PhotonNetwork.Instantiate("CurrentPiece", this.transform.position, transform.rotation, 1);
            i.transform.position = this.transform.position;
            i.transform.localScale = transform.lossyScale / 1.01f;
            i.transform.rotation = transform.rotation;
            PhotonView[] nViews5 = i.GetComponents<PhotonView>();
            nViews5[0].viewID = counter;
            counter++;
        }
    }

    [PunRPC]
    public void Action()
    {
        PhotonView photonView = this.GetComponent<PhotonView>();
        photonView.RPC("Action2", PhotonTargets.AllBuffered);
      
    }

    [PunRPC]
    public void Turn(bool a)
    {
        a = !a;
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
        objectPointer1 = GameObject.Find("Laser").GetComponent<ObjectPointer>();
        me = true;
    }

    public bool me = true;
    void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(this.transform.localPosition, moveTo, 3 * Time.deltaTime);
    }
}
