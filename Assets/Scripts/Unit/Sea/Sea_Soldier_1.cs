using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Soldier_1 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "물 전사 1";
        _unitDesc = "모조리";
        _max_hp = 15;
        _hp = _max_hp;
        _code = (int)UNIT.SEA_SOLDIER_1;
        _damage = 5;
        _basedistance = 1;
        _attackdistance = 1;
        maxCreateCount = 3;
        maintenanceCost = 1;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    public override void init()
    {
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.ATTACK);
    }

    void OnDestroy()
    {
        if (!(createCount > maxCreateCount - 1))
            GameMng.I.RemoveDelegate(waitingCreate);
        else
            GameMng.I.RemoveDelegate(maintenance);
    }
}
