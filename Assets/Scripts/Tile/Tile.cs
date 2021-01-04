using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField]
    private int posX, posY;
    [SerializeField]
    private char code;

    private string _name = "";       // ������Ʈ ���� ��Ī
    private string _desc = "";       // ������Ʈ ����

   //@brief Ÿ���� posX,posY�� ���� �Ǵ� �� �˾ƿ���
    
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


     // @brief Ÿ���� �ڵ� ���� �Ǵ� �� �˾ƿ���
    public char Code
    {
        get
        {
            return code;
        }
        set
        {
            code = value;
        }
    }

    
    // @brief Ÿ���� �̸� ���� �Ǵ� �� �˾ƿ���
    
    public string TileName
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    
     // @brief Ÿ���� ���� ���� �Ǵ� �� �˾ƿ���
     
    public string TileDesc
    {
        get
        {
            return _desc;
        }
        set
        {
            _desc = value;
        }
    }

    /**
     * @brief Ÿ���� ����ִ��� Vec2 �˾ƿ���
     */
    public Vector2 GetTileVec2
    {
        get
        {
            return new Vector2(this.transform.localPosition.x, this.transform.localPosition.y);
        }
    }
}