using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HexTileCreate : MonoBehaviour
{
    /**********
     * ?? ????
     */
    public GameObject parentObject;
    public GameObject hextile;
    public Tile tilestate;      // ??
    public GameObject castle;
    public GameObject tilecild;

    /**********
     * ??
     */
    public Tile[] starttile = new Tile[24];
    public Tile[] cells;
    int index = 0;      // ?? ?? ???
    int i = 0;          // ?? ???
    [SerializeField]
    GameObject mainCamera;

    /**********
     * ?? ??
     */
    public const float tileXOffset = 1.24f;
    public const float tileYOffset = 1.08f;

    /**********
     * .txt ?? ????, ?? ??? ????
     */
    TextAsset maptextload;
    String[] mapreadlines;
    char[] mapReadChar;

    void Awake()
    {
        LoadTilemapfromtxt("mapinfo");
        CreateHexTilePostion();
        CastleCreate();
        SetTileInfo();
        GameMng.I.refreshTurn();
    }

    /**
     * @brief ??? ???? ‹š ???? ???? ?? ????
     */
    void OnApplicationQuit()
    {
        tilestate.PosX = 0;
        tilestate.PosY = 0;
        tilestate.PosZ = 0;
        tilestate.tileuniquecode = 0;
    }

    /**
     * @brief .txt??? ? ???? ???
     * @param _filename .txt ?? ??
     */
    void LoadTilemapfromtxt(string _filename)
    {
        maptextload = Resources.Load(_filename) as TextAsset;
        mapreadlines = maptextload.text.Split('\n');
    }

    /**
     * @brief ?? ??? ??, ?? posX posY ??, ?? ?? ??
     */
    void CreateHexTilePostion()
    {
        cells = new Tile[GameMng.I.GetMapHeight * GameMng.I.GetMapWidth];
        for (int y = 0; y < GameMng.I.GetMapHeight; y++)
        {
            mapReadChar = mapreadlines[y].ToCharArray();
            for (int x = 0; x < GameMng.I.GetMapWidth; x++)
            {
                tilestate.PosX = x - y / 2; ;
                tilestate.PosY = tilestate.PosY;
                if (mapReadChar[x] >= (char)TILE.GRASS_START) { tilestate._code = (int)mapReadChar[x]; }
                else { tilestate._code = (int)Char.GetNumericValue(mapReadChar[x]); }
                tilestate.tileuniquecode = tilestate._code;
               
                GameObject child = Instantiate(hextile) as GameObject;
                child.transform.parent = parentObject.transform;
                cells[i] = child.GetComponent<Tile>();

                // mapinfo.txt  ? start_point ???? starttile ? ?? ???? ???
                if (tilestate._code >= (int)TILE.GRASS_START)
                {
                    starttile[index] = cells[i];
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
                SetNeighborTile(x, y, i++);
            }
            tilestate.PosZ++;
        }
    }

    /**
     * @brief ???? ??
     */
    void SetNeighborTile(int x, int z, int i)
    {
        if (x > 0)
        {
            cells[i].SetNeighbor(DIRECTION.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cells[i].SetNeighbor(DIRECTION.SE, cells[i - GameMng.I.GetMapWidth]);
                if (x > 0)
                {
                    cells[i].SetNeighbor(DIRECTION.SW, cells[i - GameMng.I.GetMapWidth - 1]);
                }
            }
            else
            {
                cells[i].SetNeighbor(DIRECTION.SW, cells[i - GameMng.I.GetMapWidth]);
                if (x < GameMng.I.GetMapWidth - 1)
                {
                    cells[i].SetNeighbor(DIRECTION.SE, cells[i - GameMng.I.GetMapWidth + 1]);
                }
            }
        }
    }

    /**
     * @brief NetworkMng ?? ? ??? ? ?? ? ???? ??
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
    * @brief ????? ?? ??? ???? ?? ????? ???
    */
    void SetTileInfo()
    {
        for (int i = 0; i < starttile.Length; i++)
        {
            if (starttile[i]._code >= (int)TILE.GRASS_START && starttile[i]._code < (int)BUILT.CASTLE)
            {
                starttile[i]._code -= (int)TILE.GRASS_START;
            }
        }
    }

    /**
     * @brief ?? ? ????
     * @param posX ?? x?
     * @param posy ?? y?
     */
    public Tile GetCell(int posX, int posZ)
    {
        int z = posZ;
        if (z < 0 || z >= GameMng.I.GetMapHeight)
        {
            return null;
        }
        int x = posX + z / 2;
        if (x < 0 || x >= GameMng.I.GetMapWidth)
        {
            return null;
        }
        return cells[x + z * GameMng.I.GetMapWidth];
    }

    /**
    * @brief ?? ?? ???(??? ???? ?, ??? ???? ‹š) ???? code ? ???? ???
    * @param posX GameMng.I.seleteTile.PoX ??
    * @param posY GameMng.I.seleteTile.PoY ??
    */
    public void TilecodeClear(int posX, int posY)
    {
        if (GameMng.I._hextile.GetCell(posX, posY).tileuniquecode >= (int)TILE.GRASS_START)
            GameMng.I._hextile.GetCell(posX, posY)._code = GameMng.I._hextile.GetCell(posX, posY).tileuniquecode - (int)TILE.GRASS_START;
        else
            GameMng.I._hextile.GetCell(posX, posY)._code = GameMng.I._hextile.GetCell(posX, posY).tileuniquecode;
    }

    /**
     * @brief ??? ?? ??
     * @param cell ??? ??
     */
    public void FindDistancesTo(Tile cell)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = cell.DistanceTo(cells[i]);
        }
    }
}
