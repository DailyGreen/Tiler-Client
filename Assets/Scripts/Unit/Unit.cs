using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : DynamicObject
{
    /**
     * 유닛 고유 스탯
     * 반드시 상속받는 자식의 Start 에서 값을 변경해 주시길 바랍니다.
     */
    public int _damage = 0;

    public static void Move()
    {
        GameMng.I._range.moveRange();
        Debug.Log("캐릭터 이동");
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
        Debug.Log("군사 기지 생성");
    }

    public static void buildShieldBuilding()
    {
        GameMng.I._range.moveRange();
        Debug.Log("방어 건물 생성");
    }

    public static void buildUpgradeBuilding()
    {
        GameMng.I._range.moveRange();
        Debug.Log("강화 건물 생성");
    }

    public static void unitAttacking()
    {
        GameMng.I._range.attackRange();
        Debug.Log("공격 준비 완료");
    }
}