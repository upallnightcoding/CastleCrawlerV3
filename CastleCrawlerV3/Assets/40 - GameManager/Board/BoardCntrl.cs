using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject tilePreFab;
    [SerializeField] private Transform parent;
    [SerializeField] private TileMngr tileMngr;
    [SerializeField] private TileSO TileBlankSO;
    [SerializeField] private TileSO TileCastleSO;
    [SerializeField] private TileSO TileCrownSO;

    public static int boardSize = 0;

    private TilePosition startPosition;
    private TilePosition finalPosition;
    private TilePosition currentPlayPos;

    private Dictionary<string, Move> moveDictionary = null;

    private bool SafeGuard(int count) => count < gameData.safeGuardLimit;
    private bool BuildingPath(int level) => level < gameData.level;

    public void Start()
    {
        Initialize();
        
        StartNewGame();
    }

    public void Initialize()
    {
        boardSize = gameData.boardSize;

        tileMngr.Initialize();

        moveDictionary = new Dictionary<string, Move>();

        foreach (string moveName in gameData.listOfMoves)
        {
            moveDictionary.Add(moveName, new Move(moveName));
        }
    }

    public void StartNewGame()
    {
        DrawBoard();
        CreateAPath();
    }

    public void DrawBoard()
    {
        for (int col = 0; col < boardSize; col++)
        {
            for (int row = 0; row < boardSize; row++)
            {
                tileMngr.CreateTile(new TilePosition(col, row), TileBlankSO);
            }
        }
    }

    private Stack<Move> CreateAPath()
    {
        int level = 0;
        int count = 0;

        startPosition = SelectStartingPoint();

        tileMngr.CrownTile(startPosition);

        TilePosition tile = new TilePosition(startPosition);
        Stack<Move> moves = new Stack<Move>();

        while (BuildingPath(level) && SafeGuard(count))
        {
            int[] moveIndex = ShuffleMoves();
            Move moveFound = null;

            for (int i = 0; (i < gameData.listOfMoves.Length) && (moveFound == null); i++)
            {
                if (moveDictionary.TryGetValue(gameData.listOfMoves[moveIndex[i]], out Move move))
                {
                    finalPosition = move.IsValid(tile, tileMngr);

                    if (finalPosition != null)
                    {
                        moveFound = move;
                        moves.Push(move);
                        tile = new TilePosition(finalPosition);
                        level++;
                        move.DebugIt();
                    }
                }
            }

            count++;
        }

        tileMngr.CastleTile(finalPosition);

        currentPlayPos = new TilePosition(startPosition);

        return (moves);
    }

    private int[] ShuffleMoves()
    {
        int[] moves = new int[gameData.listOfMoves.Length];

        for (int i = 0; i < gameData.listOfMoves.Length; i++)
        {
            moves[i] = i;
        }

        for (int i = 0; i < gameData.listOfMoves.Length / 2; i++)
        {
            int indexA = Random.Range(0, gameData.listOfMoves.Length);
            int indexB = Random.Range(0, gameData.listOfMoves.Length);

            int value = moves[indexA];
            moves[indexA] = moves[indexB];
            moves[indexB] = value;
        }

        return (moves);
    }

    private TilePosition SelectRandomPosition()
    {
        int col = Random.Range(0, boardSize);
        int row = Random.Range(0, boardSize);

        return (new TilePosition(col, row));
    }

    private TilePosition SelectStartingPoint()
    {
        TilePosition startPosition = SelectRandomPosition();

        tileMngr.CreateTile(startPosition, TileCrownSO);

        return (startPosition);
    }

    //private Vector3 GetTilePos(int x, int z) => new Vector3(x, 0.0f, z);
}
