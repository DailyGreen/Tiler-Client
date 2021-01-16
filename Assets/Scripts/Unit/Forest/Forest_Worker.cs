using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Worker : Worker
{
    void Awake()
    {
        _name = "�ϲ�";
        _desc = "������ ���δ�.";
        _cost = 0;
        _code = (int)UNIT.FOREST_WORKER;
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.BUILD_MINE);
        _activity.Add(ACTIVITY.BUILD_FARM);
        _activity.Add(ACTIVITY.BUILD_ATTACK_BUILDING);
        _activity.Add(ACTIVITY.BUILD_MILLITARY_BASE);
        _activity.Add(ACTIVITY.BUILD_SHIELD_BUILDING);
        _activity.Add(ACTIVITY.BUILD_UPGRADE_BUILDING);
        //_activity.Add(ACTIVITY.ATTACK);             // �ӽ��Դϴ�!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
    public void working()
    {
        _anim.SetBool("isWorking", true);
    }
}
