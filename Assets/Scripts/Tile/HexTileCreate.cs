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

    // 타일 간격
    public const float tileXOffset = 1.24f;
    public const float tileYOffset = 1.08f;

    string[] mapReadLines = File.ReadAllLines(@"Assets/mapinfo.txt");
    char[] mapReadChar;
    // Start is called before the first frame update
    void Start()
    {
        CreateHexTileMap();
    }

    void OnApplicationQuit()
    {
        tilestate.PosX = 0;
        tilestate.PosY = 0;
    }

    /**
     * @brief 헥사 타일맵 생성, 타일 posX posY 설정
     */
    void CreateHexTileMap()
    {
        for (int x = 0; x < GameMng.I.GetMapHeight; x++)
        {
            mapReadChar = mapReadLines[x].ToCharArray();
            for (int y = 0; y < GameMng.I.GetMapWidth; y++)
            {
                tilestate._code = (int)Char.GetNumericValue(mapReadChar[y]);
                GameObject child = Instantiate(hextile) as GameObject;
                child.transform.parent = parentObject.transform;

                GameMng.I.mapTile[x,y] = child.transform.GetComponent<Tile>();      // 각각의 타일 스크립트 GameMng.I.mapTile 2차원 배열에 저장
                if (y % 2 == 0)
                {
                    child.transform.position = new Vector2(x * tileXOffset, y * tileYOffset);
                }
                else
                {
                    child.transform.position = new Vector2(x * tileXOffset + tileXOffset / 2, y * tileYOffset);
                }
                tilestate.PosY++;
            }
            tilestate.PosY = 0;
            tilestate.PosX++;
        }
    }
}
