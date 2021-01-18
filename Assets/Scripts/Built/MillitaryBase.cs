using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillitaryBase : Built
{
    public static int cost = 10;   // �Ǽ� ���

    void Start()
    {
        _name = "���� ����";
        _desc = "�������� " + (3 - createCount) + "�� ����";
        _hp = 10;
        _code = (int)BUILT.MILLITARY_BASE;

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.ATTACK_UNIT_CREATE);
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }
    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (3 - createCount) + "�� ����";
        // 2�� �Ŀ� ������
        if (createCount > 2)
        {
            _desc = "���µ��� �����Ѵ�";

            //_anim.SetTrigger("isSpawn");

            init();

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
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
    void OnDestroy()
    {
        if (createCount < 2)
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
