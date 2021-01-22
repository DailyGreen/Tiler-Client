using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Soldier_0 : Unit
{
    public static int cost = 4;
    void Awake()
    {
        _name = "���� 0";
        _max_hp = 15;
        _hp = _max_hp;
        _code = (int)UNIT.FOREST_SOLDIER_0;
        _damage = 5;
        _basedistance = 1;
        _attackdistance = 1;
        maxCreateCount = 3;
        SaveX = GameMng.I.selectedTile.PosX;
        SaveY = GameMng.I.selectedTile.PosZ;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

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
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        if (createCount > maxCreateCount - 1)        // 2�� �Ŀ� ������
        {
            GameMng.I._hextile.GetCell(SaveX, SaveY)._builtObj.GetComponent<Built>()._bActAccess = false;

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
        if (!(createCount > maxCreateCount - 1))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
