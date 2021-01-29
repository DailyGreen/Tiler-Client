using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desert_Witch_1 : Unit
{
    void Awake()
    {
        _name = "���� 1";
        _unitDesc = "�縷���� ���� ���� ��?";
        _max_hp = 20;
        _hp = _max_hp;
        _code = (int)UNIT.DESERT_WITCH_1;
        _damage = 10;
        _basedistance = 1;
        _attackdistance = 2;
        maxCreateCount = 3;
        maintenanceCost = 1;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void Start()
    {
        _name = string.Format("�縷 ���� ���� 1  (������ : {0})", GameMng.I.getUserName(_uniqueNumber));
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
