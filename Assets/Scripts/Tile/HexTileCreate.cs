using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HexTileCreate : MonoBehaviour
{
    /**********
     * 타일 오브젝트
     */
    public GameObject parentObject;
    public GameObject hextile;
    public Tile tilestate;      // 프림
    public GameObject castle;
    public GameObject tilecild;

    /**********
     * 세팅
     */
    public Tile[] starttile = new Tile[24];
    public Tile[] cells;
    int index = 0;      // 시작 지점 인덱스
    int i = 0;          // 타일 인덱스
    [SerializeField]
    GameObject mainCamera;

    /**********
     * 타일 간격
     */
    public const float tileXOffset = 1.24f;
    public const float tileYOffset = 1.08f;

    /**********
     * .txt 파일 가져오기, 코드 한개씩 가져오기
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
     * @brief 게임을 종료했을 떄 프리펩에 남아있는 정보 지워주기
     */
    void OnApplicationQuit()
    {
        tilestate.PosX = 0;
        tilestate.PosY = 0;
        tilestate.PosZ = 0;
        tilestate.tileuniquecode = 0;
    }

    /**
     * @brief .txt형식의 맵 데이터를 불러옴
     * @param _filename .txt 파일 이름
     */
    void LoadTilemapfromtxt(string _filename)
    {
        maptextload = Resources.Load(_filename) as TextAsset;
        mapreadlines = maptextload.text.Split('\n');
    }

    /**
     * @brief 헥사 타일맵 생성, 타일 posX posY 설정, 시작 위치 설정
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

                // mapinfo.txt  에 start_point 코드일때 starttile 에 타일 스크립트 넣어줌
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
     * @brief 이웃타일 생성
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

            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
            {
                Debug.Log("POS " + starttile[NetworkMng.getInstance.v_user[i].startPos].transform.position);
                Vector3 tempVec = starttile[NetworkMng.getInstance.v_user[i].startPos].transform.position;
                tempVec.z = -20;

                GameMng.I.CastlePos = new Vector3(starttile[NetworkMng.getInstance.v_user[i].startPos].transform.position.x, starttile[NetworkMng.getInstance.v_user[i].startPos].transform.position.y, -20f);
                GameMng.I.CastlePosX = starttile[NetworkMng.getInstance.v_user[i].startPos].PosX;
                GameMng.I.CastlePosZ = starttile[NetworkMng.getInstance.v_user[i].startPos].PosZ;

                mainCamera.transform.position = tempVec;
                Debug.Log("POS2 " + mainCamera.transform.position);
            }
        }
    }

    /**
    * @brief 시작지점에 성이 생성이 안됐을때 기본 타일값으로 초기화
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
     * @brief 타일 값 알아오기
     * @param posX 타일 x값
     * @param posy 타일 y값
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
    * @brief 타일 코드 초기화(유닛이 이동했을 때, 건물이 부셔졌을 떄) 메모장에 code 를 가져와서 바꿔줌
    * @param posX GameMng.I.seleteTile.PoX 역활
    * @param posY GameMng.I.seleteTile.PoY 역활
    */
    public void TilecodeClear(int posX, int posY)
    {
        if (GameMng.I._hextile.GetCell(posX, posY).tileuniquecode >= (int)TILE.GRASS_START)
            GameMng.I._hextile.GetCell(posX, posY)._code = GameMng.I._hextile.GetCell(posX, posY).tileuniquecode - (int)TILE.GRASS_START;
        else
            GameMng.I._hextile.GetCell(posX, posY)._code = GameMng.I._hextile.GetCell(posX, posY).tileuniquecode;
    }

    /**
    * @brief 타일 코드 초기화(유닛이 이동했을 때, 건물이 부셔졌을 떄) 메모장에 code 를 가져와서 바꿔줌
    * @param target GameMng.I.seleteTile 역활
    */
    public void TilecodeClear(Tile target)
    {
        if (GameMng.I._hextile.GetCell(target.PosX, target.PosZ).tileuniquecode >= (int)TILE.GRASS_START)
            GameMng.I._hextile.GetCell(target.PosX, target.PosZ)._code = GameMng.I._hextile.GetCell(target.PosX, target.PosZ).tileuniquecode - (int)TILE.GRASS_START;
        else
            GameMng.I._hextile.GetCell(target.PosX, target.PosZ)._code = GameMng.I._hextile.GetCell(target.PosX, target.PosZ).tileuniquecode;
    }

    /**
     * @brief 타일간 거리 찾기
     * @param cell 선택한 타일
     */
    public void FindDistancesTo(Tile cell)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = cell.DistanceTo(cells[i]);
        }
    }
}
