using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Worker : Worker
{
    void Awake()
    {
        _name = "일꾼";
        _desc = "듬직해 보인다.";
        _cost = 0;
        _code = (int)UNIT.WORKER;
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.BUILD_MINE);
        _activity.Add(ACTIVITY.BUILD_FARM);
        _activity.Add(ACTIVITY.BUILD_ATTACK_BUILDING);
        _activity.Add(ACTIVITY.BUILD_CREATE_UNIT_BUILDING);
        _activity.Add(ACTIVITY.BUILD_SHIELD_BUILDING);
        //_activity.Add(ACTIVITY.BUILD_UPGRADE_BUILDING);
        _activity.Add(ACTIVITY.ATTACK);             // 임시입니다!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        StartCoroutine("creating");

    }

    IEnumerator creating()
    {
        yield return new WaitForSeconds(1);
        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            _anim.SetBool("isWorking", false);
            _anim.SetBool("isRunning", false);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("sdf");
            walking();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("sdf");
            working();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _anim.SetBool("isDying", true);
        }
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
}
