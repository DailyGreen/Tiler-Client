using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_CustomCode     // 건물
{
    E_BUILDING = -1,    // 건물 짓는 중
    E_NONE = 0,
    E_CASTLE = 1,       // 성
    E_REFINERY,         // 정제소
    E_FARM,             // 농장
    E_MWHOUCE,          // 미네랄 창고
    E_FWHOUCE,          // 식량 창고
    E_GAB,              // 지상 유닛 공격 건물
    E_FLAK,             // 대공포
    E_EHMBULIDING,      // 강화 건물
    E_BARRACK,          // 지상 유닛 생산 건물
    E_SBARRACK,         // 총쟁이 생산 건물
    E_AIRPORT,          // 공중 유닛 생산 건물
    E_HARBOR,           // 배 생산 건물
    E_RADER,            // 레이더
    E_NOW_CHARACTER = 30, // ================
    E_WORKMAN,          // 일꾼
    E_SWARRIOR,         // 칼전사
    E_AWARRIOR,         // 도끼전사
    E_SOLIDER,          // 소총수
    E_CATAPULT,         // 투석기
    E_SHIP_0,
    E_SHIP_1,
    E_AIRUNIT_0,
    E_AIRUNIT_1
}

public enum E_Active
{
    E_NONE = 0,
    E_WORKMAN,
    E_MOVE,
    E_BUILD,
    E_ATTACK,
    E_CANCEL,
    E_MAX
}

//public enum E_Object{
//    E_TREE = 10,

//}
