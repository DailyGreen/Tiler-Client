using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDrop : Built
{
    void Start()
    {
        _name = "����ǰ";
        _desc = "�ķ��� ����� �������� ȹ���� �� �ִ�";
        _max_hp = 1;
        _hp = _max_hp;
        _uniqueNumber = -1;
        _code = (int)BUILT.AIRDROP;
    }

}
