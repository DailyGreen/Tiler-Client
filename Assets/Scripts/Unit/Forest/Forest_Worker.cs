using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Worker : Worker
{
    void Awake()
    {
        _name = "�� ���� �ϲ�";
        _unitDesc = "������ ���δ�.";
        _code = (int)UNIT.FOREST_WORKER;
        _max_hp = 10;
        _damage = 1;
        _hp = _max_hp;
        _basedistance = 1;
        _attackdistance = 1;
        maxCreateCount = 2;
        maintenanceCost = 1;
        _damage = 0;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void Start()
    {
        _name = string.Format("�� ���� �ϲ�  (������ : {0})", GameMng.I.getUserName(_uniqueNumber));
        _emoteSide.color = GetUserColor(_uniqueNumber);
    }

    void OnDestroy()
    {
        if (!(createCount > maxCreateCount - 1))
            GameMng.I.RemoveDelegate(waitingCreate);
        else
            GameMng.I.RemoveDelegate(maintenance);
    }
}
