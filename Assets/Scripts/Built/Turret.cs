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
            GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._bActAccess = true;

            _desc = "���� ���� �� �����Ÿ� ���� ���� �����Ѵ�";

            _anim.SetTrigger("isSpawn");

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();

                //GameMng.I.AddDelegate(this.Attack);
            }

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    /**
    * @brief ��Ÿ� ���� �ִ� ���� ����
    */
    /*public void Attack()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING)
        {
            tile = gameObject.transform.parent.GetComponent<Tile>();
            GameMng.I._range.attackRange(_attackdistance);
            GameMng.I._hextile.FindDistancesTo(tile);
            for (int i = 0; i < GameMng.I._hextile.cells.Length; i++)
            {
                if (GameMng.I._hextile.cells[i].Distance <= _attackdistance && GameMng.I._hextile.cells[i]._unitObj != null)
                {
                    if (!GameMng.I._hextile.cells[i]._unitObj._uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
                    {
                        GameMng.I._hextile.cells[i]._unitObj._hp -= attack;
                        _anim.SetTrigger("isAttacking");
                        Debug.Log(string.Format("ATTACK:{0}:{1}:{2}:{3}:{4}",
            tile.PosX,
            tile.PosZ,
            GameMng.I._hextile.cells[i].PosX,
            GameMng.I._hextile.cells[i].PosZ,
            attack));
                        NetworkMng.getInstance.SendMsg(string.Format("ATTACK:{0}:{1}:{2}:{3}:{4}",
            tile.PosX,
            tile.PosZ,
            GameMng.I._hextile.cells[i].PosX,
            GameMng.I._hextile.cells[i].PosZ,
            attack));
                    }
                }
            }
        }
    }*/

    void OnDestroy()
    {
        if (createCount < maxCreateCount - 1)
            GameMng.I.RemoveDelegate(waitingCreate);
        //else
        //GameMng.I.RemoveDelegate(Attack);
    }
}
