using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMngr : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject tilePreFab;
    [SerializeField] private Transform parent;

    private TileCntrl[,] tileCntrls = null;

    private List<GameObject> destroyOnInitialize;

    public void PathTile(TilePosition position) =>
       tileCntrls[position.Col, position.Row].Tile = gameData.tilePathSO;

    public void CrownTile(TilePosition position) =>
        tileCntrls[position.Col, position.Row].Tile = gameData.tileCrownSO;

    public void CastleTile(TilePosition position) =>
        tileCntrls[position.Col, position.Row].Tile = gameData.tileCastleSO;

    public void ResetTile(TilePosition position) =>
        tileCntrls[position.Col, position.Row].Tile = gameData.tileBlankSO;

    public bool IsTileOpen(TilePosition position) =>
        tileCntrls[position.Col, position.Row].Tile.IsTileOpen();

    public void Set(TilePosition position, GameObject tile, TileSO tileSO) =>
        Set(position.Col, position.Row, tile, tileSO);

    public void Set(TilePosition position, Sprite color) =>
        tileCntrls[position.Col, position.Row].Set(color);

    public void Undo(TilePosition position) =>
        tileCntrls[position.Col, position.Row].Undo();

    public bool IsSupportProp(TilePosition position) =>
        tileCntrls[position.Col, position.Row].Tile.IsSupportProp();

    /**
     * Initialize() - Initial all attributes for the start of any game. Delete
     * any tiles that exist to start over.
     */
    public void Initialize()
    {
        destroyOnInitialize = new List<GameObject>();
        tileCntrls = new TileCntrl[gameData.boardSize, gameData.boardSize];

        if ((destroyOnInitialize != null) && (destroyOnInitialize.Count > 0))
        {
            foreach (GameObject tile in destroyOnInitialize)
            {
                Destroy(tile);
            }
        }
    }

    public void CreateTile(TilePosition position, TileSO tileSO)
    {
        GameObject go = Instantiate(tilePreFab, position.GetTilePos(), Quaternion.identity, parent);
        Set(position, go, tileSO);
    }

    public bool IsMoveValid(TilePosition position)
    {
        bool offTheBoard = IsOffTheBoard(position);
        bool tileIsOpen = false;

        if (offTheBoard)
        {
            //GameManagerCntrl.Instance.DisplayIllegalMoveBanner();
        }
        else
        {
            tileIsOpen = tileCntrls[position.Col, position.Row].Tile.IsTileInPlay();
        }

        return (!offTheBoard && tileIsOpen);
    }

    private void Set(int col, int row, GameObject tile, TileSO tileSO)
    {
        if (tileCntrls[col, row] != null)
        {
            Destroy(tileCntrls[col, row].gameObject);
        }

        tileCntrls[col, row] = tile.GetComponent<TileCntrl>(); 

        tileCntrls[col, row].Tile = tileSO;

        destroyOnInitialize.Add(tile);
    }

    private bool IsOffTheBoard(TilePosition position)
    {
        bool colOutOfRange = (position.Col >= gameData.boardSize) || (position.Col < 0);
        bool rowOutOfRange = (position.Row >= gameData.boardSize) || (position.Row < 0);

        return (colOutOfRange || rowOutOfRange);
    }
}
