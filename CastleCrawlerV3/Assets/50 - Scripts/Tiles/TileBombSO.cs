using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileBombSO", menuName = "CasteCrawler/Tile/Bomb")]
public class TileBombSO : TileSO
{
    public override IEnumerator BlockedTile(TilePosition position)
    {
        if (animation != null)
        {
            GameObject go = Object.Instantiate(
                animation,
                position.GetTilePos() + new Vector3(0.0f, 0.02f, 0.0f),
                Quaternion.identity
            );
        }

        GameManagerCntrl.Instance.UpdateHeartCount(-1);

        yield return null;
    }
}
