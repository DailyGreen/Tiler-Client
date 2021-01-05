using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Tile : Object
{
    [SerializeField]
    private int posX, posY;

    GameObject tile;
    [SerializeField]
    Sprite[] tileSprite;
    [SerializeField]
    SpriteRenderer tileSpriteRend;

    public Unit _unitObj;
    public Built _builtObj;

    void Start()
    {
        tile = this.GetComponent<GameObject>();
        this.tileSpriteRend.sprite = tileSprite[this._code];
        _name = "������";
        _desc = "�츮��";
    }

    /**
     * @brief Ÿ���� posX,posY��  ���� �Ǵ� �� �˾ƿ���
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
            return posY;
        }
        set
        {
            posY = value;
        }
    }

    /**
     * @brief Ÿ���� ����ִ��� Vec2 �˾ƿ���
     */
    public Vector2 GetTileVec2
    {
        get
        {
            return new Vector2(this.transform.position.x, this.transform.position.y);
        }
    }
}
