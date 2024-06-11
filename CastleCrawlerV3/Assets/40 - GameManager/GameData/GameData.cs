using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "CasteCrawler/Game Data")]
public class GameData : ScriptableObject
{
    [Header("Constants")]
    public static readonly Step NORTH_STEP = new Step(0, 1);
    public static readonly Step SOUTH_STEP = new Step(0, -1);
    public static readonly Step EAST_STEP = new Step(1, 0);
    public static readonly Step WEST_STEP = new Step(-1, 0);

    [Header("Game Attributes")]
    public int safeGuardLimit;
    public int level;
    public int maxLevel;
    public bool debugSw;
    public int boardSize;

    [Header("Props Count")]
    public int numberShields;
    public int numberBombs;
    public int numberHearts;

    [Header("Tile Background Colors")]
    public Material TileColorGray;
    public Material TileColorWhite;

    [Header("Game Sprites")]
    public TileSO tileBlankSO;
    public TileSO tileCastleSO;
    public TileSO tileCrownSO;
    public TileSO tilePathSO;
    public TileSO tileBombSO;
    public TileSO tileShieldSO;
    public TileSO tileHeartSO;

    [Header("Animation")]
    public GameObject FxTileAnimation;

    [Header("Player Moves")]
    public string[] listOfMoves;

    public Sprite[] dirBtnColor;
}
