using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Worker : Worker
{
    void Awake()
    {
        _name = "�� ���� �ϲ�";
        _desc = "�������� " + (2 - createCount) + "�� ����";
        _cost = 0;
        _hp = 10;
        _code = (int)UNIT.FOREST_WORKER;

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (2 - createCount) + "�� ����";
        // 2�� �Ŀ� ������
        if (createCount > 1)
        {
            _anim.SetTrigger("isSpawn");

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
                init();

            _desc = "������ ���δ�.";

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    void OnDestroy()
    {
        if (!(createCount > 1))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
