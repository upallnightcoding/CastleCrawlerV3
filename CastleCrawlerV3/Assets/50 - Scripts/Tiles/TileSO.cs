using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "CasteCrawler/Tile")]
public class TileSO : ScriptableObject, TileBase
{
    public Sprite backGround;
    public Sprite image;
    public string prompt;
    public bool isTileOpen;

    public virtual Sprite GetBackGround() => backGround;

    public virtual string GetPrompt() => prompt;

    public virtual Sprite GetSprite() => image;

    public virtual bool IsTileOpen() => isTileOpen;
}
