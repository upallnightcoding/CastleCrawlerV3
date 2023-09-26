using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePosition 
{
    public int Col { get; private set; } = 0;
    public int Row { get; private set; } = 0;

    public void Output(string text)
    {
        Debug.Log($"{text}: {Col},{Row}");
    }

    public TilePosition(int col, int row)
    {
        this.Col = col;
        this.Row = row;
    }

    public TilePosition(TilePosition tile) : this(tile.Col, tile.Row)
    {

    }

    public void MoveToNextTile(Step step, bool direction = true)
    {
        Col += step.Col * (direction ? 1 : -1);
        Row += step.Row * (direction ? 1 : -1);
    }

    public bool IsEqual(TilePosition tile)
    {
        return ((tile.Col == Col) && (tile.Row == Row));
    }

    public bool IsOnBoard()
    {
        bool colPos = (Col >= 0) && (Col < BoardCntrl.boardSize);
        bool rowPos = (Row >= 0) && (Row < BoardCntrl.boardSize);

        return (colPos && rowPos);
    }

    //public Vector3 GetTilePos(int x, int z) => new Vector3(x, 0.0f, z);

    public Vector3 GetTilePos() => new Vector3(Col, 0.0f, Row);
}
