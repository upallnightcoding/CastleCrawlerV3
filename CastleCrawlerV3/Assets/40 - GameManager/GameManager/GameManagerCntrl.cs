using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCntrl : MonoBehaviour
{
    [SerializeField] private BoardCntrl boardCntrl;
    [SerializeField] private UiCntrl uiCntrl;
    [SerializeField] private TileSO tileBombSO;

    public static GameManagerCntrl Instance = null;

    public void DisplayIllegalMoveBanner() => uiCntrl.DisplayIllegalMoveBanner();
    public void DisplayBlockedBanner() => uiCntrl.DisplayBlockedMoveBanner();

    public void DisplayBanner(StepValidType type)
    {
        switch(type)
        {
            case StepValidType.OFF_BOARD:
                DisplayIllegalMoveBanner();
                break;
            case StepValidType.BLOCKED:
                DisplayBlockedBanner();
                break;
        }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHeartCount(int count)
    {
        uiCntrl.UpdateHeartCount(count);
    }

    /**
     * IncreaseGameLevel() - Displays the next level banner and 
     * increments to the next playing level.
     */
    public void UpdateGameLevel()
    {
        uiCntrl.AddGameLevel();

        OnStartNewGame();
    }

    /**
     * OnStartNewGame() - 
     */
    public void OnStartNewGame()
    {
        Stack<Move> moves = boardCntrl.StartNewGame();

        while (moves == null)
        {
            moves = boardCntrl.StartNewGame();
        }

        uiCntrl.StartNewGame(moves);
    }

    /**
     * OnPlayerMove() - When a player makes a move, make sure that
     * the move IS enabled.  Make sure that the play move is illegal.
     * If the move creates end-of-game, display the Winner
     * Banner.
     */
    public void OnPlayerMove(string move, Sprite color)
    {
        if (uiCntrl.IsDirBtnEnabled(move))
        {
            if (boardCntrl.OnPlayerMove(move, color))
            {
                uiCntrl.OnPlayerMove(move);

                if (boardCntrl.IsFinished() && uiCntrl.TotalPointsIsZero())
                {
                    UpdateGameLevel();
                }
            } else
            {
                uiCntrl.OnBadPlayerMove();
            }
        }
    }

    /**
     * OnUndoPlayerMove() - 
     */
    public void OnUndoPlayerMove()
    {
        string moveName = boardCntrl.UndoPlayerMove();

        if (moveName != null)
        {
            uiCntrl.UndoPlayerMove(moveName);
        }
    }

    public void OnYouLoose()
    {
        uiCntrl.DisplayLooserBanner();
    }

    public void OnEnable()
    {
        UiCntrl.OnYouLooseEvent += OnYouLoose;
    }

    public void OnDisable()
    {
        UiCntrl.OnYouLooseEvent -= OnYouLoose;
    }
}
