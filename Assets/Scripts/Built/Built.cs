using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Built : DynamicObject
{
    void Awake()
    {
        _code = 199;
        Debug.Log("Built START");
    }
}