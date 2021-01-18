using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HexTileCreate : MonoBehaviour
{
    /**********
     * Ÿ�� ������Ʈ
     */
    public GameObject parentObject;
    public GameObject hextile;
    public Tile tilestate;      // ����
    public GameObject castle;
    public GameObject tilecild;

    /**********
     * ����
     */
    public Tile[] starttile = new Tile[24];
    int index = 0;
    [SerializeField]
    GameObject mainCamera;

    /**********
     * Ÿ�� ����
     */
    public const float tileXOffset = 1.24f;
    public const float tileYOffset = 1.08f;

    /**********
     * .txt ���� ��������, �ڵ� �Ѱ��� ��������
     */
    TextAsset maptextload;
    String[] mapreadlines;
    char[] mapReadChar;

    void Awake()
    {
        LoadTilemapfromtxt("mapinfo");
        CreateHexTileMap();
        CastleCreate();
        SetTileInfo();
        GameMng.I.refreshTurn();
    }

    /*
    * @brief ������ �������� �� �����鿡 �����ִ� ���� �����ֱ�
    */
    void OnApplicationQuit()
    {
        tilestate.PosX = 0;
        tilestate.PosY = 0;
    }

    /*
    * @brief .txt������ �� �����͸� �ҷ���
    * @param _filename .txt ���� �̸�
    */
    void LoadTilemapfromtxt(string _filename)
    {
        maptextload = Resources.Load(_filename) as TextAsset;
        mapreadlines = maptextload.text.Split('\n');
    }

    /**
     * @brief ��� Ÿ�ϸ� ����, Ÿ�� posX posY ����, ���� ��ġ ����
     */
    void CreateHexTileMap()
    {
        for (int y = 0; y < GameMng.I.GetMapHeight; y++)
        {
            mapReadChar = mapreadlines[y].ToCharArray();
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
            tilestate.PosY++;
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

            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
            {
                Debug.Log("POS " + starttile[NetworkMng.getInstance.v_user[i].startPos].transform.position);
                Vector3 tempVec = starttile[NetworkMng.getInstance.v_user[i].startPos].transform.position;
                tempVec.z = -20;

                GameMng.I.CastlePos = new Vector3(starttile[NetworkMng.getInstance.v_user[i].startPos].transform.position.x, starttile[NetworkMng.getInstance.v_user[i].startPos].transform.position.y, -20f);

                mainCamera.transform.position = tempVec;
                Debug.Log("POS2 " + mainCamera.transform.position);
            }
        }
    }

    /**
    * @brief Ÿ���� x,y,z �� ���� �� ���������� ���� ������ �ȵ����� �⺻ Ÿ�ϰ����� �ʱ�ȭ �� Ÿ�� �ֺ� ��ũ��Ʈ ������
    */
    void SetTileInfo()
    {
        for (int y = 0; y < GameMng.I.GetMapHeight; y++)
        {
            for (int x = 0; x < GameMng.I.GetMapWidth; x++)
            {
                //GameMng.I.mapTile[y, x].PosX = GameMng.I.mapTile[y, x].PosX - GameMng.I.mapTile[y, x].PosZ / 2;
                //GameMng.I.mapTile[y, x].PosY = -GameMng.I.mapTile[y, x].PosX - GameMng.I.mapTile[y, x].PosZ;
                if (GameMng.I.mapTile[y, x]._code >= (int)TILE.GRASS_START && GameMng.I.mapTile[y, x]._code < (int)BUILT.CASTLE)
                {
                    GameMng.I.mapTile[y, x]._code -= (int)TILE.GRASS_START;
                }
                if (x > 0)
                {
                    GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.W] = GameMng.I.mapTile[y, x - 1];
                }
                if (x >= 0 && !x.Equals(GameMng.I.GetMapWidth - 1))
                {
                    GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.E] = GameMng.I.mapTile[y, x + 1];
                }

                if (y > 0)
                {
                    if (y % 2 == 0)         // ¦��
                    {
                        if (x > 0)
                        {
                            GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.SW] = GameMng.I.mapTile[y - 1, x - 1];
                            GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.NW] = GameMng.I.mapTile[y + 1, x - 1];
                        }
                        Debug.Log("Asdf");
                        if (x >= 0 /*&& !(x < GameMng.I.GetMapWidth)*/)
                        {
                            GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.SE] = GameMng.I.mapTile[y - 1, x];
                            GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.NE] = GameMng.I.mapTile[y + 1, x];
                        }
                    }
                    else
                    {
                        GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.SW] = GameMng.I.mapTile[y - 1, x];      // ����
                        if (!y.Equals(GameMng.I.GetMapHeight - 1))
                            GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.NW] = GameMng.I.mapTile[y + 1, x];      // �»�   

                        if (x >= 0 && !x.Equals(GameMng.I.GetMapWidth - 1))
                        {
                            GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.SE] = GameMng.I.mapTile[y - 1, x + 1];      // ����
                            if (!y.Equals(GameMng.I.GetMapHeight - 1) && !x.Equals(GameMng.I.GetMapWidth - 1))
                                GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.NE] = GameMng.I.mapTile[y + 1, x + 1];      // ���

                        }
                    }
                }
                else
                {
                    if (y % 2 == 0)
                    {
                        if (x > 0)
                            GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.NW] = GameMng.I.mapTile[y + 1, x - 1];

                        GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.NE] = GameMng.I.mapTile[y + 1, x];
                    }
                    else
                    {
                        if (!y.Equals(GameMng.I.GetMapHeight - 1))
                            GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.NW] = GameMng.I.mapTile[y + 1, x];
                        if (!y.Equals(GameMng.I.GetMapHeight - 1) && !x.Equals(GameMng.I.GetMapWidth - 1))
                            GameMng.I.mapTile[y, x].tileneighbor[(int)DIRECTION.NE] = GameMng.I.mapTile[y + 1, x + 1];
                    }
                }
            }
        }
    }

    /**
        * @brief Ÿ�� �ڵ� �ʱ�ȭ(������ �̵����� ��, �ǹ��� �μ����� ��) �޸��忡 code �� �����ͼ� �ٲ���
        * @param posX GameMng.I.seleteTile.PoX ��Ȱ
        * @param posY GameMng.I.seleteTile.PoY ��Ȱ
        */
    public void TilecodeClear(int posX, int posY)
    {
        if (GameMng.I.mapTile[posY, posX]._unitObj == null || GameMng.I.mapTile[posY, posX]._builtObj == null)
        {
            mapReadChar = mapreadlines[posY].ToCharArray();
            if (mapReadChar[posX] >= (int)TILE.GRASS_START)
            {
                GameMng.I.mapTile[posY, posX]._code = (int)mapReadChar[posX] - (int)TILE.GRASS_START;
            }
            else { GameMng.I.mapTile[posY, posX]._code = (int)Char.GetNumericValue(mapReadChar[posX]); }
        }
    }
}