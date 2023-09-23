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

    private StepValidType typeTileBlocking = StepValidType.BLOCKED;

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
            typeTileBlocking = tile.GetTileBlocking();

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
     * SetTileBlockedOpen() - Set the tile type to OPEN.  Usually done
     * after something has tripped the tile's animation.  This is done 
     * reguardless of the current tile type.
     */
    public void SetTileBlockedOpen()
    {
        typeTileBlocking = StepValidType.OPEN;
    }

    public StepValidType GetTileBlocking()
    {
        return (typeTileBlocking);
    }

    /**
     * Set() - Sets the background of a tile and sets the stacks
     * in order to undo the action.
     */
    public void Set(Sprite newBackGroundSprite)
    {
        undoBGStack.Push(this.background.sprite);
        undoTextStack.Push(prompt.text);

        background.sprite = newBackGroundSprite;
    }

    /**
     * Undo() - If the undo stacks are not empty, pop the value at
     * the top of the background and text stack.  Once the data is 
     * popped it can not be retrieved.
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
