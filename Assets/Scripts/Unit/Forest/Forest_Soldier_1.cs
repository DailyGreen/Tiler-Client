using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Soldier_1 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "전사 1";
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";
        _code = (int)UNIT.FOREST_SOLDIER_1;
        _damage = 10;
        _hp = 10;

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";

        if (createCount > 2)        // 2턴 후에 생성됨
        {
            _desc = "뼈와 살을 분리시켜주지!";

            _anim.SetTrigger("isSpawn");

            _activity.Add(ACTIVITY.MOVE);
            _activity.Add(ACTIVITY.ATTACK);

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    public void walking()
    {
        _anim.SetTrigger("isRunning");
    }

    public void dying()
    {
        _anim.SetTrigger("isDying");
    }

    void OnDestroy()
    {
        if (!(createCount > 2))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
