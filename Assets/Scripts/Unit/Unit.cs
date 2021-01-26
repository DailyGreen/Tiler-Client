using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : DynamicObject
{
    /**
     * ���� ���� ����
     * �ݵ�� ��ӹ޴� �ڽ��� Start ���� ���� ������ �ֽñ� �ٶ��ϴ�.
     */
    public int _damage = 0;

    public string _unitDesc;            // ���� ���� �� ������ ����

    /**
     * @brief ���� ���� ��� �� ���� �Լ�
     */
    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

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