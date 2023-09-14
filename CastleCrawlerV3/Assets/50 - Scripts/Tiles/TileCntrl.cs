using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private TMP_Text prompt;
    [SerializeField] private Image image;
    [SerializeField] private Image background;

    private Stack<Sprite> undoBGStack = new Stack<Sprite>();
    private Stack<string> undoTextStack = new Stack<string>();

    private TileBase tile;

    private bool isShowingImage = false;

    private string resetText;

    public TileBase Tile 
    {
        get
        {
            return(tile);
        }

        set 
        {
            tile = value;

            if (gameData.debugSw)
            {
                prompt.text = tile.GetPrompt();
            }

            isShowingImage = tile.IsShowing();

            background.gameObject.SetActive(true);
            background.sprite = tile.GetBackGround();

            if (tile.GetForeGroundImage() != null)
            {
                if (isShowingImage)
                {
                    image.gameObject.SetActive(true);
                    image.sprite = tile.GetForeGroundImage();
                }
            } 
            else
            {
                image.gameObject.SetActive(false);
            }

            resetText = prompt.text;
        } 
    
    }

    public void ResetTile()
    {
        prompt.text = resetText;
    }

    public void ShowImage()
    {
        if (tile.GetForeGroundImage() != null)
        {
            image.gameObject.SetActive(true);
            image.sprite = tile.GetForeGroundImage();
        }
    }

    /**
     * Set() - 
     */
    public void Set(Sprite newBackGroundSprite)
    {
        undoBGStack.Push(this.background.sprite);
        undoTextStack.Push(prompt.text);

        background.sprite = newBackGroundSprite;
    }

    public void Animate(TilePosition position)
    {
        GameObject go = tile.Animate(position);
        //Destroy(go, 2.0f);
    }

    /**
     * Undo() - 
     */
    public void Undo()
    {
        if (undoBGStack.Count > 0)
        {
            this.background.sprite = undoBGStack.Pop();
            this.prompt.text = undoTextStack.Pop();
        }
    }
}
