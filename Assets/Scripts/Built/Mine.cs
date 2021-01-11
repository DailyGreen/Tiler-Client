using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int making;  // °ñµå »ý»ê·®
    public static int cost = 1;   // °Ç¼³ ºñ¿ë

    void Start()
    {
        _name = "±¤»ê";
        _desc = "°ñµå¸¦ Ä¶ ¼ö ÀÖ´Ù";
        _hp = 10;
        _code = (int)BUILT.MINE;
        making = 5;
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    void Update()
    {
        MakingGold();
    }

    /**
     * @brief °ñµå »ý»ê
     */
    void MakingGold()
    {
        if (Input.GetKeyDown("g"))
            GameMng.I.addGold(making);
    }
}
