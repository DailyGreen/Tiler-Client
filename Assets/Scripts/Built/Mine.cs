using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int making;              // °ñµå »ý»ê·®
    public static int cost = 1;     // °Ç¼³ ºñ¿ë

    void Start()
    {
        init();
        GameMng.I.AddDelegate(MakingGold);
    }

    void init()
    {
        _name = "±¤»ê";
        _desc = "°ñµå¸¦ Ä¶ ¼ö ÀÖ´Ù";
        _hp = 10;
        _code = (int)BUILT.MINE;
        making = 5;
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    /**
     * @brief °ñµå »ý»ê
     */
    void MakingGold()
    {
        Debug.Log("MINE CALLING");
        GameMng.I.addGold(making);
    }

    void OnDestroy()
    {
        GameMng.I.RemoveDelegate(MakingGold);
    }
}
