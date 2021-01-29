using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Soldier_0 : Unit
{
    void Awake()
    {
        _name = "�� ���� ���� 0";
        _unitDesc = "������ �׿��ָ�!";
        _max_hp = 15;
        _hp = _max_hp;
        _code = (int)UNIT.SEA_SOLDIER_0;
        _damage = 5;
        maxCreateCount = 3;
        _basedistance = 1;
        _attackdistance = 1;
        maintenanceCost = 1;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void Start()
    {
        _name = string.Format("�ٴ� ���� ���� 0  (������ : {0})", GameMng.I.getUserName(_uniqueNumber));
        _emoteSide.color = GetUserColor(_uniqueNumber);
    }

    public override void init()
    {
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.ATTACK);
    }


    void OnDestroy()
    {
        if (!(createCount > maxCreateCount - 1))
            GameMng.I.RemoveDelegate(waitingCreate);
        else
            GameMng.I.RemoveDelegate(maintenance);
    }
}
