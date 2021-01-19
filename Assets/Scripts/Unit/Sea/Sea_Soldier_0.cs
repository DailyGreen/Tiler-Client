using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Soldier_0 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "�� ���� ���� 1";
        _desc = "������ �׿��ָ�!";
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
