using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillitaryBase : Built
{
    public static int cost = 10;   // �Ǽ� ���

    void Start()
    {
        _name = "���� ����";
        _desc = "���ֵ��� �����Ѵ�";
        _hp = 10;
        _code = (int)BUILT.MILLITARY_BASE;
    }

    void init()
    {
        _activity.Add(ACTIVITY.ATTACK_UNIT_CREATE);
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public static void CreateAttackUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.ATTACK_UNIT_CREATE;
            GameMng.I._range.moveRange();
        }
        Debug.Log("���� ���� ������ ��ġ ����");

    }

}
