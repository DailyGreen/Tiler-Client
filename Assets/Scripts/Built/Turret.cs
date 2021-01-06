using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Built
{
    public int attack;   // 공격력
    public static int cost = 5;   // 건설 비용
    // Start is called before the first frame update
    void Start()
    {
        _name = "터렛";
        _desc = "턴이 끝날 때 사정거리 안의 적을 공격한다";
        _hp = 7;
        attack = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
