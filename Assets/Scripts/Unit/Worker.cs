using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    void Start()
    {
        _name = "�ϲ�";
        _desc = "���ض� �뿹��";
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
        Debug.Log("���� ����");
    }

    public static void buildFarm()
    {
        Debug.Log("���� ����");
    }

    public static void buildAttackBuilding()
    {
        Debug.Log("�ͷ� ����");
    }

    public static void buildCreateUnitBuilding()
    {
        Debug.Log("���� �ǹ� ����");
    }

    public static void buildShieldBuilding()
    {
        Debug.Log("��� �ǹ� ����");
    }

    public static void buildUpgradeBuilding()
    {
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