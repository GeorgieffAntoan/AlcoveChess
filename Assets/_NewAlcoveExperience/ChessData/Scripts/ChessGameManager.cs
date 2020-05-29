using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ChessGameManager : GameManager
{ 
    public int requiredPlayers = 2;
    public string myTeam = "";

    private void Start()
    {
        //set my team
        if (PhotonNetwork.isMasterClient)
            myTeam = "WHITE"; //host is always white
        else
            myTeam = "BLACK";
    }

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
              Debug.Log("myturn");
            }
            else if (myTeam == "BLACK")
            {
                Debug.Log("notmyturn");
                //do stuff here, like enable grabbing on black pieces
            }
        }
        //it is not my turn
        else
        {

            //do stuff here, like disable grabbing on ALL pieces
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
} 
