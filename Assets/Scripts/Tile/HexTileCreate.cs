using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class HexTileCreate : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject hextile;
    public Tile tilestate;

    // Ÿ�� �� ũ��
    private const int mapWidth = 20;
    private const int mapHeight = 5;

    // Ÿ�� ����
    public const float tileXOffset = 1.24f;
    public const float tileYOffset = 1.08f;

    public int tempX;

    string[] mapReadLines = File.ReadAllLines(@"Assets/mapinfo.txt");
    char[] mapReadChar;
    // Start is called before the first frame update
    void Start()
    {
        CreateHexTileMap();
    }

    /**
     * @brief ��� Ÿ�ϸ� ����, Ÿ�� posX posY ����
     */
    void CreateHexTileMap()
    {
        for (int x = 0; x < mapHeight; x++)
        {
            mapReadChar = mapReadLines[x].ToCharArray();
            for (int y = 0; y < mapWidth; y++)
            {
                tilestate.Code = mapReadChar[y];
                if (!tempX.Equals(x))
                {
                    tilestate.PosY = 0;
                }
                GameObject child = Instantiate(hextile) as GameObject;
                child.transform.parent = parentObject.transform;

                if (y % 2 == 0)
                {
                    child.transform.position = new Vector2(x * tileXOffset, y * tileYOffset);
                }
                else
                {
                    child.transform.position = new Vector2(x * tileXOffset + tileXOffset / 2, y * tileYOffset);
                }
                tempX = x;
                tilestate.PosY++;
            }
            tilestate.PosX++; ;
        }
    }
}
