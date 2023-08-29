using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePath : TileBase
{
    private GameData gameData = null;

    public TilePath(GameData gameData)
    {
        this.gameData = gameData;
    }

    public override Material GetBackGround()
    {
        return (gameData.TileColorGray);
    }

    public override Sprite GetIcon()
    {
        return (null);
    }

    public override string GetPrompt()
    {
        return ("Path");
    }

    public override bool IsTileOpen()
    {
        return (false);
    }
}
