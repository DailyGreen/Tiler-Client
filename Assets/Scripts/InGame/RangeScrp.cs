using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScrp : MonoBehaviour
{
    public Unit UnitSc = null;

    [SerializeField]
    private Transform[] MoveRangeTr = new Transform[6];                                                             //0: �»�, 1: ���, 2: ��, 3: ��, 4: ����, 5: ����
    [SerializeField]
    private Transform[] AttackRangeTr = new Transform[6];                                                            //0: �»�, 1: ���, 2: ��, 3: ��, 4: ����, 5: ����

    private float fHeight = 0.0f;                                                                                   //�� ���� �ִ� ũ��
    private float fwidth = 0.0f;                                                                                    //�� ���� �ִ� ũ��

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
    * @brief �̵� ���� ���
    */
    public void MoveRange()
    {
        //�Ϲ����� Ÿ�ϵ�
        if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY + 1) <= fHeight)                     //�»�
        {
            MoveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY + 1].transform.position;
        }
        if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY - 1) >= 0)                     //����
        {
            MoveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY - 1].transform.position;
        }
        if ((GameMng.I.NowTile.PosX + 1) <= fwidth && (GameMng.I.NowTile.PosY + 1) <= fHeight)                     //���
        {
            MoveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY + 1].transform.position;
        }
        if ((GameMng.I.NowTile.PosX + 1) <= fwidth && (GameMng.I.NowTile.PosY - 1) >= 0)                     //����
        {
            MoveRangeTr[5].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY - 1].transform.position;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if ((GameMng.I.NowTile.PosY % 2) == 1)                                                                             //Ÿ���� PosY�� Ȧ���϶�
        {
            if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY + 1) <= fHeight)                     //�»�
            {
                MoveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
            }
            if ((GameMng.I.NowTile.PosX - 1) >= 0 && (GameMng.I.NowTile.PosY - 1) >= 0)                     //����
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
        else                                                                                                              //Ÿ���� PosY�� ¦���϶�
        {
            if ((GameMng.I.NowTile.PosX + 1) <= fwidth && (GameMng.I.NowTile.PosY + 1) <= fHeight)                     //���
            {
                MoveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX, GameMng.I.NowTile.PosY + 1].transform.position;
            }
            if ((GameMng.I.NowTile.PosX + 1) <= fwidth && (GameMng.I.NowTile.PosY - 1) >= 0)                     //����
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
        if ((GameMng.I.NowTile.PosX - 1) >= 0)                                                         //��
        {
            MoveRangeTr[2].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX - 1, GameMng.I.NowTile.PosY].transform.position;
        }
        if ((GameMng.I.NowTile.PosX + 1) <= fwidth)                                                        //��
        {
            MoveRangeTr[3].position = GameMng.I.mapTile[GameMng.I.NowTile.PosX + 1, GameMng.I.NowTile.PosY].transform.position;
        }
    }
    /**
* @brief ���� Ÿ�� ��ġ ����
*/
    public void RangeTileReset()
    {
        for (int i = 0; i < MoveRangeTr.Length; i++)
        {
            MoveRangeTr[i].transform.localPosition = Vector2.zero;                           //����Ÿ�� ��ġ �ʱ�ȭ
        }
    }

    /**
* @brief ���� ���� ���
*/
    void AttackRange()
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
            if (GameMng.I.NowTile.PosX > 0)                                                                                                                             //���ݹ����� �Ѱ��� �ȳ����� �� �� ��
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
