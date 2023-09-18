using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "CasteCrawler/Tile/Tile")]
public class TileSO : ScriptableObject, TileBase
{
    public Sprite backGround;
    public Sprite image;
    public string prompt;
    public bool isTileOpen;
    public bool isTileBlocked;
    public bool isSupportProp;
    public GameObject animation;
    public bool isShowing;

    public virtual Sprite GetBackGround() => backGround;

    public virtual string GetPrompt() => prompt;

    public virtual Sprite GetForeGroundImage() => image;

    public virtual bool IsTileOpen() => isTileOpen;

    public virtual bool IsTileBlocked() => isTileBlocked;

    public virtual bool IsSupportProp() => isSupportProp;

    public virtual bool IsShowing() => isShowing;

    public virtual IEnumerator BlockedTile(TilePosition position)
    {
        yield return null;
    }
}
