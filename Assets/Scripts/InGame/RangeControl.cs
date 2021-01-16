using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeControl : MonoBehaviour
{
    [SerializeField]
    private Transform[] moveRangeTr = new Transform[6];
    [SerializeField]
    private Transform[] attackRangeTr = new Transform[6];

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
            if (GameMng.I.selectedTile.tileneighbor[i] == null)
                continue;
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
        for (int i = 0; i < moveRangeTr.Length; i++)
        {
            if (GameMng.I.selectedTile.tileneighbor[i] != null && GameMng.I.selectedTile.tileneighbor[i]._builtObj != null)
                attackRangeTr[i].transform.position = GameMng.I.selectedTile.tileneighbor[i].transform.position;
            else if (GameMng.I.selectedTile.tileneighbor[i] != null && GameMng.I.selectedTile.tileneighbor[i]._unitObj != null)
                attackRangeTr[i].transform.position = GameMng.I.selectedTile.tileneighbor[i].transform.position;
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
