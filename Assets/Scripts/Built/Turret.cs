using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Built
{
    public static int attack;              // ���ݷ�

    public Tile tilestate;          // �ͷ��� �ö� �ִ� Ÿ�� ����

    public static int maintenanceCost = 0;   // ���� ���

    public int _TurnCount = 0;
    void Awake()
    {
        _name = "�ͷ�";
        _code = (int)BUILT.ATTACK_BUILDING;
        _attackdistance = 2;
        maxCreateCount = 3;
        attack = 5;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void Start()
    {
        _name = string.Format("{0} ���� �ͷ�  (������ : {1})", GameMng.I.getUserTribe(_uniqueNumber), GameMng.I.getUserName(_uniqueNumber));
        _emoteSide.color = GetUserColor(_uniqueNumber);

        switch ((int)NetworkMng.getInstance.myTribe)
        {
            case 0:     // �� ����
                _max_hp = 7;
                _hp = _max_hp;
                attack = 5;
                maintenanceCost = 2;
                break;
            case 1:     // �� ����
                _max_hp = 7;
                _hp = _max_hp;
                attack = 5;
                maintenanceCost = 3;
                break;
            case 2:     // �縷 ����
                _max_hp = 7;
                _hp = _max_hp;
                attack = 5;
                maintenanceCost = 4;
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
                _TurnCount = 0;
                GameMng.I.AddDelegate(maintenance);
            }
        }
    }

    public void maintenance()
    {
        GameMng.I.minGold(maintenanceCost);
    }


    /**
     * @brief ��Ÿ� ���� �ִ� ���� ����
     */
    public void Attack()
    {
        maintenance();

        _TurnCount++;

        tilestate = gameObject.transform.parent.GetComponent<Tile>();
        GameMng.I._hextile.FindDistancesTo(tilestate);

        if ((_TurnCount % 3).Equals(0))
        {
            _anim.SetTrigger("isAttacking");

            NetworkMng.getInstance.SendMsg(string.Format("ATTACK_TURRET:{0}:{1}:{2}:{3}:{4}",
            tilestate.PosX,
            tilestate.PosZ,
            attack,
            this._uniqueNumber,
            _TurnCount));
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
