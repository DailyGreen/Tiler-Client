using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField]
    private int posX, posY;
    [SerializeField]
    private char code;

    private string _name = "";       // 오브젝트 고유 명칭
    private string _desc = "";       // 오브젝트 설명

   //@brief 타일의 posX,posY값 설정 또는 값 알아오기
    
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


     // @brief 타일의 코드 설정 또는 값 알아오기
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

    
    // @brief 타일의 이름 설정 또는 값 알아오기
    
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

    
     // @brief 타일의 설명 설정 또는 값 알아오기
     
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
     * @brief 타일이 어디있는지 Vec2 알아오기
     */
    public Vector2 GetTileVec2
    {
        get
        {
            return new Vector2(this.transform.localPosition.x, this.transform.localPosition.y);
        }
    }
}