using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Soldier_0 : Unit
{
    public static int cost = 4;

    void Awake()
    {
        _name = "�� ���� ���� 0";
        _max_hp = 15;
        _hp = _max_hp;
        _code = (int)UNIT.SEA_SOLDIER_0;
        _damage = 5;
        maxCreateCount = 3;
        _basedistance = 1;
        _attackdistance = 1;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";
    }

    public void init()
    {
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.ATTACK);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";
        // 2�� �Ŀ� ������
        if (createCount > maxCreateCount - 1)
        {
            _anim.SetTrigger("isSpawn");

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
                init();

            _desc = "������ �׿��ָ�!";

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    void OnDestroy()
    {
        if (!(createCount > maxCreateCount - 1))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
