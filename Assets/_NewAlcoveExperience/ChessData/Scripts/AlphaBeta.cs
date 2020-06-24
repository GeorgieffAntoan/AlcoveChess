using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class AlphaBeta
{
    int maxDepth = 4;

    List<Move> _moves = new List<Move>();
    List<Tile> _tilesWithPieces = new List<Tile>();
    List<Tile> _blackPieces = new List<Tile>();
    List<Tile> _whitePieces = new List<Tile>();
    Stack<Move> moveStack = new Stack<Move>();
    Weights _weight = new Weights();
    Tile[,] _localBoard = new Tile[8, 8];
    int _whiteScore = 0;
    int _blackScore = 0;
    Move bestMove;

    Board _board;

    public Move GetMove()
    {
        _board = Board.Instance;
        bestMove = _CreateMove(_board.GetTileFromBoard(new int2(0, 0)), _board.GetTileFromBoard(new int2(0, 0)));
        AB(maxDepth, -100000000, 1000000000, true);
        return bestMove;
    }
    int AB(int depth, int alpha, int beta, bool max)
    {
        _GetBoardState();
        if (depth == 0)
        {
            return _Evaluate();
        }
        if (max)
        {
            int score = -10000000;
            List<Move> allMoves = _GetMoves(Piece.playerColor.BLACK);
            for (int i = 0; i < allMoves.Count; i++)
            {
                moveStack.Push(allMoves[i]);

                _DoFakeMove(allMoves[i].firstPosition, allMoves[i].secondPosition);

                score = AB(depth - 1, alpha, beta, false);

                _UndoFakeMove();

                if (score > alpha)
                {
                    allMoves[i].score = score;
                    if (allMoves[i].score > bestMove.score && depth == maxDepth)
                    {
                        bestMove = allMoves[i];
                    }
                    alpha = score;
                }
                if (score >= beta)
                {
                    break;
                }
            }
            return alpha;
        }
        else
        {
            int score = 10000000;
            List<Move> allMoves = _GetMoves(Piece.playerColor.WHITE);
            for (int i = 0; i < allMoves.Count; i++)
            {
                moveStack.Push(allMoves[i]);

                _DoFakeMove(allMoves[i].firstPosition, allMoves[i].secondPosition);

                score = AB(depth - 1, alpha, beta, true);
                _UndoFakeMove();

                if (score < beta)
                {
                    allMoves[i].score = score;
                    beta = score;
                }
                if (score <= alpha)
                {
                    break;
                }
            }
            return beta;

        }
    }
    IEnumerator undoFake()
    {
        yield return new WaitForSeconds(.3f);

    }

    void _UndoFakeMove()
    {
        Move tempMove = moveStack.Pop();
        Tile movedTo = tempMove.secondPosition;
        Tile movedFrom = tempMove.firstPosition;
        Piece pieceKilled = tempMove.pieceKilled;
        Piece pieceMoved = tempMove.pieceMoved;

        movedFrom.CurrentPiece = movedTo.CurrentPiece;

        if (pieceKilled != null)
        {
            movedTo.CurrentPiece = pieceKilled;
        }
        else
        {
            movedTo.CurrentPiece = null;
        }
    }

    void _DoFakeMove(Tile currentTile, Tile targetTile)
    {
        targetTile.SwapFakePieces(currentTile.CurrentPiece);
        currentTile.CurrentPiece = null;
    }

    List<Move> _GetMoves(Piece.playerColor color)
    {
        List<Move> turnMove = new List<Move>();
        List<Tile> pieces = new List<Tile>();

        if (color == Piece.playerColor.BLACK)
            pieces = _blackPieces;
        else pieces = _whitePieces;

        for (int i = 0; i < pieces.Count; i++)
        {
            MoveFactory factory = new MoveFactory(_board);
            List<Move> pieceMoves = factory.GetMoves(pieces[i].CurrentPiece, pieces[i].Position);

            for (int j = 0; j < pieceMoves.Count; j++)
            {
                Move newMove = _CreateMove(pieceMoves[j].firstPosition, pieceMoves[j].secondPosition);
                turnMove.Add(newMove);
            }
        }
        return turnMove;
    }

    int _Evaluate()
    {
        float pieceDifference = 0;
        float whiteWeight = 0;
        float blackWeight = 0;

        foreach (Tile tile in _whitePieces)
        {
            whiteWeight += _weight.GetBoardWeight(tile.CurrentPiece.Type, tile.CurrentPiece.position, Piece.playerColor.WHITE);
        }
        foreach (Tile tile in _blackPieces)
        {
            blackWeight += _weight.GetBoardWeight(tile.CurrentPiece.Type, tile.CurrentPiece.position, Piece.playerColor.BLACK);
        }
        pieceDifference = (_blackScore + (blackWeight / 100)) - (_whiteScore + (whiteWeight / 100));
        return Mathf.RoundToInt(pieceDifference * 100);
    }

    void _GetBoardState()
    {
        _blackPieces.Clear();
        _whitePieces.Clear();
        _blackScore = 0;
        _whiteScore = 0;
        _tilesWithPieces.Clear();

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                _localBoard[x, y] = _board.GetTileFromBoard(new int2(x, y));
                if (_localBoard[x, y].CurrentPiece != null && _localBoard[x, y].CurrentPiece.Type != Piece.pieceType.UNKNOWN)
                {
                    _tilesWithPieces.Add(_localBoard[x, y]);
                }
            }
        }
        for (int i = 0; i < _tilesWithPieces.Count; i++)
        {
            if (_tilesWithPieces[i].CurrentPiece.Player == Piece.playerColor.BLACK)
            {
                _blackScore += _weight.GetPieceWeight(_tilesWithPieces[i].CurrentPiece.Type);
                _blackPieces.Add(_tilesWithPieces[i]);
            }
            else
            {
                _whiteScore += _weight.GetPieceWeight(_tilesWithPieces[i].CurrentPiece.Type);
                _whitePieces.Add(_tilesWithPieces[i]);
            }
        }
    }

    Move _CreateMove(Tile tile, Tile move)
    {
        Move tempMove = new Move();
        tempMove.firstPosition = tile;
        tempMove.pieceMoved = tile.CurrentPiece;
        tempMove.secondPosition = move;

        if (move.CurrentPiece != null)
        {
            tempMove.pieceKilled = move.CurrentPiece;
        }

        return tempMove;
    }
}
