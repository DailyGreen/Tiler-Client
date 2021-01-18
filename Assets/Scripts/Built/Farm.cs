using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Built
{
    public int making;              // 식량 생산량
    public static int cost = 4;     // 건설 비용

    void Awake()
    {
        _name = "농장";
        _desc = "식량을 생산한다";
        _hp = 10;
        _code = (int)BUILT.FARM;
        making = 2;
        GameMng.I.AddDelegate(MakingFood);
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    /**
     * @brief 식량 생산
     */
    void MakingFood()
    {
        Debug.Log("MAKING FOOD CALL !!!!!!!!");
        GameMng.I.addFood(making);
    }

    /**
     * @brief  식량 약탈 공격력 퍼센트 (밸런스 조정 필요)
     * @param attactdmg 공격유닛 대미지
     */
    public void FoodPlunder(int attactdmg)
    {
        GameMng.I._food = GameMng.I._food * attactdmg / 100;
    }

    void OnDestroy()
    {
        GameMng.I.RemoveDelegate(MakingFood);
    }
}
