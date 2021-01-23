using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    public void init()
    {
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.BUILD_MINE);
        _activity.Add(ACTIVITY.BUILD_FARM);
        _activity.Add(ACTIVITY.BUILD_ATTACK_BUILDING);
        _activity.Add(ACTIVITY.BUILD_MILLITARY_BASE);
        _activity.Add(ACTIVITY.BUILD_SHIELD_BUILDING);
        _activity.Add(ACTIVITY.BUILD_UPGRADE_BUILDING);
        //_activity.Add(ACTIVITY.ATTACK);             // 임시입니다!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //StartCoroutine("creating");
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