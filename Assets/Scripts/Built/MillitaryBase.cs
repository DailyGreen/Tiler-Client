using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillitaryBase : Built
{
    public static int cost = 10;   // 건설 비용

    void Start()
    {
        _name = "군사 기지";
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";
        _hp = 10;
        _code = (int)BUILT.MILLITARY_BASE;

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.SOLDIER_0_UNIT_CREATE);
        _activity.Add(ACTIVITY.SOLDIER_1_UNIT_CREATE);
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";
        // 2턴 후에 생성됨
        if (createCount > 2)
        {
            _desc = "병력들을 생성한다";

            //_anim.SetTrigger("isSpawn");

            init();

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    public static void CreateAttackFirstUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.SOLDIER_0_UNIT_CREATE;
            GameMng.I._range.moveRange();
        }

    }

    public static void CreateAttackSecondUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.SOLDIER_1_UNIT_CREATE;
            GameMng.I._range.moveRange();
        }
    }

    void OnDestroy()
    {
        if (createCount < 2)
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
