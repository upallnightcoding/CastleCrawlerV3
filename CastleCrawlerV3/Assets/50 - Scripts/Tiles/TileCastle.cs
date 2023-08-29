using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCastle : TileBase
{
    private GameData gameData = null;

    public TileCastle(GameData gameData)
    {
        this.gameData = gameData;
    }

    public override Material GetBackGround()
    {
        return (gameData.TileColorWhite);
    }

    public override Sprite GetIcon()
    {
        return (gameData.castleSprite);
    }

    public override string GetPrompt()
    {
        return ("End");
    }

    public override bool IsTileOpen()
    {
        return (false);
    }
}
