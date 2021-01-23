using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Built
{
    public int attack;   // ���ݷ�
    public static int cost = 5;   // �Ǽ� ���

    void Awake()
    {
        _name = "�ͷ�";
        _max_hp = 7;
        _hp = _max_hp;
        _code = (int)BUILT.ATTACK_BUILDING;
        attack = 2;
        _attackdistance = 2;
        maxCreateCount = 3;
        //SaveX = GameMng.I.selectedTile.PosX;
        //SaveY = GameMng.I.selectedTile.PosZ;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";


        // 2�� �Ŀ� ������
        if (createCount > maxCreateCount - 1)
        {

            _desc = "���� ���� �� �����Ÿ� ���� ���� �����Ѵ�";

            GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._bActAccess = true;
            _anim.SetTrigger("isSpawn");
            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();
            }
            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    public void Attack()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.attackRange(_attackdistance);
        }
    }

    void OnDestroy()
    {
        if (createCount < maxCreateCount - 1)
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
