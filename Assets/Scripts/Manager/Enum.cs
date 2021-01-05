using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 어떤 행동인지에 대한 코드
 */
public enum ACTIVITY
{
    NONE = 0,                   // 광산 짓기
    BUILD_MINE,                 // 광산 짓기
    BUILD_FARM,                 // 농장 짓기
    BUILD_ATTACK_BUILDING,      // 공격 건물 짓기
    BUILD_CREATE_UNIT_BUILDING, // 유닛 생성 건물 짓기
    BUILD_SHIELD_BUILDING,      // 방어 건물 짓기
    BUILD_UPGRADE_BUILDING      // 업그레이드 건물 짓기
}