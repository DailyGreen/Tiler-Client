using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Tile : Object
{
    [SerializeField]
    private int posX, posY, posZ;
    [SerializeField]
    private int distance;
    public int tileuniquecode = 0;

    GameObject tile;
    [SerializeField]
    Sprite[] tileSprite;
    [SerializeField]
    SpriteRenderer tileSpriteRend;

    public Unit _unitObj;
    public Built _builtObj;
    public Tile[] neighbors = new Tile[6];
    void Start()
    {
        tile = this.GetComponent<GameObject>();
        if (!this._code.Equals((int)BUILT.CASTLE))
        {
            if (this._code >= (int)TILE.GRASS_START) { this.tileSpriteRend.sprite = tileSprite[this._code - (int)TILE.GRASS_START]; }
            else if (this._code > (int)TILE.CAN_MOVE && this._code < (int)BUILT.CASTLE) { this.tileSpriteRend.sprite = tileSprite[this._code - 1]; }
            else if (this._code < (int)TILE.CAN_MOVE) { this.tileSpriteRend.sprite = tileSprite[this._code]; }
        }
        _name = "독도는";
        _desc = "우리땅";
    }

    /**
     * @brief 타일의 posX,posY,posZ값  설정 또는 값 알아오기
     */
    public int PosX
    {
        get
        {
            return posX;
        }
        set
        {
            posX = value;
        }
    }
    public int PosY
    {
        get
        {
            return -posX - posZ;
        }
        set
        {
            posY = value;
        }
    }
    public int PosZ
    {
        get
        {
            return posZ;
        }
        set
        {
            posZ = value;
        }
    }
    public int Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
        }
    }
    /**
     * @brief 타일이 어디있는지 Vec2 알아오기
     */
    public Vector2 GetTileVec2
    {
        get
        {
            return new Vector2(this.transform.position.x, this.transform.position.y);
        }
    }

    /**
     * @brief 타일이 비었는지 확인
     * @param t 현제 타일
     */
    public static bool isEmptyTile(Tile t)
    {
        if (t._unitObj == null && t._builtObj == null && t._code < (int)TILE.CAN_MOVE)
            return true;
        return false;
    }

    /**
     * @brief 타일 이웃 설정
     * @param direction 방향
     * @param cell 이웃타일
     */
    public void SetNeighbor(DIRECTION direction, Tile cell)
    {
        neighbors[(int)direction] = cell;
        DIRECTION directiontemp = (int)direction < 3 ? (direction + 3) : (direction - 3);
        cell.neighbors[(int)directiontemp] = this;
    }

    /**
    * @brief 타일간 거리 계산하기
    * @param other 다른 타일
    */
    public int DistanceTo(Tile other)
    {
        return
            ((PosX < other.PosX ? other.PosX - PosX : PosX - other.PosX) +
            (PosY < other.PosY ? other.PosY - PosY : PosY - other.PosY) +
            (PosZ < other.PosZ ? other.PosZ - PosZ : PosZ - other.PosZ)) / 2;

    }
}
