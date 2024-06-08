using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    private Step[] move = null;
    public string moveName = null;

    public Move(string moveName)
    {
        this.moveName = moveName;

        move = new Step[moveName.Length];

        for (int character = 0; character < moveName.Length; character++)
        {
            switch (moveName.Substring(character, 1))
            {
                case "N":
                    move[character] = GameData.NORTH_STEP;
                    break;
                case "S":
                    move[character] = GameData.SOUTH_STEP;
                    break;
                case "E":
                    move[character] = GameData.EAST_STEP;
                    break;
                case "W":
                    move[character] = GameData.WEST_STEP;
                    break;
            }
        }
    }

    public void DebugIt()
    {
        //Debug.Log($"Move: {moveName}");
    }

    public TilePosition IsValid(TilePosition beginTile, TileMngr tileMgr)
    {
        bool valid = true;
        TilePosition nextTile = new TilePosition(beginTile);
        Stack<TilePosition> tracking = new Stack<TilePosition>();

        for (int i = 0; (i < move.Length) && valid; i++)
        {
            nextTile.MoveToNextTile(move[i]);

            valid = (nextTile.IsOnBoard() && tileMgr.IsTileOpen(nextTile));

            if (valid)
            {
                tracking.Push(new TilePosition(nextTile));
            }
        }

        if (valid)
        {
            foreach (TilePosition position in tracking)
            {
                tileMgr.PathTile(position);
            }
        }
        else
        {
            foreach (TilePosition position in tracking)
            {
                tileMgr.ResetTile(position);
            }

            nextTile = null;
        }

        return (nextTile);
    }
}
