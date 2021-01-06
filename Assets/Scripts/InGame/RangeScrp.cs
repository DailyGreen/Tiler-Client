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

    private float fHeight = 0.0f;                                                                                   //맵 세로 최대 크기
    private float fwidth = 0.0f;                                                                                    //맵 가로 최대 크기

    // Start is called before the first frame update
    void Start()
    {
        fHeight = 19.0f;
        fwidth = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveRange();
        //AttackRange();
    }

    /**
    * @brief 이동 범위 계산
    */
    public void MoveRange()
    {
        //일반적인 타일들
        if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY + 1) <= fHeight)                     //좌상
        {
            MoveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY + 1].transform.position;
        }
        if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY - 1) >= 0)                     //좌하
        {
            MoveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY - 1].transform.position;
        }
        if ((GameMng.I.NowTile.PosX + 1) <= fwidth && (GameMng.I.NowTile.PosY + 1) <= fHeight)                     //우상
        {
            MoveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY + 1].transform.position;
        }
        if ((GameMng.I.NowTile.PosX + 1) <= fwidth && (GameMng.I.NowTile.PosY - 1) >= 0)                     //우하
        {
            MoveRangeTr[5].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY - 1].transform.position;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if ((GameMng.I.NowTile.PosY % 2) == 1)                                                                             //타일의 PosY가 홀수일때
        {
            if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY + 1) <= fHeight)                     //좌상
            {
                MoveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
            }
            if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY - 1) >= 0)                     //좌하
            {
                MoveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1].transform.position;
            }
            if (GameMng.I.NowTile.PosX == 0)
            {
                if (GameMng.I.NowTile.PosY != fHeight)
                    MoveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
                MoveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1].transform.position;
            }

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        else                                                                                                              //타일의 PosY가 짝수일때
        {
            if ((GameMng.I.NowTile.PosX + 1) <= fwidth && (GameMng.I.NowTile.PosY + 1) <= fHeight)                     //우상
            {
                MoveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
            }
            if ((GameMng.I.NowTile.PosX + 1) <= fwidth && (GameMng.I.NowTile.PosY - 1) >= 0)                     //우하
            {
                MoveRangeTr[5].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1].transform.position;
            }
            if (GameMng.I.NowTile.PosX == fwidth)
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
        if ((GameMng.I.NowTile.PosX + 1) <= fwidth)                                                        //우
        {
            MoveRangeTr[3].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY].transform.position;
        }
    }
    /**
* @brief 범위 타일 위치 리셋
*/
    public void RangeTileReset()
    {
        for (int i = 0; i < MoveRangeTr.Length; i++)
        {
            MoveRangeTr[i].transform.localPosition = Vector2.zero;                           //범위타일 위치 초기화
        }
    }

    /**
* @brief 공격 범위 계산
*/
    public void AttackRange()
    {
        if ((GameMng.I.NowTile.PosY % 2) == 1)
        {
            if (GameMng.I.NowTile.PosY < fHeight)
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1]._unitObj != null)
                    AttackRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
            }
            if (GameMng.I.NowTile.PosY < fHeight && GameMng.I.NowTile.PosX < fwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY + 1]._unitObj != null)
                    AttackRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY + 1].transform.position;
            }
            if (GameMng.I.NowTile.PosY > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1]._unitObj != null)
                    AttackRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1].transform.position;
            }
            if (GameMng.I.NowTile.PosY > 0 && GameMng.I.NowTile.PosX < fwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY - 1]._unitObj != null)
                    AttackRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY - 1].transform.position;
            }
            if (GameMng.I.NowTile.PosX > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY]._unitObj != null)
                    AttackRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY].transform.position;
            }
            if (GameMng.I.NowTile.PosX < fwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY]._unitObj != null)
                    AttackRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY].transform.position;
            }
        }
        else
        {
            if (GameMng.I.NowTile.PosY > 0 && GameMng.I.NowTile.PosX > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY - 1]._unitObj != null)
                    AttackRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY - 1].transform.position;
            }
            if (GameMng.I.NowTile.PosY > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1]._unitObj != null)
                    AttackRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY - 1].transform.position;
            }
            if (GameMng.I.NowTile.PosX > 0)                                                                                                                             //공격범위가 한곳이 안나오면 이 잘 못
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY + 1]._unitObj != null)
                    AttackRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY + 1].transform.position;
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY]._unitObj != null)
                    AttackRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY].transform.position;
            }
            if (GameMng.I.NowTile.PosX <= fwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1]._unitObj != null)
                    AttackRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
            }
            if (GameMng.I.NowTile.PosX < fwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY]._unitObj != null)
                    AttackRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY].transform.position;
            }
        }
    }

}
