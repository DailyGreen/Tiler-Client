using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    void Start()
    {
        _hp = 0;
        _cost = 0;
        _damage = 0;
        _code = 1;
    }

    void Update()
    {
        CharClickMove(this.gameObject);
    }
}
