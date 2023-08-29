using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCntrl : MonoBehaviour
{
    [SerializeField] private BoardCntrl boardCntrl;

    public void StartNewGame()
    {
        boardCntrl.StartNewGame();
    }
}
