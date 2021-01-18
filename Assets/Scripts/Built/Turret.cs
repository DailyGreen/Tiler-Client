using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Built
{
    public int attack;   // ���ݷ�
    public static int cost = 5;   // �Ǽ� ���

    void Start()
    {
        _name = "�ͷ�";
        _desc = "�������� " + (3 - createCount) + "�� ����";
        _hp = 7;
        _code = (int)BUILT.ATTACK_BUILDING;
        attack = 2;

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (3 - createCount) + "�� ����";
        // 2�� �Ŀ� ������
        if (createCount > 2)
        {
            _desc = "���� ���� �� �����Ÿ� ���� ���� �����Ѵ�";

            //_anim.SetTrigger("isSpawn");

            init();

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    public void Attack()
    {
        Debug.Log("�ͷ��� Ŭ���Ͽ����ϴ�.");
        if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.attackRange();
        }
    }

    void OnDestroy()
    {
        if (createCount < 2)
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
