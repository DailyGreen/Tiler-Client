using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillitaryBase : Built
{
    public static int cost = 10;   // 건설 비용
    // Start is called before the first frame update
    void Start()
    {
        _name = "군사 기지";
        _desc = "유닛들을 생성한다";
        _hp = 10;
        _activity.Add(ACTIVITY.ATTACK_UNIT_CREATE);
        _activity.Add(ACTIVITY.DESTROY_BUILT);
        _code = (int)BUILT.MILLITARY_BASE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CreateAttackUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.ATTACK_UNIT_CREATE;
            GameMng.I._range.moveRange();
        }
        Debug.Log("병력 유닛 생성할 위치 선정");

    }

}
