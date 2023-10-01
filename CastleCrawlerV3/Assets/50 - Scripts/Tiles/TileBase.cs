using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TileBase
{
    public Sprite GetBackGround();

    public Sprite GetForeGroundImage();

    public string GetPrompt();

    public bool IsTileOpen();

    public StepValidType GetTileBlocking();

    public bool IsSupportProp();

    public void Animation(TileMngr tileMngr, TilePosition position, Sprite color, bool firstCall);

    public bool IsShowing();
}
