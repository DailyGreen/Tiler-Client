using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Built
{
    public int attack;              // 공격력

    public Tile tilestate;          // 터렛이 올라가 있는 타일 정보

    void Awake()
    {
        _name = "터렛";
        _code = (int)BUILT.ATTACK_BUILDING;
        _attackdistance = 2;
        maxCreateCount = 3;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void Start()
    {
        _name = string.Format("{0} 종족 터렛  (소유자 : {1})", GameMng.I.getUserTribe(_uniqueNumber), GameMng.I.getUserName(_uniqueNumber));
        _emoteSide.color = GetUserColor(_uniqueNumber);

        switch((int)NetworkMng.getInstance.myTribe)
        {
            case 0:     // 숲 종족
                _max_hp = 7;
                _hp = _max_hp;
                attack = 5;
                break;
            case 1:     // 물 종족
                _max_hp = 7;
                _hp = _max_hp;
                attack = 5;
                break;
            case 2:     // 사막 종족
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
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";


        // 2턴 후에 생성됨
        if (createCount > maxCreateCount - 1)
        {
            _desc = "턴이 끝날 때 사정거리 안의 적을 공격한다";

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
    * @brief 사거리 내에 있는 적을 공격
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
                            // 파괴
                            obj.DestroyMyself();
                            GameMng.I._hextile.GetCell(GameMng.I._hextile.cells[i].PosX, GameMng.I._hextile.cells[i].PosZ)._unitObj = null;
                            GameMng.I._hextile.GetCell(GameMng.I._hextile.cells[i].PosX, GameMng.I._hextile.cells[i].PosZ)._builtObj = null;
                            GameMng.I._hextile.TilecodeClear(GameMng.I._hextile.cells[i]);        // TODO : 코드 값 원래 값으로
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
