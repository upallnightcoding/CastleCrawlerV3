using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCntrl : MonoBehaviour
{
    [SerializeField] private BoardCntrl boardCntrl;
    [SerializeField] UiCntrl uiCntrl;

    public static GameManagerCntrl Instance = null;

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

    public void DisplayIllegalMoveBanner()
    {
        uiCntrl.DisplayIllegalMoveBanner();
    }

    public void OnStartNewGame()
    {
        Stack<Move> moves = boardCntrl.StartNewGame();

        if (moves != null)
        {
            uiCntrl.StartNewGame(moves);
        }
    }

    public void OnPlayerMove(string move, Sprite color)
    {
        if (uiCntrl.IsDirBtnEnabled(move))
        {
            if (boardCntrl.OnPlayerMove(move, color))
            {
                uiCntrl.OnPlayerMove(move);

                if (boardCntrl.IsFinished() && uiCntrl.TotalPointsIsZero())
                {
                    //uiCntrl.DisplayWinBanner();
                }
            }
        }
    }

    public void OnUndoPlayerMove()
    {
        string moveName = boardCntrl.UndoPlayerMove();

        if (moveName != null)
        {
            uiCntrl.UndoPlayerMove(moveName);
        }
    }
}
