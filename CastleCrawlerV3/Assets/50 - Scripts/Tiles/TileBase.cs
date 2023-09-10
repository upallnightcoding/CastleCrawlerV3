using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TileBase
{
    public Sprite GetBackGround();

    public Sprite GetForeGroundImage();

    public string GetPrompt();

    public bool IsTileOpen();

    public bool IsTileInPlay();

    public bool IsSupportProp();

    public void Animate(TilePosition position);

    public bool IsShowing();
}
