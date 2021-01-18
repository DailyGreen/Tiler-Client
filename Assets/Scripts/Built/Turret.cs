using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Built
{
    public int attack;   // 공격력
    public static int cost = 5;   // 건설 비용

    void Start()
    {
        _name = "터렛";
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";
        _hp = 7;
        _code = (int)BUILT.ATTACK_BUILDING;
        attack = 2;

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";
        // 2턴 후에 생성됨
        if (createCount > 2)
        {
            _desc = "턴이 끝날 때 사정거리 안의 적을 공격한다";

            //_anim.SetTrigger("isSpawn");

            init();

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    public void Attack()
    {
        Debug.Log("터렛을 클릭하였습니다.");
        if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.attackRange();
        }
    }

    void OnDestroy()
    {
        if (createCount < 2)
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
