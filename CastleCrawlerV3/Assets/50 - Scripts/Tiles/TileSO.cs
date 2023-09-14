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
    public bool isTileInPlay;
    public bool isSupportProp;
    public GameObject animation;
    public bool isShowing;

    public virtual Sprite GetBackGround() => backGround;

    public virtual string GetPrompt() => prompt;

    public virtual Sprite GetForeGroundImage() => image;

    public virtual bool IsTileOpen() => isTileOpen;

    public virtual bool IsTileInPlay() => isTileInPlay;

    public virtual bool IsSupportProp() => isSupportProp;

    public virtual bool IsShowing() => isShowing;

    public GameObject Animate(TilePosition position)
    {
        GameObject go = null;

        if (animation != null)
        {
            go = Object.Instantiate(
                animation,
                position.GetTilePos() + new Vector3(0.0f, 0.02f, 0.0f),
                Quaternion.identity
            );
        }

        return (go);
    }
}
