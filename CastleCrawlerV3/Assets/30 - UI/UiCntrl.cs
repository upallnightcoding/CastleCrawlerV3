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
    [SerializeField] private TMP_Text heartsCnt;
    [SerializeField] private Image star1On;
    [SerializeField] private Image star2On;
    [SerializeField] private Image star3On;
    [SerializeField] private GameObject nextLevelBanner;
    [SerializeField] private GameObject winBanner;
    [SerializeField] private GameObject looseBanner;
    [SerializeField] private GameObject illegalMoveBanner;
    [SerializeField] private TMP_Text bombShowText;

    // Dictionary of button controllers by name
    private Dictionary<string, DirBtnCntrl> dirBtnDict;

    // List of direction buttons
    private List<GameObject> listOfDirBtns = null;

    private int starCounter = 0;

    // Number if hearts at th beginning of the game
    private int health = 3;

    private bool bombShowSw = false;

    private void Start()
    {
        listOfDirBtns = new List<GameObject>();
        level.text = gameData.level.ToString();
    }

    /**
     * DisplayIllegalMoveBanner() - Displays the illegal move Banner
     */
    public void DisplayIllegalMoveBanner() =>
        StartCoroutine(DisplayBanner(illegalMoveBanner));

    public void DisplayWinBanner() =>
        StartCoroutine(DisplayBanner(winBanner));

    /**
     * BombShowSw() - 
     */
    public void BombShowSw()
    {
        bombShowSw = !bombShowSw;

        bombShowText.text = (bombShowSw) ? "Hide Bombs" : "Show Bombs";
    }

    /**
     * UpdateHeartCount() - 
     */
    public void UpdateHeartCount(int count)
    {
        health += count;

        if (health > 0)
        {
            heartsCnt.text = health.ToString();
        }
    }

    IEnumerator DisplayBanner(GameObject banner)
    {
        banner.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        banner.gameObject.SetActive(false);
    }

    /**
     * OnPlayerMove() - When the player selects a move update the move
     * counter that is assoicated with the button.
     */
    public void OnPlayerMove(string moveName)
    {
        dirBtnDict[moveName].UpdateMoveCounter();
    }

    /*public void UpdateHealth(int hearts)
    {
        health += hearts;

        heartsCnt.text = "x" + health.ToString();
    }*/

    /**
     * AddGameLevel() - 
     */
    public void AddGameLevel()
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
