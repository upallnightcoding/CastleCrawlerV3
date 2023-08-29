using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject tilePreFab;
    [SerializeField] private Transform parent;

    private int boardSize;

    private TilePosition startPosition;
    private TilePosition finalPosition;

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

        moveDictionary = new Dictionary<string, Move>();

        foreach (string moveName in gameData.listOfMoves)
        {
            moveDictionary.Add(moveName, new Move(moveName));
        }
    }

    public void StartNewGame()
    {
        DrawBoard();
    }

    public void DrawBoard()
    {
        for (int col = 0; col < boardSize; col++)
        {
            for (int row = 0; row < boardSize; row++)
            {
                Vector3 position = GetTilePos(col, row);
                GameObject tile = Instantiate(tilePreFab, position, Quaternion.identity, parent);
                tile.GetComponent<TileCntrl>().Tile = new TileCastle(gameData);
            }
        }
    }

    private Stack<Move> CreateAPath()
    {
        int level = 0;
        int count = 0;
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
                    //finalPosition = move.IsValid(tile, tileMngr);

                    if (finalPosition != null)
                    {
                        moveFound = move;
                        moves.Push(move);
                        tile = new TilePosition(finalPosition);
                        level++;
                    }
                }
            }

            count++;
        }

        //tileMngr.SetEndingTile(finalPosition);

        //currentPlayPos = new TilePosition(startPosition);

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

    private Vector3 GetTilePos(int x, int z) => new Vector3(x, 0.0f, z);
}
