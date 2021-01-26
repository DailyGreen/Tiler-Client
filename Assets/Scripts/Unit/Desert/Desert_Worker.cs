using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desert_Worker : Worker
{
    public static int cost = 4;

    void Awake()
    {
        _name = "�縷 ���� �ϲ�";
        _code = (int)UNIT.DESERT_WORKER;
        _max_hp = 10;
        _hp = _max_hp;
        _basedistance = 1;
        maxCreateCount = 2;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";
        _unitDesc = "������ ���δ�.";

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void OnDestroy()
    {
        if (!(createCount > maxCreateCount - 1))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
