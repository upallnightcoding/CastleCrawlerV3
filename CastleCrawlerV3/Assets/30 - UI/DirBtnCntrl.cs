using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class DirBtnCntrl : MonoBehaviour
{
    [SerializeField] private TMP_Text moveTxt;
    [SerializeField] private TMP_Text counterTxt;
    [SerializeField] private Image btnImage;
    [SerializeField] private GameData gameData;

    private bool enabledBtn = true;
    private int count = 0;
    private int colorIndex = 0;

    public bool IsDirBtnEnabled() => enabledBtn;

    public int GetCount() => count;

    public void OnPlayerMove()
    {
        GameManagerCntrl.Instance.OnPlayerMove(moveTxt.text, gameData.dirBtnColor[colorIndex]);
    }

    public void OnDirectionClick()
    {
        if (enabledBtn)
        {
            counterTxt.text = (--count).ToString();

            if (count == 0)
            {
                enabledBtn = false;
            }
        }
    }

    public void UndoPlayerMove()
    {
        if (count == 0)
        {
            enabledBtn = true;
        }

        counterTxt.text = (++count).ToString();
    }

    public void Initialize(string direction, int colorIndex, int count)
    {
        // Set the button text
        moveTxt.text = direction;

        // Set the sprite of the button image
        btnImage.sprite = gameData.dirBtnColor[colorIndex];

        // Initialize the count of this direction
        counterTxt.text = count.ToString();

        this.count = count;
        this.colorIndex = colorIndex;
    }

}
