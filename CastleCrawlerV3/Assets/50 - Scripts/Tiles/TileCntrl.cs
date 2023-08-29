using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileCntrl : MonoBehaviour
{
    [SerializeField] private TMP_Text prompt;
    [SerializeField] private Image icon;
    [SerializeField] private MeshRenderer background;

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

            prompt.text = tile.GetPrompt();
            icon.sprite = tile.GetIcon();
            background.material = tile.GetBackGround();
        } 
    
    }
}
