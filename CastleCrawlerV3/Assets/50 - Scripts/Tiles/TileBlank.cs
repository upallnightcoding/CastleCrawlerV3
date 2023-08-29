using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlank : TileBase
{
    private GameData gameData = null;

    public TileBlank(GameData gameData)
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
        return ("Test");
    }

    public override bool IsTileOpen()
    {
        return (true);
    }
}
