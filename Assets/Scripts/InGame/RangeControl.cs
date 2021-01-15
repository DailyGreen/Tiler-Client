using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeControl : MonoBehaviour
{
    [SerializeField]
    private Transform[] moveRangeTr = new Transform[6];                                                             //0: 좌상, 1: 우상, 2: 좌, 3: 우, 4: 좌하, 5: 우하
    [SerializeField]
    private Transform[] attackRangeTr = new Transform[6];                                                            //0: 좌상, 1: 우상, 2: 좌, 3: 우, 4: 좌하, 5: 우하

    private float nHeight = 0.0f;                                                                                   //맵 세로 최대 크기
    private float nwidth = 0.0f;                                                                                    //맵 가로 최대 크기

    void Start()
    {
        nHeight = GameMng.I.GetMapHeight - 1;
        nwidth = GameMng.I.GetMapWidth - 1;
    }

    /**
   * @brief 이동 범위 계산
   */
    public void moveRange()
    {
        for (int i = 0; i < moveRangeTr.Length; i++)
        {
            if (GameMng.I.selectedTile.tileneighbor[i] != null && GameMng.I.selectedTile.tileneighbor[i]._code < (int)TILE.CAN_MOVE)
                moveRangeTr[i].transform.position = GameMng.I.selectedTile.tileneighbor[i].transform.position;
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
        if (GameMng.I.selectedTile._builtObj != null)   // 범위 안에 건물이 있을시
        {
            if ((GameMng.I.selectedTile.PosY % 2) == 1)
            {
                if (GameMng.I.selectedTile.PosY < nHeight)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX]._unitObj != null)
                        attackRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX].transform.position;
                }
                if (GameMng.I.selectedTile.PosY < nHeight && GameMng.I.selectedTile.PosX < nwidth)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX + 1]._unitObj != null)
                        attackRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX + 1].transform.position;
                }
                if (GameMng.I.selectedTile.PosY > 0)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX]._unitObj != null)
                        attackRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX].transform.position;
                }
                if (GameMng.I.selectedTile.PosY > 0 && GameMng.I.selectedTile.PosX < nwidth)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX + 1]._unitObj != null)
                        attackRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX + 1].transform.position;
                }
                if (GameMng.I.selectedTile.PosX > 0)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1]._unitObj != null)
                        attackRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1].transform.position;
                }
                if (GameMng.I.selectedTile.PosX < nwidth)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1]._unitObj != null)
                        attackRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1].transform.position;
                }
            }
            else
            {
                if (GameMng.I.selectedTile.PosY > 0 && GameMng.I.selectedTile.PosX > 0)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX - 1]._unitObj != null)
                        attackRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX - 1].transform.position;
                }
                if (GameMng.I.selectedTile.PosY > 0)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX]._unitObj != null)
                        attackRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX].transform.position;
                }
                if (GameMng.I.selectedTile.PosX > 0)                                                                                                                             //공격범위가 한곳이 안나오면 이 잘 못
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX - 1]._unitObj != null)
                        attackRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX - 1].transform.position;
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1]._unitObj != null)
                        attackRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1].transform.position;
                }
                if (GameMng.I.selectedTile.PosX <= nwidth)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX]._unitObj != null)
                        attackRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX].transform.position;
                }
                if (GameMng.I.selectedTile.PosX < nwidth)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1]._unitObj != null)
                        attackRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1].transform.position;
                }
            }
        }
        else if (GameMng.I.selectedTile._unitObj != null)   // 범위 안에 유닛이 있을 시
        {
            if ((GameMng.I.selectedTile.PosY % 2) == 1)
            {
                if (GameMng.I.selectedTile.PosY < nHeight)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX]._builtObj != null)
                        attackRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX].transform.position;
                }
                if (GameMng.I.selectedTile.PosY < nHeight && GameMng.I.selectedTile.PosX < nwidth)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX + 1]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX + 1]._builtObj != null)
                        attackRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX + 1].transform.position;
                }
                if (GameMng.I.selectedTile.PosY > 0)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX]._builtObj != null)
                        attackRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX].transform.position;
                }
                if (GameMng.I.selectedTile.PosY > 0 && GameMng.I.selectedTile.PosX < nwidth)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX + 1]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX + 1]._builtObj != null)
                        attackRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX + 1].transform.position;
                }
                if (GameMng.I.selectedTile.PosX > 0)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1]._builtObj != null)
                        attackRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1].transform.position;
                }
                if (GameMng.I.selectedTile.PosX < nwidth)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1]._builtObj != null)
                        attackRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1].transform.position;
                }
            }
            else
            {
                if (GameMng.I.selectedTile.PosY > 0 && GameMng.I.selectedTile.PosX > 0)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX - 1]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX - 1]._builtObj != null)
                        attackRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX - 1].transform.position;
                }
                if (GameMng.I.selectedTile.PosY > 0)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX]._builtObj != null)
                        attackRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX].transform.position;
                }
                if (GameMng.I.selectedTile.PosX > 0)                                                                                                                             //공격범위가 한곳이 안나오면 이 잘 못
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX - 1]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX - 1]._builtObj != null)
                        attackRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX - 1].transform.position;
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1]._builtObj != null)
                        attackRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1].transform.position;
                }
                if (GameMng.I.selectedTile.PosX <= nwidth)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX]._builtObj != null)
                        attackRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX].transform.position;
                }
                if (GameMng.I.selectedTile.PosX < nwidth)
                {
                    if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1]._unitObj != null
                        || GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1]._builtObj != null)
                        attackRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1].transform.position;
                }
            }
        }
    }
    /**
    * @brief 공격 범위 타일 위치 리셋
    */
    public void AttackrangeTileReset()
    {
        for (int i = 0; i < attackRangeTr.Length; i++)
        {
            attackRangeTr[i].transform.localPosition = Vector2.zero;                           //범위타일 위치 초기화
        }
    }
}
