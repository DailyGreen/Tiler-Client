using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{

    //void Start()
    //{
    //    _name = "�ϲ�";
    //    _desc = "���ض� �뿹��";
    //    _cost = 0;
    //    _code = (int)UNIT.WORKER;
    //    _activity.Add(ACTIVITY.MOVE);
    //    _activity.Add(ACTIVITY.BUILD_MINE);
    //    _activity.Add(ACTIVITY.BUILD_FARM);
    //    _activity.Add(ACTIVITY.BUILD_ATTACK_BUILDING);
    //    _activity.Add(ACTIVITY.BUILD_CREATE_UNIT_BUILDING);
    //    _activity.Add(ACTIVITY.BUILD_SHIELD_BUILDING);
    //    _activity.Add(ACTIVITY.BUILD_UPGRADE_BUILDING);
    //}

    public static void Move()
    {
        GameMng.I._range.moveRange();
        Debug.Log("ĳ���� �̵�");
    }

    public static void buildMine()
    {
        GameMng.I._range.moveRange();
    }

    public static void buildFarm()
    {
        GameMng.I._range.moveRange();
    }

    public static void buildAttackBuilding()
    {
        GameMng.I._range.moveRange();
    }

    public static void buildCreateUnitBuilding()
    {
        GameMng.I._range.moveRange();
        Debug.Log("���� �ǹ� ����");
    }

    public static void buildShieldBuilding()
    {
        GameMng.I._range.moveRange();
        Debug.Log("��� �ǹ� ����");
    }

    public static void buildUpgradeBuilding()
    {
        GameMng.I._range.moveRange();
        Debug.Log("��ȭ �ǹ� ����");
    }

    public static void unitAttacking()
    {
        GameMng.I._range.attackRange();
        Debug.Log("���� �غ� �Ϸ�");
    }
}