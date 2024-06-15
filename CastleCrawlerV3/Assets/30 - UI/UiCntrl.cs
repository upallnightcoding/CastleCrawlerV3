using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] private GameObject blockedMoveBanner;
    [SerializeField] private TMP_Text moveCountDownTxt;
    [SerializeField] private Slider levelSlider;
    [SerializeField] private GameObject anotherGameDialog;
    [SerializeField] private Image howTo;

    public static event System.Action OnYouLooseEvent = delegate { };

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

    // Display the Illegal Move Banner
    public void DisplayIllegalMoveBanner() =>
        StartCoroutine(DisplayBanner(illegalMoveBanner));

    // Display the Looser Banner
    public void DisplayLooserBanner() =>
        StartCoroutine(DisplayBanner(looseBanner));

    // Display the Winner Banner
    public void DisplayWinBanner() =>
        StartCoroutine(DisplayBanner(winBanner));

    // Display the Blocked Move Banner
    public void DisplayBlockedMoveBanner() =>
        StartCoroutine(DisplayBanner(blockedMoveBanner));

    // Display the Next Level Banner
    public void DisplayNextLevelBanner() =>
        StartCoroutine(DisplayBanner(nextLevelBanner));

    /**
     * PlayAnotherGameYes() - 
     */
    public void PlayAnotherGameYes()
    {
        health = 3;
        UpdateHeartCount(0);
        levelSlider.value = 4;
        SetGameLevel();
        anotherGameDialog.SetActive(false);
        GameManagerCntrl.Instance.PlayAnotherGameYes();
    }

    /**
     * HowToOn() - 
     */
    public void HowToOn()
    {
        howTo.gameObject.SetActive(true);
    }

    /**
     * HowToOff() -
     */
    public void HowToOff()
    {
        howTo.gameObject.SetActive(false);
    }

    /**
     * PlayAnotherGameNo() - 
     */
    public void PlayAnotherGameNo()
    {
        GameManagerCntrl.Instance.PlayAnotherGameNo();
    }

    /**
     * UpdateHeartCount() - Update the heart count.  If the heart count 
     * reaches zero, display the "Play Again" dialog.
     */
    public void UpdateHeartCount(int count)
    {
        health += count;

        heartsCntTxt.text = health.ToString();

        if (health <= 0)
        {
            anotherGameDialog.SetActive(true);
        }
    }

    /**
     * SetGameLevel() - 
     */
    public void SetGameLevel()
    {
        levelTxt.text = levelSlider.value.ToString();

        gameData.level = (int) levelSlider.value;

        starCounter = 0;
    }

    /**
     * DisplayBanner() - 
     */
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

        CountDownMoveCounter();
    }

    /**
     * CountDownMoveCounter() - Counts down the number of moves.  If the
     * number of moves reaches 0, the "You Loose" banner is displayed
     * and no additional moves can be made.
     */
    private void CountDownMoveCounter()
    {
        if (moveCountDown.CheckUpdateDone(-1))
        {
            InvokeYouLoose();
        }
    }

    public bool HasMoreMoves()
    {
        return (moveCountDown.HasMoreMoves());
    }

    /**
     * OnBadPlayerMove() - If a player makes a bad move the UI still
     * needs to be updated to reflect the move.
     */
    public void OnBadPlayerMove()
    {
        CountDownMoveCounter();
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
                DisplayWinBanner();
                break;
            case 2:
                star2On.gameObject.SetActive(true);
                DisplayWinBanner();
                break;
            case 3:
                star3On.gameObject.SetActive(true);
                DisplayWinBanner();
                break;
            case 4:
                star1On.gameObject.SetActive(false);
                star2On.gameObject.SetActive(false);
                star3On.gameObject.SetActive(false);
                gameData.level = (++gameData.level) > gameData.maxLevel ? gameData.maxLevel : gameData.level;
                levelTxt.text = gameData.level.ToString();
                starCounter = 0;
                DisplayNextLevelBanner();
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
        int index = 0;

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

        string[] moveList = new string[moveCntDict.Count];
        index = 0;

        foreach (string moveName in moveCntDict.Keys)
        {
            moveList[index++] = moveName;
        }

        for (int i = 0; i < moveCntDict.Count; i++)
        {
            int indexA = Random.Range(0, moveList.Length);
            int indexB = Random.Range(0, moveList.Length);

            string value = moveList[indexA];
            moveList[indexA] = moveList[indexB];
            moveList[indexB] = value;
        }

        index = 0;

        foreach (string moveName in moveList)
        {
            int count = moveCntDict[moveName];

            GameObject button = Object.Instantiate(dirBtnPreFab, dirBtnContainer);
            DirBtnCntrl dirBtnCntrl = button.GetComponent<DirBtnCntrl>();
            dirBtnCntrl.Initialize(moveName, index++, count);

            dirBtnDict[moveName] = dirBtnCntrl;

            listOfDirBtns.Add(button);
        }
    }

    public void InvokeYouLoose()
    {
        OnYouLooseEvent.Invoke();
    }

    /**
     * UndoPlayerMove() - Undo a player's move by updating the count of
     * the direction button and counting down to zero the number of turns
     * taken.
     */
    public void UndoPlayerMove(string moveName)
    {
        dirBtnDict[moveName].UndoPlayerMove();

        CountDownMoveCounter();
    }

    /**
     * IsDirBtnEnabled() - 
     */
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

    public bool HasMoreMoves()
    {
        return (value > 0);
    }

    public bool CheckUpdateDone(int update)
    {
        bool done = false;

        if (value == 0)
        {
            done = true;
        } else
        {
            value += update;
            textValue.text = value.ToString();
        }

        return (done);
    }
}
