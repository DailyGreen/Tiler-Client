using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Built
{
    public int attack;              // ���ݷ�

    public Tile tilestate;          // �ͷ��� �ö� �ִ� Ÿ�� ����

    void Awake()
    {
        _name = "�ͷ�";
        _code = (int)BUILT.ATTACK_BUILDING;
        _attackdistance = 2;
        maxCreateCount = 3;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void Start()
    {
        _name = string.Format("{0} ���� �ͷ�  (������ : {1})", GameMng.I.getUserTribe(_uniqueNumber), GameMng.I.getUserName(_uniqueNumber));
        _emoteSide.color = GetUserColor(_uniqueNumber);

        switch((int)NetworkMng.getInstance.myTribe)
        {
            case 0:     // �� ����
                _max_hp = 7;
                _hp = _max_hp;
                attack = 5;
                break;
            case 1:     // �� ����
                _max_hp = 7;
                _hp = _max_hp;
                attack = 5;
                break;
            case 2:     // �縷 ����
                _max_hp = 7;
                _hp = _max_hp;
                attack = 5;
                break;
        }
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
        GameMng.I.AddDelegate(this.Attack);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";


        // 2�� �Ŀ� ������
        if (createCount > maxCreateCount - 1)
        {
            _desc = "���� ���� �� �����Ÿ� ���� ���� �����Ѵ�";

            _anim.SetTrigger("isSpawn");

            GameMng.I.RemoveDelegate(this.waitingCreate);

            GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._bActAccess = true;
            GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._anim.SetBool("isWorking", false);

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();
            }
        }
    }

    /**
    * @brief ��Ÿ� ���� �ִ� ���� ����
    */
    public void Attack()
    {
        int around = 0;
        if (GameMng.I.myTurn)
        {
            tilestate = gameObject.transform.parent.GetComponent<Tile>();
            GameMng.I._hextile.FindDistancesTo(tilestate);
            DynamicObject obj = null;
            for (int i = 0; i < GameMng.I._hextile.cells.Length; i++)
            {
                if (GameMng.I._hextile.cells[i].Distance <= _attackdistance && GameMng.I._hextile.cells[i]._unitObj != null)
                {
                    if (!GameMng.I._hextile.cells[i]._unitObj._uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
                    {
                        Debug.Log("around unit: " + around);
                        around++;
                        NetworkMng.getInstance.SendMsg(string.Format("ATTACK:{0}:{1}:{2}:{3}:{4}",
                        tilestate.PosX,
                        tilestate.PosZ,
                        GameMng.I._hextile.cells[i].PosX,
                        GameMng.I._hextile.cells[i].PosZ, attack));
                        if (GameMng.I._hextile.cells[i]._unitObj != null) { obj = GameMng.I._hextile.cells[i]._unitObj; }
                        _anim.SetTrigger("isAttacking");

                        obj._hp -= attack;
                        if (obj._hp <= 0)
                        {
                            // �ı�
                            obj.DestroyMyself();
                            GameMng.I._hextile.GetCell(GameMng.I._hextile.cells[i].PosX, GameMng.I._hextile.cells[i].PosZ)._unitObj = null;
                            GameMng.I._hextile.GetCell(GameMng.I._hextile.cells[i].PosX, GameMng.I._hextile.cells[i].PosZ)._builtObj = null;
                            GameMng.I._hextile.TilecodeClear(GameMng.I._hextile.cells[i]);        // TODO : �ڵ� �� ���� ������
                        }
                        obj = null;
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        if (createCount < maxCreateCount - 1)
            GameMng.I.RemoveDelegate(waitingCreate);
        else
            GameMng.I.RemoveDelegate(Attack);
    }
}
