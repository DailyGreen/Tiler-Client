using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMng : MonoBehaviour
{
    public ACTIVITY act = 0;

    public Worker worker = null;

    public GameObject[] builtObj = null;
    // Update is called once per frame
    void Update()
    {
        GameMng.I.UnitClickMove(worker.gameObject, worker);

        if (Input.GetMouseButtonDown(1))
        {
            switch (act)
            {
                case ACTIVITY.BUILD_MINE:
                    Building(Mine.cost, (int)BUILT.MINE);
                    break;
                case ACTIVITY.BUILD_FARM:
                    Building(Farm.cost, (int)BUILT.FARM);
                    break;
                case ACTIVITY.BUILD_ATTACK_BUILDING:
                    Building(Turret.cost, (int)BUILT.ATTACK_BUILDING);
                    break;
            }
        }
    }
    public void Building(int cost, int index)
    {
        GameMng.I.MouseLaycast();
        if (GameMng.I.GetTileCs._builtObj == null)
        {
            if (GameMng.I._gold > cost)
            {
                GameObject Child = Instantiate(builtObj[index - 200], GameMng.I.GetTileCs.transform) as GameObject;
                GameMng.I.GetTileCs._builtObj = Child.GetComponent<Built>();
                GameMng.I.GetTileCs._code = index;
                GameMng.I.minGold(cost);
                GameMng.I.RangeSc.RangeTileReset();
            }
        }
    }
}