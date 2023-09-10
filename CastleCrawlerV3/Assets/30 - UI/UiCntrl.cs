using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiCntrl : MonoBehaviour
{
    [SerializeField] private Transform dirBtnContainer;
    [SerializeField] private GameObject dirBtnPreFab;
    [SerializeField] private TMP_Text level;
    [SerializeField] private GameData gameData;
    [SerializeField] private Image star1On;
    [SerializeField] private Image star2On;
    [SerializeField] private Image star3On;
    [SerializeField] private TMP_Text heartsCnt;

    private Dictionary<string, DirBtnCntrl> dirBtnDict;

    private List<GameObject> listOfDirBtns = null;

    private int starCounter = 0;

    private int health = 3;

    private void Start()
    {
        listOfDirBtns = new List<GameObject>();
        level.text = gameData.level.ToString();
    }

    /**
     * OnPlayerMove() - When the player selects a move update the move
     * counter that is assoicated with the button.
     */
    public void OnPlayerMove(string moveName)
    {
        dirBtnDict[moveName].UpdateMoveCounter();
    }

    public void UpdateHealth(int hearts)
    {
        health += hearts;

        heartsCnt.text = "x" + health.ToString();
    }

    /**
     * UpdateGameLevel() - 
     */
    public void UpdateGameLevel()
    {
        switch(++starCounter)
        {
            case 1:
                star1On.gameObject.SetActive(true);
                break;
            case 2:
                star2On.gameObject.SetActive(true);
                break;
            case 3:
                star3On.gameObject.SetActive(true);
                break;
            case 4:
                star1On.gameObject.SetActive(false);
                star2On.gameObject.SetActive(false);
                star3On.gameObject.SetActive(false);
                level.text = (++gameData.level).ToString();
                starCounter = 0;
                break;
        }
    }

    public bool TotalPointsIsZero()
    {
        int total = 0;

        foreach (string move in dirBtnDict.Keys)
        {
            DirBtnCntrl dirBtnCntrl = dirBtnDict[move];

            total += dirBtnCntrl.GetCount();
        }

        return (total == 0);
    }

    public void StartNewGame(Stack<Move> moves)
    {
        Dictionary<string, int> moveCntDict = new Dictionary<string, int>();
        dirBtnDict = new Dictionary<string, DirBtnCntrl>();

        listOfDirBtns.ForEach((element) => Destroy(element));

        foreach (Move move in moves)
        {
            if (moveCntDict.TryGetValue(move.moveName, out int count))
            {
                moveCntDict[move.moveName] = ++count;
            }
            else
            {
                moveCntDict[move.moveName] = 1;
            }
        }

        int colorIndex = 0;

        foreach (string moveName in moveCntDict.Keys)
        {
            int count = moveCntDict[moveName];

            GameObject button = Object.Instantiate(dirBtnPreFab, dirBtnContainer);
            DirBtnCntrl dirBtnCntrl = button.GetComponent<DirBtnCntrl>();
            dirBtnCntrl.Initialize(moveName, colorIndex++, count);

            dirBtnDict[moveName] = dirBtnCntrl;

            listOfDirBtns.Add(button);
        }
    }

    public void UndoPlayerMove(string moveName)
    {
        dirBtnDict[moveName].UndoPlayerMove();
    }

    public bool IsDirBtnEnabled(string moveName)
    {
        return (dirBtnDict[moveName].IsDirBtnEnabled());
    }
}
