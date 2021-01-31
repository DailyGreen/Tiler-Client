using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDrop : Built
{
    void Start()
    {
        _name = "보급품";
        _desc = "식량과 골드중 랜덤으로 획득할 수 있다";
        _max_hp = 1;
        _hp = _max_hp;
        _uniqueNumber = -1;
        _code = (int)BUILT.AIRDROP;
    }

}
