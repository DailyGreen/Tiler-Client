using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Built : Object
{
    /*
     * 건물 고유  스탯
     * 반드시 상속받는 자식의 Start 에서 값을 변경해 주시길 바랍니다.
     */
    public int _hp = 0;

    public RaycastHit2D hit;

    /**
     * @brief 건물을 생성
     * @param 생성할 child 게임오브젝트
     */
    public void Building(GameObject built)
    {
        hit = GameMng.I.MouseLaycast();
        if (GameMng.I.GetTileCs._builtObj == null)
        {
           
            {
                Instantiate(built, GameMng.I.GetTileCs.transform);
                GameMng.I.GetTileCs._builtObj = this;
            }
        }
    }
}