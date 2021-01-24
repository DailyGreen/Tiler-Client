using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desert_Soldier_0 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "전사 0";
        _max_hp = 15;
        _hp = _max_hp;
        _code = (int)UNIT.DESERT_SOLDIER_0;
        _damage = 5;
        _basedistance = 1;
        _attackdistance = 1;
        maxCreateCount = 3;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.ATTACK);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";

        if (createCount > maxCreateCount - 1)        // 2턴 후에 생성됨
        {
            GameMng.I._hextile.GetCell(SaveX, SaveY)._builtObj.GetComponent<Built>()._bActAccess = true;

            _desc = "모조리 죽여주마!";

            _anim.SetTrigger("isSpawn");

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
                init();

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    void OnDestroy()
    {
        if (!(createCount > maxCreateCount - 1))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
