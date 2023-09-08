using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCntrl : MonoBehaviour
{
    [SerializeField] private Transform dirBtnContainer;
    [SerializeField] private GameObject dirBtnPreFab;

    private Dictionary<string, DirBtnCntrl> dirBtnDict;

    private List<GameObject> listOfDirBtns = null;

    private void Start()
    {
        listOfDirBtns = new List<GameObject>();
    }

    public void OnPlayerMove(string moveName)
    {
        dirBtnDict[moveName].OnDirectionClick();
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
