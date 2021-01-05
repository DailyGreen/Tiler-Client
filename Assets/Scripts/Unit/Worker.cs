using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    void Start()
    {
        _name = "일꾼";
        _desc = "일해라 노예야";
        _cost = 0;
        _activity.Add(ACTIVITY.BUILD_MINE);
        _activity.Add(ACTIVITY.BUILD_FARM);
        _activity.Add(ACTIVITY.BUILD_ATTACK_BUILDING);
        _activity.Add(ACTIVITY.BUILD_CREATE_UNIT_BUILDING);
        _activity.Add(ACTIVITY.BUILD_SHIELD_BUILDING);
        _activity.Add(ACTIVITY.BUILD_UPGRADE_BUILDING);
    }

    void Update()
    {
        GameMng.I.UnitClickMove(this.gameObject, this);
    }

    public static void buildMine()
    {
        Debug.Log("광산 생성");
    }

    public static void buildFarm()
    {
        Debug.Log("농장 생성");
    }

    public static void buildAttackBuilding()
    {
        Debug.Log("터렛 생성");
    }

    public static void buildCreateUnitBuilding()
    {
        Debug.Log("유닛 건물 생성");
    }

    public static void buildShieldBuilding()
    {
        Debug.Log("방어 건물 생성");
    }

    public static void buildUpgradeBuilding()
    {
        Debug.Log("강화 건물 생성");
    }
}

/* 

광산 짓기
골드 창고 짓기
농장 짓기
식량 창고 짓기
공격 건물 짓기 
유닛 생성 건물 짓기
방어 건물 짓기
강화 건물 짓기
건물 수리


*/