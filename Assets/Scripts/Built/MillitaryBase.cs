using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillitaryBase : Built
{
    public static int cost = 10;   // 건설 비용

    void Awake()
    {
        _name = "군사 기지";
        _max_hp = 10;
        _hp = _max_hp;
        _code = (int)BUILT.MILLITARY_BASE;
        maxCreateCount = 3;
        _basedistance = 1;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";
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
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";
        // 2턴 후에 생성됨
        if (createCount > maxCreateCount - 1)
        {
            _desc = "병력들을 생성한다";

            _anim.SetTrigger("isSpawn");

            GameMng.I.RemoveDelegate(this.waitingCreate);
            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();
            }
        }
    }

    public static void CreateAttackFirstUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.SOLDIER_0_UNIT_CREATE;
            GameMng.I._range.moveRange(1);
        }

    }

    public static void CreateAttackSecondUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.SOLDIER_1_UNIT_CREATE;
            GameMng.I._range.moveRange(1);
        }
    }

    void OnDestroy()
    {
        if (createCount < maxCreateCount - 1)
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
