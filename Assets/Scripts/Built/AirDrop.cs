using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDrop : Built
{
    // Start is called before the first frame update
    void Start()
    {
        _name = "보급품";
        _desc = "식량과 골드중 랜덤으로 획득할 수 있다";
        _hp = 1;
        _code = (int)BUILT.AIRDROP;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
