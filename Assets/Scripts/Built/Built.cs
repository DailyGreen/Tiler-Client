using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Built : DynamicObject
{
    public int _turn = 0;   // 건설시 소모하는 턴수
    void Start()
    {
        _code = 199;
        Debug.Log("128921uwqd");
    }

    public Mine mine = null;

    public Farm farm = null;

    public Turret turret = null;

    /**
     * @brief 건물을 생성
     * @param GameObject built     생성할 건물 게임오브젝트
     * @param int cost             생성할 건물의 건설비용
     */

}