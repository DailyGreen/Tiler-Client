using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Built
{
    void Start()
    {
        //uniqueNumber = NetworkMng.getInstance.uniqueNumber;                                         //성의 유니크 넘버를 정해줌
        _name = "성";
        _desc = "고향을 지켜라!!";
        _max_hp = 15;
        _hp = _max_hp;
        _code = (int)BUILT.CASTLE;

        if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            _activity.Add(ACTIVITY.WORKER_UNIT_CREATE);
    }

    /*
    * @brief 일꾼 생성
    */
    public static void CreateUnitBtn()
    {
        GameMng.I._range.moveRange();
        //if (GameMng.I.selectedTile._code == (int)BUILT.CASTLE)
        //{
        //    GameMng.I._BuiltGM.act = ACTIVITY.WORKER_UNIT_CREATE;
        //    GameMng.I._range.moveRange();
        //}
        Debug.Log("일꾼 유닛 생성");
    }
}
