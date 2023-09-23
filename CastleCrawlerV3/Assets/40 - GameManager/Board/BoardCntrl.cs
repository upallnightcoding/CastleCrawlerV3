using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject tilePreFab;
    [SerializeField] private Transform parent;
    [SerializeField] private TileMngr tileMngr;
    [SerializeField] private GameObject FXExplosion;

    public static int boardSize = 0;

    private TilePosition startPosition;
    private TilePosition finalPosition;
    private TilePosition currentPlayPos;

    private Dictionary<string, Move> moveDictionary = null;

    private Stack<string> moveStack;

    private bool completedMove = false;

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

    /**
     * StartNewGame() - 
     */
    public Stack<Move> StartNewGame()
    {
        moveStack = new Stack<string>();

        DrawBoard();

        Stack<Move> moves = CreateAPath();

        PlaceTileOnBoard(gameData.tileShieldSO, 20);
        //PlaceTileOnBoard(gameData.tileBombSO, 25);
        //PlaceTileOnBoard(gameData.tileHeartSO, 10);

        return (moves);
    }

    /**
     * DrawBoard() - Draw the initial board with all blanks tiles.
     */
    public void DrawBoard()
    {
        for (int col = 0; col < boardSize; col++)
        {
            for (int row = 0; row < boardSize; row++)
            {
                TilePosition position = new TilePosition(col, row);
                tileMngr.CreateTile(position, gameData.tileBlankSO);
            }
        }
    }

    /**
     * IsFinished() - Returns true if the current player position is
     * equal to the final position of the path.
     */
    public bool IsFinished()
    {
        return (currentPlayPos.IsEqual(finalPosition));
    }

    #region PlayerMove

    public bool OnPlayerMove(string moveName, Sprite color)
    {
        StartCoroutine(OnPlayerMoveCR(moveName, color));

        return(completedMove);
    }

    /**
     * OnPlayerMove() - 
     */
    public IEnumerator OnPlayerMoveCR(string moveName, Sprite color)
    {
        completedMove = true;
        StepValidType isStepValid = StepValidType.OPEN;
        List<TilePosition> tracking = new List<TilePosition>();
        TilePosition startingTile = new TilePosition(currentPlayPos);

        for (int step = 0; (step < moveName.Length) && completedMove; step++)
        {
            MoveToNextTile(moveName, step);

            isStepValid = tileMngr.IsStepValid(currentPlayPos);

            completedMove = CheckStepValid(isStepValid, color, startingTile, tracking);

            yield return new WaitForSeconds(0.75f);
        }

        if(completedMove)
        {
            moveStack.Push(moveName);
        }
    }

    private bool CheckStepValid(
        StepValidType isStepValid,
        Sprite color,
        TilePosition startingTile,
        List<TilePosition> tracking
    )
    {
        bool completedMove = false;

        switch (isStepValid)
        {
            case StepValidType.OPEN:
                completedMove = true;
                tracking.Add(new TilePosition(currentPlayPos));
                tileMngr.SetTileColor(currentPlayPos, color);
                GameManagerCntrl.Instance.AddGameLevel();
                break;
            case StepValidType.PASS_THROUGH:
                completedMove = true;
                tileMngr.AnimateBlocking(currentPlayPos);
                tileMngr.SetTileColor(currentPlayPos, color);
                tileMngr.SetTileToOpen(currentPlayPos);
                break;
            case StepValidType.OFF_BOARD:
            case StepValidType.BLOCKED:
                completedMove = false;
                ResetStartingPoint(startingTile, tracking);
                currentPlayPos = new TilePosition(startingTile);
                foreach (TilePosition resetPosition in tracking)
                {
                    tileMngr.Undo(resetPosition);
                }
                break;
        }

        return (completedMove);
    }

    private void MoveToNextTile(string moveName, int step)
    {
        switch (moveName.Substring(step, 1))
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
    }

    private TilePosition ResetStartingPoint(TilePosition startingTile, List<TilePosition> tracking)
    {
        TilePosition currentPlayPos;

        if (tracking.Count == 0)
        {
            currentPlayPos = new TilePosition(startingTile);
        }
        else
        {
            currentPlayPos = new TilePosition(tracking[tracking.Count - 1]);
        }

        return (currentPlayPos);
    }

    /*private IEnumerator LaydownTiles(List<TilePosition> tracking, Sprite color)
    {
        foreach (TilePosition position in tracking)
        {
            tileMngr.Set(position, color);
            yield return new WaitForSeconds(0.75f);
        }
    }*/

    /*private IEnumerator LaydownInValidTiles(List<TilePosition> tracking, Sprite color)
    {
        int count = tracking.Count;
        TilePosition position = null;

        for (int i = 0; i < count; i++)
        {
            position = tracking[i];
            tileMngr.Set(position, color);

            yield return new WaitForSeconds((i < count - 1) ? 0.75f : 0.1f);
        }

        if (position != null)
        {
            tileMngr.Animate(position);

            foreach (TilePosition resetPosition in tracking)
            {
                tileMngr.Undo(resetPosition);
            }

            tileMngr.ShowImage(position);
        }
    }*/

    #endregion

    public string UndoPlayerMove()
    {
        string moveName = null;

        if (moveStack.Count > 0)
        {
            moveName = moveStack.Pop();

            for (int character = moveName.Length - 1; character >= 0; character--)
            {
                tileMngr.Undo(currentPlayPos);

                switch (moveName.Substring(character, 1))
                {
                    case "N":
                        currentPlayPos.MoveToNextTile(GameData.NORTH_STEP, false);
                        break;
                    case "S":
                        currentPlayPos.MoveToNextTile(GameData.SOUTH_STEP, false);
                        break;
                    case "E":
                        currentPlayPos.MoveToNextTile(GameData.EAST_STEP, false);
                        break;
                    case "W":
                        currentPlayPos.MoveToNextTile(GameData.WEST_STEP, false);
                        break;
                }
            }
        }

        return (moveName);
    }

    /**
     * PlaceTileOnBoard
     */
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

    /**
     * CreateAPath() - 
     */
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
        //TilePosition startPosition = new TilePosition(1, 1);

        tileMngr.CreateTile(startPosition, gameData.tileCrownSO);

        return (startPosition);
    }

    //private Vector3 GetTilePos(int x, int z) => new Vector3(x, 0.0f, z);
}
