using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE
{
    GRASS = 0,
    SAND,
    DIRT,
    MARS,
    STONE,
    CAN_MOVE,
}

/*
 * 어떤 행동인지에 대한 코드
 */
public enum ACTIVITY
{
    NONE = 100,
    ACTING,                     // 행동중
    MOVE,                       // 이동
    BUILD_MINE,                 // 광산 짓기
    BUILD_FARM,                 // 농장 짓기
    BUILD_ATTACK_BUILDING,      // 공격 건물 짓기
    BUILD_CREATE_UNIT_BUILDING, // 유닛 생성 건물 짓기
    BUILD_SHIELD_BUILDING,      // 방어 건물 짓기
    BUILD_UPGRADE_BUILDING,     // 업그레이드 건물 짓기
    WORKER_UNIT_CREATE,          // 워커 유닛 생성
}

public enum UNIT
{
    WORKER = 100
}


public enum BUILT
{
    MINE = 200,
    FARM,
    ATTACK_BUILDING,
    CREATE_UNIT_BUILDING,
    SHIELD_BUILDING,
    UPGRADE_BUILDING,
    CASTLE
}