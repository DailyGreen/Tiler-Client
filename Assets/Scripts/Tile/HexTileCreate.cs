using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class HexTileCreate : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject hextile;
    public Tile tilestate;      // ����

    // Ÿ�� ����
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
     * @brief ��� Ÿ�ϸ� ����, Ÿ�� posX posY ����
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

                GameMng.I.mapTile[x,y] = child.transform.GetComponent<Tile>();      // ������ Ÿ�� ��ũ��Ʈ GameMng.I.mapTile 2���� �迭�� ����
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
