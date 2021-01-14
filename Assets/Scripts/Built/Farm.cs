using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Built
{
    public int making;              // 식량 생산량
    public static int cost = 4;     // 건설 비용

    void Start()
    {
        init();
        GameMng.I.AddDelegate(MakingFood);
    }

    void init()
    {
        _name = "농장";
        _desc = "식량을 생산한다";
        _hp = 10;
        _code = (int)BUILT.FARM;
        making = 2;
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    /**
     * @brief 식량 생산
     */
    void MakingFood()
    {
        Debug.Log("MAKING FOOD CALL !!!!!!!!");
        GameMng.I.addMaxMem(making);
    }

    void OnDestroy()
    {
        GameMng.I.RemoveDelegate(MakingFood);
    }
}
