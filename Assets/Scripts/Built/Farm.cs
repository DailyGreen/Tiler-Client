using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Built
{
    public int making;              // 식량 생산량
    public static int cost = 1;     // 건설 비용

    void Awake()
    {
        _name = "농장";
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";
        _hp = 10;
        _code = (int)BUILT.FARM;
        making = 2;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";

        // 2턴 후에 생성됨
        if (createCount > 2)
        {
            _desc = "식량을 생산한다";

            //_anim.SetTrigger("isSpawn");

            GameMng.I.RemoveDelegate(this.waitingCreate);

            // 내꺼라면
            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();

                GameMng.I.AddDelegate(MakingFood);
            }
        }
    }

    /**
     * @brief 식량 생산
     */
    void MakingFood()
    {
        Debug.Log("MAKING FOOD CALL !!!!!!!!");
        GameMng.I.addFood(making);
    }
    void OnDestroy()
    {
        if (createCount > 2 && NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            GameMng.I.RemoveDelegate(MakingFood);
        else
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
