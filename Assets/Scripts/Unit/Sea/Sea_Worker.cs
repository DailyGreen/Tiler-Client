using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Worker : Worker
{
    void Awake()
    {
        init();
        _name = "�� ���� �ϲ�";
        _desc = "������ ���δ�.";
        _cost = 0;
        _code = (int)UNIT.FOREST_WORKER;
    }
}
