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
        _desc = "���� ���� �� �����Ÿ� ���� ���� �����Ѵ�";
        _hp = 7;
        _code = (int)BUILT.ATTACK_BUILDING;
        attack = 2;
    }

    void Update()
    {
        //Attack();
    }

    public void Attack()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //GameMng.I.mouseRaycast();
        if (GameMng.I.targetTile._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.attackRange();
        }
        //}
    }
}
