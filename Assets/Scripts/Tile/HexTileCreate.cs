using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class HexTileCreate : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject hextile;
    public Tile tilestate;      // 프림
    public GameObject castle;
    public GameObject tilecild;

    public Tile[] starttile = new Tile[24];
    int index = 0;

    // 타일 간격
    public const float tileXOffset = 1.24f;
    public const float tileYOffset = 1.08f;

    string[] mapReadLines = File.ReadAllLines(@"Assets/mapinfo.txt");
    char[] mapReadChar;

    // Start is called before the first frame update
    void Awake()
    {
        CreateHexTileMap();
        CastleCreate();
        SetTileInfo();
        GameMng.I.refreshTurn();
    }

    void OnApplicationQuit()
    {
        tilestate.PosX = 0;
        tilestate.PosY = 0;
    }

    /**
     * @brief 헥사 타일맵 생성, 타일 posX posY 설정, 시작 위치 설정
     */
    void CreateHexTileMap()
    {
        for (int y = 0; y < GameMng.I.GetMapHeight; y++)
        {
            mapReadChar = mapReadLines[y].ToCharArray();
            for (int x = 0; x < GameMng.I.GetMapWidth; x++)
            {
                if (mapReadChar[x] >= (char)TILE.GRASS_START) { tilestate._code = (int)mapReadChar[x]; }
                else { tilestate._code = (int)Char.GetNumericValue(mapReadChar[x]); }
                GameObject child = Instantiate(hextile) as GameObject;
                child.transform.parent = parentObject.transform;

                GameMng.I.mapTile[y, x] = child.transform.GetComponent<Tile>();      // 각각의 타일 스크립트 GameMng.I.mapTile 2차원 배열에 저장

                // mapinfo.txt  에 start_point 코드일때 starttile 에 타일 스크립트 넣어줌
                if (tilestate._code >= (int)TILE.GRASS_START)
                {
                    starttile[index] = GameMng.I.mapTile[y, x];
                    index++;
                }
                child.name = x.ToString() + "," + y.ToString();
                if (y % 2 == 0)
                {
                    child.transform.position = new Vector2(x * tileXOffset, y * tileYOffset);
                }
                else
                {
                    child.transform.position = new Vector2(x * tileXOffset + tileXOffset / 2, y * tileYOffset);
                }

                tilestate.PosX++;
            }
            tilestate.PosX = 0;
            tilestate.PosZ++;
        }
    }

    /**
     * @brief NetworkMng 에서 값 받아와 성 생성 후 고유번호 분리
     */
    void CastleCreate()
    {
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            tilecild = Instantiate(castle, starttile[NetworkMng.getInstance.v_user[i].startPos].transform) as GameObject;
            starttile[NetworkMng.getInstance.v_user[i].startPos]._builtObj = tilecild.GetComponent<Built>();
            starttile[NetworkMng.getInstance.v_user[i].startPos]._builtObj._uniqueNumber = NetworkMng.getInstance.v_user[i].uniqueNumber;
            starttile[NetworkMng.getInstance.v_user[i].startPos]._code = (int)BUILT.CASTLE;
            Debug.Log(starttile[NetworkMng.getInstance.v_user[i].startPos]._code);
        }
    }

    /**
     * @brief 타일의 x,y,z 값 설정 및 시작지점에 성이 생성이 안됐을때 기본 타일값으로 초기화
     */
    void SetTileInfo()
    {
        for (int y = 0; y < GameMng.I.GetMapHeight; y++)
        {
            for (int x = 0; x < GameMng.I.GetMapWidth; x++)
            {
                GameMng.I.mapTile[y, x].PosX = GameMng.I.mapTile[y,x].PosX - GameMng.I.mapTile[y, x].PosZ / 2;
                GameMng.I.mapTile[y, x].PosY = -GameMng.I.mapTile[y, x].PosX - GameMng.I.mapTile[y, x].PosZ; 
                if (GameMng.I.mapTile[y, x]._code >= (int)TILE.GRASS_START && GameMng.I.mapTile[y, x]._code < (int)BUILT.CASTLE)
                {
                    GameMng.I.mapTile[y, x]._code -= (int)TILE.GRASS_START;
                }
            }
        }
    }
}