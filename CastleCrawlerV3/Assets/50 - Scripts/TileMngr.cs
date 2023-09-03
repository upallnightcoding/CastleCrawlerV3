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

    public void Start()
    {
        //destroyOnInitialize = new List<GameObject>();
        //tileCntrls = new TileCntrl[gameData.boardSize, gameData.boardSize];
    }

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

    public void Set(TilePosition position, GameObject tile, TileSO tileSO)
    {
        Set(position.Col, position.Row, tile, tileSO);
    }

    private void Set(int col, int row, GameObject tile, TileSO tileSO)
    {
        tileCntrls[col, row] = tile.GetComponent<TileCntrl>(); 

        tileCntrls[col, row].Tile = tileSO;

        destroyOnInitialize.Add(tile);
    }

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

    //public void SetTileAsVisted(TilePosition position) =>
        //tileCntrls[position.Col, position.Row].SetTileAsVisted();
}
