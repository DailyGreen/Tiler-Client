using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea_Worker : Worker
{
    public static int cost = 4;
    void Awake()
    {
        init();
        _name = "물 종족 일꾼";
        _desc = "듬직해 보인다.";
        _code = (int)UNIT.FOREST_WORKER;
    }
}
