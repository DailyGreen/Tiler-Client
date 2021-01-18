using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Soldier_0 : Unit
{
    void Awake()
    {
        _name = "���� 1";
        _desc = "�������� " + (3 - createCount) + "�� ����";
        _cost = 0;
        _code = (int)UNIT.FORSET_SOILDER;
        _damage = 5;

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (3 - createCount) + "�� ����";

        if (createCount > 2)        // 2�� �Ŀ� ������
        {
            _desc = "������ �׿��ָ�!";

            _anim.SetTrigger("isSpawn");

            _activity.Add(ACTIVITY.MOVE);
            _activity.Add(ACTIVITY.ATTACK);

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    public void walking()
    {
        _anim.SetTrigger("isRunning");
    }

    public void dying()
    {
        _anim.SetTrigger("isDying");
    }

    void OnDestroy()
    {
        if (!(createCount > 2))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
