using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desert_Witch_1 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "법사 1";
        _unitDesc = "사막에선 무슨 마법 써?";
        _max_hp = 20;
        _hp = _max_hp;
        _code = (int)UNIT.DESERT_WITCH_1;
        _damage = 10;
        _basedistance = 1;
        _attackdistance = 2;
        maxCreateCount = 3;
        maintenanceCost = 1;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";
        _emoteSide.color = GetUserColor(_uniqueNumber);

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
