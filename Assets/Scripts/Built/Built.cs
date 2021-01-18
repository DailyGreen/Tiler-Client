using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Built : DynamicObject
{
    public int _turn = 0;   // 건설시 소모하는 턴수
    void Awake()
    {
        _code = 199;
        Debug.Log("Built START");
    }

    public Mine mine = null;

    public Farm farm = null;

    public Turret turret = null;

    public MillitaryBase millitarybase = null;
}