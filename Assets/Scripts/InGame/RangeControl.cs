using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeControl : MonoBehaviour
{
    [SerializeField]
    private Transform[] moveRangeTr = new Transform[6];
    [SerializeField]
    private Transform[] attackRangeTr = new Transform[6];
    [SerializeField]
    private Transform selectRangeTr;

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
            if (GameMng.I.selectedTile.neighbors[i] == null)
                continue;
            if (GameMng.I.selectedTile.neighbors[i] != null && GameMng.I.selectedTile.neighbors[i]._code < (int)TILE.CAN_MOVE)
                moveRangeTr[i].transform.position = GameMng.I.selectedTile.neighbors[i].transform.position;
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
            if (GameMng.I.selectedTile.neighbors[i] != null && GameMng.I.selectedTile.neighbors[i]._builtObj != null
                && !GameMng.I.selectedTile.neighbors[i]._builtObj._uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
                attackRangeTr[i].transform.position = GameMng.I.selectedTile.neighbors[i].transform.position;
            else if (GameMng.I.selectedTile.neighbors[i] != null && GameMng.I.selectedTile.neighbors[i]._unitObj != null
                && !GameMng.I.selectedTile.neighbors[i]._unitObj._uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
                attackRangeTr[i].transform.position = GameMng.I.selectedTile.neighbors[i].transform.position;
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

    public void SelectTileSetting(bool TrSetting = false)
    {
        if (TrSetting)
            selectRangeTr.localPosition = Vector2.zero;
        else
            selectRangeTr.position = GameMng.I.selectedTile.transform.position;
    }

}
