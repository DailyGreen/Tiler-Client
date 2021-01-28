using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Worker : Worker
{
    public static int cost = 4;

    void Awake()
    {
        _name = "�� ���� �ϲ�";
        _unitDesc = "������ ���δ�.";
        _code = (int)UNIT.SEA_WORKER;
        _max_hp = 10;
        _hp = _max_hp;
        maxCreateCount = 2;
        _basedistance = 1;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";
        _emoteSide.color = GetUserColor();
        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void OnDestroy()
    {
        if (!(createCount > maxCreateCount))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
