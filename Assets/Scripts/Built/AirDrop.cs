using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDrop : Built
{
    void Start()
    {
        init();
    }

    void init()
    {
        _name = "����ǰ";
        _desc = "�ķ��� ����� �������� ȹ���� �� �ִ�";
        _hp = 1;
        _code = (int)BUILT.AIRDROP;
    }
}