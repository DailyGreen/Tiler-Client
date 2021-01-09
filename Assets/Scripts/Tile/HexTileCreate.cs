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
    public GameObject castle;
    public GameObject tilecild;

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
     * @brief ��� Ÿ�ϸ� ����, Ÿ�� posX posY ����, ���� ��ġ ����
     */
    void CreateHexTileMap()
    {
        for (int y = 0; y < GameMng.I.GetMapHeight; y++)
        {
            mapReadChar = mapReadLines[y].ToCharArray();
            for (int x = 0; x < GameMng.I.GetMapWidth; x++)
            {
                tilestate._code = (int)Char.GetNumericValue(mapReadChar[x]);
                GameObject child = Instantiate(hextile) as GameObject;
                child.transform.parent = parentObject.transform;

                GameMng.I.mapTile[y, x] = child.transform.GetComponent<Tile>();      // ������ Ÿ�� ��ũ��Ʈ GameMng.I.mapTile 2���� �迭�� ����

                // mapinfo.txt  �� start_point �ڵ��϶� ���� Instantiate �ϰ� tile._builtObj �� �� ��ũ��Ʈ �־����� �ϼ��� �ڵ带 tile._code �� ���� (�ӽ�)
                if (tilestate._code.Equals((int)TILE.START_POINT))
                {
                    tilecild = Instantiate(castle, GameMng.I.mapTile[y, x].transform) as GameObject;
                    GameMng.I.mapTile[y, x]._builtObj = tilecild.GetComponent<Built>();
                    GameMng.I.mapTile[y, x]._code = (int)BUILT.CASTLE;
                }
                
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
            tilestate.PosY++;
        }
    }
}
