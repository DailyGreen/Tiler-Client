using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Soldier_0 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "���� 0";
        _desc = "�������� " + (3 - createCount) + "�� ����";
        _hp = 15;
        _code = (int)UNIT.FOREST_SOLDIER_0;
        _damage = 5;

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.ATTACK);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (3 - createCount) + "�� ����";

        if (createCount > 2)        // 2�� �Ŀ� ������
        {
            _desc = "������ �׿��ָ�!";

            _anim.SetTrigger("isSpawn");

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
                init();

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
