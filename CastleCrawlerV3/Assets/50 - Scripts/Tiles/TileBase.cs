using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TileBase
{
    public Sprite GetBackGround();

    public Sprite GetForeGroundImage();

    public string GetPrompt();

    public bool IsTileOpen();

    public StepValidType IsTileBlocked();

    public bool IsSupportProp();

    public IEnumerator BlockedTile(TilePosition position);

    public bool IsShowing();
}
