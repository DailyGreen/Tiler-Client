using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Worker : Worker
{
    public static int cost = 4;
    void Awake()
    {
        init();
        _name = "�� ���� �ϲ�";
        _desc = "������ ���δ�.";
        _code = (int)UNIT.FOREST_WORKER;
    }
}
