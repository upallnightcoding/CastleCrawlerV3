using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileHeartSO", menuName = "CasteCrawler/Tile/Heart")]
public class TileHeartSO : TileSO
{
    public override void PassThrough(
        TileMngr tileMngr, 
        TilePosition position, 
        Sprite color
    )
    {
        if (animation != null)
        {
            GameObject go = Object.Instantiate(
                animation,
                position.GetTilePos() + new Vector3(0.0f, 0.02f, 0.0f),
                Quaternion.identity
            );
        }

        GameManagerCntrl.Instance.UpdateHeartCount(1);

        tileMngr.SetTileColor(position, color);

        tileMngr.SetTileToOpen(position);
    }
}
