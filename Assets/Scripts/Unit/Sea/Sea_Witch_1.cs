using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Witch_1 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "�� ���� ���� 2";
        _unitDesc = "���� �� ������ ����?";
        _max_hp = 15;
        _hp = _max_hp;
        _code = (int)UNIT.SEA_WITCH_1;
        _damage = 5;
        _basedistance = 2;
        _attackdistance = 2;
        maxCreateCount = 3;
        maintenanceCost = 1;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void Start()
    {
        _name = string.Format("�ٴ� ���� ���� 1  (������ : {0})", GameMng.I.getUserName(_uniqueNumber));
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
    }
}
