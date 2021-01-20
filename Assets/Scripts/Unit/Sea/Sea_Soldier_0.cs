using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Soldier_0 : Unit
{
    void Awake()
    {
        _name = "물 종족 전사 1";
        _desc = "모조리 죽여주마!";
        _cost = 0;
        _code = (int)UNIT.SEA_SOLDIER_0;
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.ATTACK);
        StartCoroutine("creating");
    }

    IEnumerator creating()
    {
        yield return new WaitForSeconds(1);
        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
    }

    public void walking()
    {
        _anim.SetTrigger("isRunning");
    }

    public void dying()
    {
        _anim.SetTrigger("isDying");
    }
}
