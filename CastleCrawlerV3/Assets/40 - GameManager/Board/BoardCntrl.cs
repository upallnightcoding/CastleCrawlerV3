using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject tilePreFab;
    [SerializeField] private Transform parent;
    [SerializeField] private TileMngr tileMngr;
    //[SerializeField] private TileSO TileBlankSO;
    //[SerializeField] private TileSO TileCastleSO;
    //[SerializeField] private TileSO TileCrownSO;

    public static int boardSize = 0;

    private TilePosition startPosition;
    private TilePosition finalPosition;
    private TilePosition currentPlayPos;

    private Dictionary<string, Move> moveDictionary = null;

    private Stack<string> moveStack;

    private bool SafeGuard(int count) => count < gameData.safeGuardLimit;
    private bool BuildingPath(int level) => level < gameData.level;

    public void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        boardSize = gameData.boardSize;

        tileMngr.Initialize();

        moveDictionary = new Dictionary<string, Move>();

        foreach (string moveName in gameData.listOfMoves)
        {
            moveDictionary.Add(moveName, new Move(moveName));
        }
    }

    public Stack<Move> StartNewGame()
    {
        moveStack = new Stack<string>();

        DrawBoard();

        Stack<Move> moves = CreateAPath();

        PlaceTileOnBoard(gameData.tileShieldSO, 10);
        PlaceTileOnBoard(gameData.tileBombSO, 10);
        PlaceTileOnBoard(gameData.tileHeartSO, 10);

        return (moves);
        //return (null);
    }

    public void DrawBoard()
    {
        for (int col = 0; col < boardSize; col++)
        {
            for (int row = 0; row < boardSize; row++)
            {
                tileMngr.CreateTile(new TilePosition(col, row), gameData.tileBlankSO);
            }
        }
    }

    public bool IsFinished()
    {
        return (currentPlayPos.IsEqual(finalPosition));
    }

    public bool OnPlayerMove(string moveName, Sprite color)
    {
        bool valid = true;
        Stack<TilePosition> tracking = new Stack<TilePosition>();
        TilePosition startingTile = new TilePosition(currentPlayPos);

        for (int move = 0; (move < moveName.Length) && valid; move++)
        {
            switch (moveName.Substring(move, 1))
            {
                case "N":
                    currentPlayPos.MoveToNextTile(GameData.NORTH_STEP);
                    break;
                case "S":
                    currentPlayPos.MoveToNextTile(GameData.SOUTH_STEP);
                    break;
                case "E":
                    currentPlayPos.MoveToNextTile(GameData.EAST_STEP);
                    break;
                case "W":
                    currentPlayPos.MoveToNextTile(GameData.WEST_STEP);
                    break;
            }

            valid = tileMngr.IsMoveValid(currentPlayPos);

            if (valid)
            {
                tracking.Push(new TilePosition(currentPlayPos));
            }
        }

        if (valid)
        {
            moveStack.Push(moveName);

            foreach (TilePosition position in tracking)
            {
                tileMngr.Set(position, color);
            }
        }
        else
        {
            currentPlayPos = new TilePosition(startingTile);
        }

        return (valid);
    }

    private void PlaceTileOnBoard(TileSO tile, int n)
    {
        for (int i = 0; i < n; i++)
        {
            TilePosition position = SelectRandomPosition();

            if (tileMngr.IsSupportProp(position))
            {
                tileMngr.CreateTile(position, tile);
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

        tileMngr.CreateTile(startPosition, gameData.tileCrownSO);

        return (startPosition);
    }

    //private Vector3 GetTilePos(int x, int z) => new Vector3(x, 0.0f, z);
}
