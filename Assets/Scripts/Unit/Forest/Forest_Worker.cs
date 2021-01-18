using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Worker : Worker
{
    void Awake()
    {
        _name = "숲 종족 일꾼";
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";
        _cost = 0;
        _code = (int)UNIT.FOREST_WORKER;

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";
        // 2턴 후에 생성됨
        if (createCount > 2)
        {
            _anim.SetTrigger("isSpawn");

            init();

            _desc = "듬직해 보인다.";

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    void OnDestroy()
    {
        if (!(createCount > 2))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
