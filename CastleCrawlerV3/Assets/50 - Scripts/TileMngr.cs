using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMngr : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    private TileCntrl[,] tileCntrls = null;

    private List<GameObject> tileObjects;

    public void Start()
    {
        tileObjects = new List<GameObject>();
        tileCntrls = new TileCntrl[gameData.boardSize, gameData.boardSize];
    }

    public void Initialize()
    {
        if ((tileObjects != null) && (tileObjects.Count > 0))
        {
            foreach (GameObject tile in tileObjects)
            {
                Destroy(tile);
            }
        }
    }

    public bool IsTileOpen(TilePosition position) =>
        tileCntrls[position.col, position.row].Tile.IsTileOpen();
}
