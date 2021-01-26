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

    public string _unitDesc;            // 유닛 생성 시 나오는 설명

    /**
     * @brief 유닛 생성 대기 및 생성 함수
     */
    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";

        if (createCount > maxCreateCount - 1)
        {
            GameMng.I._hextile.GetCell(SaveX, SaveY)._builtObj.GetComponent<Built>()._bActAccess = true;
            GameMng.I._hextile.GetCell(SaveX, SaveY)._builtObj._anim.SetTrigger("isComplete");

            if (this._code != (int)UNIT.FOREST_WORKER && this._code != (int)UNIT.SEA_WORKER && this._code != (int)UNIT.DESERT_WORKER)
                GameMng.I._hextile.GetCell(SaveX, SaveY)._builtObj.GetComponent<MillitaryBase>().CreatingUnitobj = null;

            _desc = _unitDesc;

            _anim.SetTrigger("isSpawn");

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
                init();

            GameMng.I.RemoveDelegate(waitingCreate);
        }
    }

    public virtual void init()
    {

    }

}