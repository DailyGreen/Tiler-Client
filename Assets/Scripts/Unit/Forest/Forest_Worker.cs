using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Worker : Worker
{
    public static int cost = 4;

    void Awake()
    {
        _name = "숲 종족 일꾼";
        _unitDesc = "듬직해 보인다.";
        _code = (int)UNIT.FOREST_WORKER;
        _max_hp = 10;
        _hp = _max_hp;
        _basedistance = 1;
        maxCreateCount = 2;
        maintenanceCost = 1;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";
        _emoteSide.color = GetUserColor(_uniqueNumber);

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void OnDestroy()
    {
        if (!(createCount > maxCreateCount - 1))
            GameMng.I.RemoveDelegate(waitingCreate);
        else
            GameMng.I.RemoveDelegate(maintenance);
    }
}
