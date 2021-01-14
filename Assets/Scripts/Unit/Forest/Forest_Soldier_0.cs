using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Soldier_0 : Unit
{
    void Awake()
    {
        _name = "전사 1";
        _desc = "모조리 죽여주마!";
        _cost = 0;
        _code = (int)UNIT.FORSET_SOILDER;
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
        _anim.SetBool("isWorking", false);
        _anim.SetBool("isRunning", true);
    }
    public void working()
    {
        _anim.SetBool("isRunning", false);
        _anim.SetBool("isWorking", true);
    }
    public void dying()
    {
        _anim.SetBool("isDying", true);
    }
}
