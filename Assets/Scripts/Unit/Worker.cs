using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    RaycastHit2D hit;
    void Start()
    {
        _name = "일꾼";
        _desc = "일해라 노예야";
        _cost = 0;
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.BUILD_MINE);
        _activity.Add(ACTIVITY.BUILD_FARM);
        _activity.Add(ACTIVITY.BUILD_ATTACK_BUILDING);
        _activity.Add(ACTIVITY.BUILD_CREATE_UNIT_BUILDING);
        _activity.Add(ACTIVITY.BUILD_SHIELD_BUILDING);
        _activity.Add(ACTIVITY.BUILD_UPGRADE_BUILDING);
    }

    public static void Move()
    {
        RangeScrp RangSc = GameObject.Find("RangeParent").GetComponent<RangeScrp>();
        RangSc.MoveRange();
        Debug.Log("캐릭터 이동");
    }

    public static void buildMine()
    {
        RangeScrp RangSc = GameObject.Find("RangeParent").GetComponent<RangeScrp>();
        RangSc.MoveRange();
        
    }

    public static void buildFarm()
    {
        RangeScrp RangSc = GameObject.Find("RangeParent").GetComponent<RangeScrp>();
        RangSc.MoveRange();
    }

    public static void buildAttackBuilding()
    {
        RangeScrp RangSc = GameObject.Find("RangeParent").GetComponent<RangeScrp>();
        RangSc.MoveRange();
        Debug.Log("터렛 생성");
    }

    public static void buildCreateUnitBuilding()
    {
        RangeScrp RangSc = GameObject.Find("RangeParent").GetComponent<RangeScrp>();
        RangSc.MoveRange();
        Debug.Log("유닛 건물 생성");
    }

    public static void buildShieldBuilding()
    {
        RangeScrp RangSc = GameObject.Find("RangeParent").GetComponent<RangeScrp>();
        RangSc.MoveRange();
        Debug.Log("방어 건물 생성");
    }

    public static void buildUpgradeBuilding()
    {
        RangeScrp RangSc = GameObject.Find("RangeParent").GetComponent<RangeScrp>();
        RangSc.MoveRange();
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