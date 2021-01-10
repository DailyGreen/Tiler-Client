using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Built
{
    // Start is called before the first frame update
    void Start()
    {
        uniqueNumber = NetworkMng.getInstance.uniqueNumber;                                         // ���� ����ũ �ѹ��� ������
        _name = "��";
        _desc = "�ϲ��� �����Ѵ�";
        _hp = 15;
        _code = (int)BUILT.CASTLE;
        _activity.Add(ACTIVITY.WORKER_UNIT_CREATE);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void CreateUnit()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.CASTLE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.WORKER_UNIT_CREATE;
            GameMng.I._range.moveRange();
        }

    }
}
