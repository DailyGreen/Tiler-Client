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

    public Tile[] starttile = new Tile[24];
    int index = 0;

    // Ÿ�� ����
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
     * @brief ��� Ÿ�ϸ� ����, Ÿ�� posX posY ����, ���� ��ġ ����
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

                GameMng.I.mapTile[y, x] = child.transform.GetComponent<Tile>();      // ������ Ÿ�� ��ũ��Ʈ GameMng.I.mapTile 2���� �迭�� ����

                // mapinfo.txt  �� start_point �ڵ��϶� starttile �� Ÿ�� ��ũ��Ʈ �־���
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
     * @brief NetworkMng ���� �� �޾ƿ� �� ���� �� ������ȣ �и�
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
     * @brief Ÿ���� x,y,z �� ���� �� ���������� ���� ������ �ȵ����� �⺻ Ÿ�ϰ����� �ʱ�ȭ
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