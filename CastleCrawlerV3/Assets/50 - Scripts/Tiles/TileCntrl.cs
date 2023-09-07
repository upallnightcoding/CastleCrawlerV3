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

    //private TileState ressetTileState;
    private Material resetMaterial;
    private string resetText;

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

    public void Set(Sprite background)
    {
        this.background.sprite = background;
    }

    /*public void SetTileAsVisted()
    {
        SetTile(TileState.VISITED, GameManagerCntrl.Instance.DisplayTileMaterial());
    }

    private void SetTile(TileState tileState, Material material)
    {
        SetTile(tileState, material, tileState.ToString());
    }

    private void SetTile(TileState tileState, Material material, string text)
    {
        ressetTileState = state;
        resetMaterial = GetComponent<Renderer>().material;
        resetText = tileLabel.text;

        state = tileState;
        GetComponent<Renderer>().material = material;

        if (gameData.debugSw)
        {
            tileLabel.text = text;
        }
    }*/
}
