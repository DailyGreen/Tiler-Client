using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int stack;   // º¸À¯ °ñµå·®
    public int making;  // »ý»ê °ñµå·®

    // Start is called before the first frame update
    void Start()
    {
        _name = "±¤»ê";
        _desc = "°ñµå¸¦ Ä¶ ¼ö ÀÖ´Ù";
        _hp = 10;
        _cost = 0;

        stack = 0;
        making = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("g"))
            stack += making;
    }
}
