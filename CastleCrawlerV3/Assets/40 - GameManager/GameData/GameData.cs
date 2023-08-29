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
    public int boardSize;
    public int safeGuardLimit;
    public int level;

    [Header("Tile Background Colors")]
    public Material TileColorGray;
    public Material TileColorWhite;

    [Header("Game Sprites")]
    public Sprite crownSprite;
    public Sprite castleSprite;

    [Header("Player Moves")]
    public string[] listOfMoves;
}