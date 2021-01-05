using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScrp : MonoBehaviour
{
    public Unit UnitSc = null;

    [SerializeField]
    private Transform[] MoveRangeTr = new Transform[6];                                                             //0: 좌상, 1: 우상, 2: 좌, 3: 우, 4: 좌하, 5: 우하
    [SerializeField]
    private Transform[] AttackRangeTr = new Transform[6];                                                            //0: 좌상, 1: 우상, 2: 좌, 3: 우, 4: 좌하, 5: 우하

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveRange();
        AttackRange();
    }

    /**
    * @brief 이동 범위 계산
    */
    void MoveRange()
    {
        //일반적인 타일들
        if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY + 1) <= 19)                     //좌상
        {
            MoveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY + 1].transform.position;
        }
        if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY - 1) >= 0)                     //좌하
        {
            MoveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY - 1].transform.position;
        }
        if ((GameMng.I.NowTile.PosX + 1) <= 4 && (GameMng.I.NowTile.PosY + 1) <= 19)                     //우상
        {
            MoveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY + 1].transform.position;
        }
        if ((GameMng.I.NowTile.PosX + 1) <= 4 && (GameMng.I.NowTile.PosY - 1) >= 0)                     //우하
        {
            MoveRangeTr[5].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY - 1].transform.position;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if ((GameMng.I.NowTile.PosY % 2) == 1)                                                                             //타일의 PosY가 홀수일때
        {
            if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY + 1) <= 19)                     //좌상
            {
                MoveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
            }
            if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY - 1) >= 0)                     //좌하
            {
                MoveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1].transform.position;
            }
            if (GameMng.I.NowTile.PosX == 0)
            {
                if (GameMng.I.NowTile.PosY != 19)
                    MoveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
                MoveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1].transform.position;
            }

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        else                                                                                                              //타일의 PosY가 짝수일때
        {
            if ((GameMng.I.NowTile.PosX + 1) <= 4 && (GameMng.I.NowTile.PosY + 1) <= 19)                     //우상
            {
                MoveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
            }
            if ((GameMng.I.NowTile.PosX + 1) <= 4 && (GameMng.I.NowTile.PosY - 1) >= 0)                     //우하
            {
                MoveRangeTr[5].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1].transform.position;
            }
            if (GameMng.I.NowTile.PosX == 4)
            {
                MoveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
                if (GameMng.I.NowTile.PosY != 0)
                    MoveRangeTr[5].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1].transform.position;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if ((GameMng.I.NowTile.PosX - 1) >= 0)                                                         //좌
        {
            MoveRangeTr[2].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY].transform.position;
        }
        if ((GameMng.I.NowTile.PosX + 1) <= 4)                                                        //우
        {
            MoveRangeTr[3].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY].transform.position;
        }

        if (!GameMng.I.bUnitMoveCheck)                                                                   //이동이 종료되면
        {
            for (int i = 0; i < MoveRangeTr.Length; i++)
            {
                MoveRangeTr[i].transform.localPosition = Vector2.zero;                           //범위타일 위치 초기화
            }
        }
    }
    /**
* @brief 공격 범위 계산
*/
    void AttackRange()
    {
        
    }

}
