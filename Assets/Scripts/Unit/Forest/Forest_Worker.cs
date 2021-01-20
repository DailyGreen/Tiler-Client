using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Worker : Worker
{
    public static int cost = 4;
    void Awake()
    {
        _name = "숲 종족 일꾼";
        _desc = "생성까지 " + (2 - createCount) + "턴 남음";
        _code = (int)UNIT.FOREST_WORKER;
        _hp = 10;

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (2 - createCount) + "턴 남음";
        // 2턴 후에 생성됨
        if (createCount > 1)
        {
            _anim.SetTrigger("isSpawn");

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
                init();

            _desc = "듬직해 보인다.";

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    void OnDestroy()
    {
        if (!(createCount > 1))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
