using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Built
{
    void Start()
    {
        //uniqueNumber = NetworkMng.getInstance.uniqueNumber;                                         //성의 유니크 넘버를 정해줌
        _name = string.Format("{0} 종족 성  (소유자 : {1})", GameMng.I.getUserTribe(_uniqueNumber), GameMng.I.getUserName(_uniqueNumber));
        _desc = "일꾼을 생성한다";
        _max_hp = 15;
        _hp = _max_hp;
        _code = (int)BUILT.CASTLE;
        _anim.SetTrigger("isSpawn");
        _emoteSide.color = GetUserColor(_uniqueNumber);

        if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            _activity.Add(ACTIVITY.WORKER_UNIT_CREATE);
    }

    /*
     * @brief 일꾼 생성
     */
    public static void CreateUnitBtn()
    {
        GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        //GameMng.I.cleanActList();
    }
}
