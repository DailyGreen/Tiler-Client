using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Worker : Worker
{
    void Awake()
    {
        _name = "�� ���� �ϲ�";
        _desc = "�������� " + (3 - createCount) + "�� ����";
        _cost = 0;
        _code = (int)UNIT.FOREST_WORKER;

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (3 - createCount) + "�� ����";
        // 2�� �Ŀ� ������
        if (createCount > 2)
        {
            _anim.SetTrigger("isSpawn");

            init();

            _desc = "������ ���δ�.";

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    void OnDestroy()
    {
        if (!(createCount > 2))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
