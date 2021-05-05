using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : DynamicObject
{
    /**
     * 유닛 고유 스탯
     * 반드시 상속받는 자식의 Start 에서 값을 변경해 주시길 바랍니다.
     */
    public int _damage = 0;

    public int maintenanceCost = 0;

    public int PosX, PosZ;

    public string _unitDesc;            // 유닛 생성 시 나오는 설명

    public bool _actdebuff = false;

    /**
     * @brief 유닛 생성 대기 및 생성 함수
     */
    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";

        if (createCount > maxCreateCount - 1)
        {

            if (this._code != (int)UNIT.FOREST_WORKER && this._code != (int)UNIT.SEA_WORKER && this._code != (int)UNIT.DESERT_WORKER)
            {
                GameMng.I._hextile.GetCell(SaveX, SaveY)._builtObj.GetComponent<Built>()._bActAccess = true;

                GameMng.I._hextile.GetCell(SaveX, SaveY)._builtObj.GetComponent<MillitaryBase>().CreatingUnitobj = null;
            }
            GameMng.I._hextile.GetCell(SaveX, SaveY)._builtObj._anim.SetTrigger("isComplete");
            _desc = _unitDesc;

            _anim.SetTrigger("isSpawn");

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();
                GameMng.I.AddDelegate(maintenance);
            }

            GameMng.I.RemoveDelegate(waitingCreate);
        }
    }

    public void maintenance()
    {
        GameMng.I.minFood(maintenanceCost);

        if (GameMng.I.countHungry > (NetworkMng.getInstance.v_user.Count * 9))
        {
            int percent = Random.Range(1, 100);
            if (percent > 90)
            {
                DestroyMyself();
                GameMng.I._hextile.GetCell(PosX, PosZ)._unitObj = null;
                GameMng.I._hextile.TilecodeClear(PosX, PosZ);
                NetworkMng.getInstance.SendMsg(string.Format("DIE_UNIT:{0}:{1}", PosX, PosZ));
            }
        }
        else if (GameMng.I.countHungry > (NetworkMng.getInstance.v_user.Count * 6) && !_actdebuff)
        {
            // 랜덤 행동불능 (확률 %)
            int percent = Random.Range(1, 100);
            if (percent > 80)
            {
                _bActAccess = false;
                _actdebuff = true;
            }
        }
        else if (GameMng.I.countHungry > (NetworkMng.getInstance.v_user.Count * 3))
        {
            // 체력 감소
            _hp -= 1;
        }

        if (GameMng.I.countHungry < 0 && _actdebuff)
        { 
            _bActAccess = true;
            _actdebuff = false;
        }

        if (_hp < 1)
        {
            DestroyMyself();
            GameMng.I._hextile.GetCell(PosX, PosZ)._unitObj = null;
            GameMng.I._hextile.TilecodeClear(PosX, PosZ);
            NetworkMng.getInstance.SendMsg(string.Format("DIE_UNIT:{0}:{1}", PosX, PosZ));
        }
    }


    public virtual void init()
    {

    }

}