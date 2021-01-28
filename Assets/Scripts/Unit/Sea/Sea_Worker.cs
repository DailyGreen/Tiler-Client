using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Worker : Worker
{
    public static int cost = 4;

    void Awake()
    {
        _name = "물 종족 일꾼";
        _unitDesc = "듬직해 보인다.";
        _code = (int)UNIT.SEA_WORKER;
        _max_hp = 10;
        _hp = _max_hp;
        maxCreateCount = 2;
        _basedistance = 1;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";
        _emoteSide.color = GetUserColor();
        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void OnDestroy()
    {
        if (!(createCount > maxCreateCount))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
