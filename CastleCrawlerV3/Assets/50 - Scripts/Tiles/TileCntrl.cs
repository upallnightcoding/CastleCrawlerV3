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

    private Stack<Sprite> undoStack = new Stack<Sprite>();

    private TileBase tile;

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
                background.gameObject.SetActive(false);
            }
            else
            {
                background.gameObject.SetActive(true);
                background.sprite = tile.GetBackGround();
            }

            if (tile.GetSprite() != null)
            {
                image.gameObject.SetActive(true);
                image.sprite = tile.GetSprite();
            } 
            else
            {
                image.gameObject.SetActive(false);
            }
        } 
    
    }

    /**
     * Set() - 
     */
    public void Set(Sprite background)
    {
        undoStack.Push(this.background.sprite);
        this.background.sprite = background;
    }

    /**
     * Undo() - 
     */
    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            this.background.sprite = undoStack.Pop();
        }
    }
}
