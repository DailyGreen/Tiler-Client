using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Built
{
    void Awake()
    {
        //uniqueNumber = NetworkMng.getInstance.uniqueNumber;                                         //���� ����ũ �ѹ��� ������
        _name = "��";
        _desc = "�ϲ��� �����Ѵ�";
        _hp = 15;
        _code = (int)BUILT.CASTLE;
        _activity.Add(ACTIVITY.WORKER_UNIT_CREATE);
    }

    /*
    * @brief �ϲ� ����
    */
    public static void CreateUnitBtn()
    {
        GameMng.I._range.moveRange();
        //if (GameMng.I.selectedTile._code == (int)BUILT.CASTLE)
        //{
        //    GameMng.I._BuiltGM.act = ACTIVITY.WORKER_UNIT_CREATE;
        //    GameMng.I._range.moveRange();
        //}
        Debug.Log("�ϲ� ���� ����");
    }
}
