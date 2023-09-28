using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiCntrl : MonoBehaviour
{
    [SerializeField] private Transform dirBtnContainer;
    [SerializeField] private GameObject dirBtnPreFab;
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private GameData gameData;
    [SerializeField] private TMP_Text heartsCntTxt;
    [SerializeField] private Image star1On;
    [SerializeField] private Image star2On;
    [SerializeField] private Image star3On;
    [SerializeField] private GameObject nextLevelBanner;
    [SerializeField] private GameObject winBanner;
    [SerializeField] private GameObject looseBanner;
    [SerializeField] private GameObject illegalMoveBanner;
    [SerializeField] private TMP_Text moveCountDownTxt;

    // Dictionary of button controllers by name
    private Dictionary<string, DirBtnCntrl> dirBtnDict;

    // List of direction buttons
    private List<GameObject> listOfDirBtns = null;

    private int starCounter = 0;

    // Number if hearts at th beginning of the game
    private int health = 3;

    private UiValue moveCountDown = null;

    private void Start()
    {
        listOfDirBtns = new List<GameObject>();
    }

    /**
     * DisplayIllegalMoveBanner() - Displays the illegal move Banner
     */
    public void DisplayIllegalMoveBanner() =>
        StartCoroutine(DisplayBanner(illegalMoveBanner));

    public void DisplayWinBanner() =>
        StartCoroutine(DisplayBanner(winBanner));

    /**
     * UpdateHeartCount() - 
     */
    public void UpdateHeartCount(int count)
    {
        health += count;

        if (health > 0)
        {
            heartsCntTxt.text = health.ToString();
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

        moveCountDown.update(-1);
    }

    /**
     * OnBadPlayerMove() - If a player makes a bad move the UI still
     * needs to be updated to reflect the move.
     */
    public void OnBadPlayerMove()
    {
        moveCountDown.update(-1);
    }

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
                levelTxt.text = (++gameData.level).ToString();
                starCounter = 0;
                break;
        }
    }

    /**
     * TotalPointsIsZero() - 
     */
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

    /**
     * StartNewGame() - 
     */
    public void StartNewGame(Stack<Move> moves)
    {
        Dictionary<string, int> moveCntDict = new Dictionary<string, int>();
        dirBtnDict = new Dictionary<string, DirBtnCntrl>();
        levelTxt.text = gameData.level.ToString();

        listOfDirBtns.ForEach((element) => Destroy(element));

        moveCountDown = new UiValue(moveCountDownTxt, 2 * gameData.level);

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

    /**
     * UndoPlayerMove() - Undo a player's move by updating the count of
     * the direction button and counting down to zero the number of turns
     * taken.
     */
    public void UndoPlayerMove(string moveName)
    {
        dirBtnDict[moveName].UndoPlayerMove();

        moveCountDown.update(-1);
    }

    public bool IsDirBtnEnabled(string moveName)
    {
        return (dirBtnDict[moveName].IsDirBtnEnabled());
    }
}

public class UiValue
{
    private int value;
    private TMP_Text textValue;

    public UiValue(TMP_Text text, int initialValue)
    {
        textValue = text;
        value = initialValue;
        textValue.text = initialValue.ToString();
    }

    public void update(int update)
    {
        value += update;
        textValue.text = value.ToString();
    }
}
