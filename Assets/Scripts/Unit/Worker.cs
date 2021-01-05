using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    RaycastHit2D hit;
    void Start()
    {
        _name = "�ϲ�";
        _desc = "���ض� �뿹��";
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
        Debug.Log("ĳ���� �̵�");
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
        Debug.Log("�ͷ� ����");
    }

    public static void buildCreateUnitBuilding()
    {
        RangeScrp RangSc = GameObject.Find("RangeParent").GetComponent<RangeScrp>();
        RangSc.MoveRange();
        Debug.Log("���� �ǹ� ����");
    }

    public static void buildShieldBuilding()
    {
        RangeScrp RangSc = GameObject.Find("RangeParent").GetComponent<RangeScrp>();
        RangSc.MoveRange();
        Debug.Log("��� �ǹ� ����");
    }

    public static void buildUpgradeBuilding()
    {
        RangeScrp RangSc = GameObject.Find("RangeParent").GetComponent<RangeScrp>();
        RangSc.MoveRange();
        Debug.Log("��ȭ �ǹ� ����");
    }

    
}

/* 

���� ����
��� â�� ����
���� ����
�ķ� â�� ����
���� �ǹ� ���� 
���� ���� �ǹ� ����
��� �ǹ� ����
��ȭ �ǹ� ����
�ǹ� ����


*/