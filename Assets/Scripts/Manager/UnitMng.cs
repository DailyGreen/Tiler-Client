using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMng : MonoBehaviour
{
    public Mine mine = null;

    public Farm farm = null;

    public ACTIVITY act = 0;

    public Worker worker = null;

    RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        GameMng.I.UnitClickMove(worker.gameObject, worker);

        if (Input.GetMouseButtonDown(1))
        {
            switch (act)
            {
                case ACTIVITY.BUILD_MINE:
                    Building(mine, Mine.cost);
                    break;
                case ACTIVITY.BUILD_FARM:
                    Building(farm, Farm.cost);
                    break;
            }
        }
    }
    public void Building(Built built, int cost)
    {
        hit = GameMng.I.MouseLaycast();
        if (hit.collider.tag.Equals("Tile") && GameMng.I.GetTileCs._builtObj == null)
        {
            if (GameMng.I._gold > cost)
            {
                Instantiate(built, GameMng.I.GetTileCs.transform);
                GameMng.I.GetTileCs._builtObj = built;
                GameMng.I.minGold(cost);
            }
        }
    }
}
