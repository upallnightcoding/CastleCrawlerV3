using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePosition 
{
    public int Col { get; private set; }
    public int Row { get; private set; }

    public TilePosition(int col, int row)
    {
        this.Col = col;
        this.Row = row;
    }

    public TilePosition(TilePosition tile) : this(tile.Col, tile.Row)
    {

    }
}
