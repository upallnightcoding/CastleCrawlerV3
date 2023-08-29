using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCrown : TileBase
{
    private GameData gameData = null;

    public TileCrown(GameData gameData)
    {
        this.gameData = gameData;
    }

    public override Material GetBackGround()
    {
        return (gameData.TileColorWhite);
    }

    public override Sprite GetIcon()
    {
        return (gameData.crownSprite);
    }

    public override string GetPrompt()
    {
        return ("Start");
    }

    public override bool IsTileOpen()
    {
        return (false);
    }
}
