using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeControl : MonoBehaviour
{
    [SerializeField]
    private Transform[] moveRangeTr = new Transform[6];                                                             //0: �»�, 1: ���, 2: ��, 3: ��, 4: ����, 5: ����
    [SerializeField]
    private Transform[] attackRangeTr = new Transform[6];                                                            //0: �»�, 1: ���, 2: ��, 3: ��, 4: ����, 5: ����

    private float fHeight = 0.0f;                                                                                   //�� ���� �ִ� ũ��
    private float fwidth = 0.0f;                                                                                    //�� ���� �ִ� ũ��

    void Start()
    {
        fHeight = 19.0f;
        fwidth = 4.0f;
    }


    /**
    * @brief �̵� ���� ���
    */
    public void moveRange()
    {
        //�Ϲ����� Ÿ�ϵ�
        if ((GameMng.I.selectedTile.PosX - 1) >= 0 && (GameMng.I.selectedTile.PosY + 1) <= fHeight)                     //�»�
        {
            moveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY + 1].transform.position;
        }
        if ((GameMng.I.selectedTile.PosX - 1) >= 0 && (GameMng.I.selectedTile.PosY - 1) >= 0)                     //����
        {
            moveRangeTr[4].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY - 1].transform.position;
        }
        if ((GameMng.I.selectedTile.PosX + 1) <= fwidth && (GameMng.I.selectedTile.PosY + 1) <= fHeight)                     //���
        {
            moveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY + 1].transform.position;
        }
        if ((GameMng.I.selectedTile.PosX + 1) <= fwidth && (GameMng.I.selectedTile.PosY - 1) >= 0)                     //����
        {
            moveRangeTr[5].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY - 1].transform.position;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if ((GameMng.I.selectedTile.PosY % 2) == 1)                                                                             //Ÿ���� PosY�� Ȧ���϶�
        {
            if ((GameMng.I.selectedTile.PosX - 1) >= 0 && (GameMng.I.selectedTile.PosY + 1) <= fHeight)                     //�»�
            {
                moveRangeTr[0].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY + 1].transform.position;
            }
            if ((GameMng.I.selectedTile.PosX - 1) >= 0 && (GameMng.I.selectedTile.PosY - 1) >= 0)                     //����
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
        else                                                                                                              //Ÿ���� PosY�� ¦���϶�
        {
            if ((GameMng.I.selectedTile.PosX + 1) <= fwidth && (GameMng.I.selectedTile.PosY + 1) <= fHeight)                     //���
            {
                moveRangeTr[1].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY + 1].transform.position;
            }
            if ((GameMng.I.selectedTile.PosX + 1) <= fwidth && (GameMng.I.selectedTile.PosY - 1) >= 0)                     //����
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
        if ((GameMng.I.selectedTile.PosX - 1) >= 0)                                                         //��
        {
            moveRangeTr[2].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX - 1, GameMng.I.selectedTile.PosY].transform.position;
        }
        if ((GameMng.I.selectedTile.PosX + 1) <= fwidth)                                                        //��
        {
            moveRangeTr[3].position = GameMng.I.mapTile[GameMng.I.selectedTile.PosX + 1, GameMng.I.selectedTile.PosY].transform.position;
        }
    }
    /**
* @brief ���� Ÿ�� ��ġ ����
*/
    public void rangeTileReset()
    {
        for (int i = 0; i < moveRangeTr.Length; i++)
        {
            moveRangeTr[i].transform.localPosition = Vector2.zero;                           //����Ÿ�� ��ġ �ʱ�ȭ
        }
    }

    /**
* @brief ���� ���� ���
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
            if (GameMng.I.selectedTile.PosX > 0)                                                                                                                             //���ݹ����� �Ѱ��� �ȳ����� �� �� ��
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
