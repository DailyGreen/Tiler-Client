using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : DynamicObject
{
    /**
     * ���� ���� ����
     * �ݵ�� ��ӹ޴� �ڽ��� Start ���� ���� ������ �ֽñ� �ٶ��ϴ�.
     */
    public int _damage = 0;

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

    public static void buildMillitaryBaseBuilding()
    {
        GameMng.I._range.moveRange();
        Debug.Log("���� ���� ����");
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