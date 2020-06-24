using UnityEngine;
using System.Collections;
using System.IO;
using ExitGames.Client.Photon;
using BayatGames.Serialization.Formatters.Binary;
using System;

[System.Serializable]
public class Move
{
    public Tile firstPosition = null;
    public Tile secondPosition = null;
    public Piece pieceMoved = null;
    public Piece pieceKilled = null;
    public int score = -100000000;
}
