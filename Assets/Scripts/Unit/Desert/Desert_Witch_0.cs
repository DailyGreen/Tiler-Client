using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desert_Witch_0 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "법사 0";
        _unitDesc = "사막에도 마법이 있어?";
        _max_hp = 20;
        _hp = _max_hp;
        _code = (int)UNIT.DESERT_WITCH_0;
        _damage = 10;
        _basedistance = 1;
        _attackdistance = 2;
        maxCreateCount = 3;
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
    }
}
