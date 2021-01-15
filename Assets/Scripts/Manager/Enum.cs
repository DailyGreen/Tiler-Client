using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 타일 방향
 * 좌, 좌하, 우하, 좌상, 우상, 우
 */
public enum DIRECTION
{
    W = 0, SW = 1, SE = 2, NW = 3, NE=4  ,E=5
}

/*
 * 타일 종류에 대한 코드
 */
public enum TILE
{
    GRASS = 0,
    SAND,
    DIRT,
    MARS,
    STONE,
    CAN_MOVE,           // 이전까지 움직일수 있는 타일
    SEA_01,
    SEA_02,
    SEA_03,
    GRASS_START = 'F',
    SAND_START = 'G',
    DIRT_START = 'H',
    MARS_START = 'I',
    STONE_START = 'J',
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
    BUILD_MILLITARY_BASE,       // 유닛 생성 건물 짓기
    BUILD_SHIELD_BUILDING,      // 방어 건물 짓기
    BUILD_UPGRADE_BUILDING,     // 업그레이드 건물 짓기
    WORKER_UNIT_CREATE,         // 워커 유닛 생성
    ATTACK_UNIT_CREATE,         // 공격 유닛 생성
    DESTROY_BUILT,              // 건물 파괴
    ATTACK,                     // 공격
}

public enum BUILT
{
    MINE = 200,
    FARM,
    ATTACK_BUILDING,
    MILLITARY_BASE,
    CASTLE,
    SHIELD_BUILDING,
    UPGRADE_BUILDING,
    AIRDROP
}

public enum UNIT
{
    FOREST_WORKER = 300,
    FORSET_SOILDER,
}

public enum COLOR
{
    COLOR_0 = 0,
    COLOR_1,
    COLOR_2,
    COLOR_3,
    COLOR_4,
    COLOR_5,
    COLOR_6,
    COLOR_7,
    COLOR_8
}

public class CustomColor
{
    public static string TransColor(COLOR color)
    {
        switch (color)
        {
            case COLOR.COLOR_0:
                return "#CA3E77";
            case COLOR.COLOR_1:
                return "#4474C5";
            case COLOR.COLOR_2:
                return "#44C59B";
            case COLOR.COLOR_3:
                return "#E05A5D";
            case COLOR.COLOR_4:
                return "#FBFF3B";
            case COLOR.COLOR_5:
                return "#676767";
            case COLOR.COLOR_6:
                return "#69E4E5";
            case COLOR.COLOR_7:
                return "#A969E5";
            case COLOR.COLOR_8:
                return "#FFA340";
        }
        return "#000000";
    }
}