using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Witch_0 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "���� 0";
        _unitDesc = "������ �ٷ� �� �ȴ�";
        _max_hp = 20;
        _hp = _max_hp;
        _code = (int)UNIT.FOREST_WITCH_0;
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
        _name = string.Format("�� ���� ���� 0  (������ : {0})", GameMng.I.getUserName(_uniqueNumber));
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
