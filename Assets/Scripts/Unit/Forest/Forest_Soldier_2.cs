using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Soldier_2 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "전사 2";
        _max_hp = 20;
        _hp = _max_hp;
        _code = (int)UNIT.FOREST_SOLDIER_2;
        _damage = 10;
        _basedistance = 1;
        _attackdistance = 2;
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
            GameMng.I._hextile.GetCell(SaveX, SaveY)._builtObj._anim.SetTrigger("isComplete");

            _desc = "강해보인다... 아마두";

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
