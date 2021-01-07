using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeControl : MonoBehaviour
{
    [SerializeField]
    private Transform[] moveRangeTr = new Transform[6];                                                             //0: 좌상, 1: 우상, 2: 좌, 3: 우, 4: 좌하, 5: 우하
    [SerializeField]
    private Transform[] attackRangeTr = new Transform[6];                                                            //0: 좌상, 1: 우상, 2: 좌, 3: 우, 4: 좌하, 5: 우하

    private float fHeight = 0.0f;                                                                                   //맵 세로 최대 크기
    private float fwidth = 0.0f;                                                                                    //맵 가로 최대 크기

    void Start()
    {
        fHeight = 19.0f;
        fwidth = 4.0f;
    }


    /**
    * @brief 이동 범위 계산
    */
    public void moveRange()
    {
        //일반적인 타일들
        if ((GameMng.I.selectedTile.PosX - 1) >= 0 && (GameMng.I.selectedTile.PosY + 1) <= fHeight)                     //좌상
        {
            moveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY + 1].transform.position;
        }
        if ((GameMng.I.selectedTile.PosX - 1) >= 0 && (GameMng.I.selectedTile.PosY - 1) >= 0)                     //좌하
        {
            moveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY - 1].transform.position;
        }
        if ((GameMng.I.selectedTile.PosX + 1) <= fwidth && (GameMng.I.selectedTile.PosY + 1) <= fHeight)                     //우상
        {
            moveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY + 1].transform.position;
        }
        if ((GameMng.I.selectedTile.PosX + 1) <= fwidth && (GameMng.I.selectedTile.PosY - 1) >= 0)                     //우하
        {
            moveRangeTr[5].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY - 1].transform.position;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if ((GameMng.I.selectedTile.PosY % 2) == 1)                                                                             //타일의 PosY가 홀수일때
        {
            if ((GameMng.I.selectedTile.PosX - 1) >= 0 && (GameMng.I.selectedTile.PosY + 1) <= fHeight)                     //좌상
            {
                moveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY + 1].transform.position;
            }
            if ((GameMng.I.selectedTile.PosX - 1) >= 0 && (GameMng.I.selectedTile.PosY - 1) >= 0)                     //좌하
            {
                moveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY - 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosX == 0)
            {
                if (GameMng.I.selectedTile.PosY != fHeight)
                    moveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY + 1].transform.position;
                moveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY - 1].transform.position;
            }

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        else                                                                                                              //타일의 PosY가 짝수일때
        {
            if ((GameMng.I.selectedTile.PosX + 1) <= fwidth && (GameMng.I.selectedTile.PosY + 1) <= fHeight)                     //우상
            {
                moveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY + 1].transform.position;
            }
            if ((GameMng.I.selectedTile.PosX + 1) <= fwidth && (GameMng.I.selectedTile.PosY - 1) >= 0)                     //우하
            {
                moveRangeTr[5].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY - 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosX == fwidth)
            {
                moveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY + 1].transform.position;
                if (GameMng.I.selectedTile.PosY != 0)
                    moveRangeTr[5].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY - 1].transform.position;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if ((GameMng.I.selectedTile.PosX - 1) >= 0)                                                         //좌
        {
            moveRangeTr[2].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY].transform.position;
        }
        if ((GameMng.I.selectedTile.PosX + 1) <= fwidth)                                                        //우
        {
            moveRangeTr[3].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY].transform.position;
        }
    }
    /**
* @brief 범위 타일 위치 리셋
*/
    public void rangeTileReset()
    {
        for (int i = 0; i < moveRangeTr.Length; i++)
        {
            moveRangeTr[i].transform.localPosition = Vector2.zero;                           //범위타일 위치 초기화
        }
    }

    /**
* @brief 공격 범위 계산
*/
    public void attackRange()
    {
        if ((GameMng.I.selectedTile.PosY % 2) == 1)
        {
            if (GameMng.I.selectedTile.PosY < fHeight)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY + 1]._unitObj != null)
                    attackRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY + 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosY < fHeight && GameMng.I.selectedTile.PosX < fwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY + 1]._unitObj != null)
                    attackRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY + 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosY > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY - 1]._unitObj != null)
                    attackRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY - 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosY > 0 && GameMng.I.selectedTile.PosX < fwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY - 1]._unitObj != null)
                    attackRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY - 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosX > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY]._unitObj != null)
                    attackRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY].transform.position;
            }
            if (GameMng.I.selectedTile.PosX < fwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY]._unitObj != null)
                    attackRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY].transform.position;
            }
        }
        else
        {
            if (GameMng.I.selectedTile.PosY > 0 && GameMng.I.selectedTile.PosX > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY - 1]._unitObj != null)
                    attackRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY - 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosY > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY - 1]._unitObj != null)
                    attackRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY - 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosX > 0)                                                                                                                             //공격범위가 한곳이 안나오면 이 잘 못
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY + 1]._unitObj != null)
                    attackRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY + 1].transform.position;
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY]._unitObj != null)
                    attackRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY].transform.position;
            }
            if (GameMng.I.selectedTile.PosX <= fwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY + 1]._unitObj != null)
                    attackRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY + 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosX < fwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY]._unitObj != null)
                    attackRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY].transform.position;
            }
        }
    }

}
