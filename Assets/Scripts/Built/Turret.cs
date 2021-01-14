using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Built
{
    public int attack;   // 공격력
    public static int cost = 5;   // 건설 비용

    void Start()
    {
        init();
        //GameMng.I.AddDelegate();
    }

    void init()
    {
        _name = "터렛";
        _desc = "턴이 끝날 때 사정거리 안의 적을 공격한다";
        _hp = 7;
        _code = (int)BUILT.ATTACK_BUILDING;
        attack = 2;
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void Attack()
    {
        Debug.Log("터렛을 클릭하였습니다.");
        if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.attackRange();
        }
    }

    //void OnDestroy()
    //{
    //    GameMng.I.RemoveDelegate();
    //}
}
