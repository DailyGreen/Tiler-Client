using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Witch_0 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "물 종족 법사 0";
        _unitDesc = "과연 물 마법의 힘은?";
        _max_hp = 15;
        _hp = _max_hp;
        _code = (int)UNIT.SEA_WITCH_0;
        _damage = 5;
        _basedistance = 1;
        _attackdistance = 1;
        maxCreateCount = 3;
        maintenanceCost = 1;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void Start()
    {
        _name = string.Format("바다 종족 법사 0  (소유자 : {0})", GameMng.I.getUserName(_uniqueNumber));
        _emoteSide.color = GetUserColor(_uniqueNumber);
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
