using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase
{
    public abstract Material GetBackGround();

    public abstract Sprite GetIcon();

    public abstract string GetPrompt();

    public abstract bool IsTileOpen();
}
