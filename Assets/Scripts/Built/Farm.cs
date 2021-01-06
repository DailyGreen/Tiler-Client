using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Built
{
    public int making;  // 식량 생산량
    public static int cost = 4;   // 건설 비용

    // Start is called before the first frame update
    void Start()
    {
        _name = "농장";
        _desc = "식량을 생산한다";
        _hp = 10;
        _code = (int)BUILT.FARM;
        making = 2;
    }

    // Update is called once per frame
    void Update()
    {
        MakingFood();
    }

    /**
     * @brief 식량 생산
     */
    void MakingFood()
    {
        if (Input.GetKeyDown("f"))
            GameMng.I.addMaxMem(making);
    }
}
