using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeControl : MonoBehaviour
{
    [SerializeField]
    private Transform[] moveRangeTr = new Transform[6];                                                             //0: �»�, 1: ���, 2: ��, 3: ��, 4: ����, 5: ����
    [SerializeField]
    private Transform[] attackRangeTr = new Transform[6];                                                            //0: �»�, 1: ���, 2: ��, 3: ��, 4: ����, 5: ����

    private float nHeight = 0.0f;                                                                                   //�� ���� �ִ� ũ��
    private float nwidth = 0.0f;                                                                                    //�� ���� �ִ� ũ��

    void Start()
    {
        nHeight = GameMng.I.GetMapHeight - 1;
        nwidth = GameMng.I.GetMapWidth - 1;
    }


    /**
    * @brief �̵� ���� ���
    */
    public void moveRange()
    {
        if ((GameMng.I.selectedTile.PosY % 2) == 1)
        {
            if (GameMng.I.selectedTile.PosY < nHeight)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX]._code < (int)TILE.CAN_MOVE)    // �»�
                    moveRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX].transform.position;
            }
            if (GameMng.I.selectedTile.PosY < nHeight && GameMng.I.selectedTile.PosX < nwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX + 1]._code < (int)TILE.CAN_MOVE)     // ���
                    moveRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX + 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosY > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX]._code < (int)TILE.CAN_MOVE)
                    moveRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX].transform.position;
            }
            if (GameMng.I.selectedTile.PosY > 0 && GameMng.I.selectedTile.PosX < nwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX + 1]._code < (int)TILE.CAN_MOVE)
                    moveRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX + 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosX > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1]._code < (int)TILE.CAN_MOVE)
                    moveRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosX < nwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1]._code < (int)TILE.CAN_MOVE)
                    moveRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1].transform.position;
            }
        }
        else
        {
            if (GameMng.I.selectedTile.PosY > 0 && GameMng.I.selectedTile.PosX > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX - 1]._code < (int)TILE.CAN_MOVE)
                    moveRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX - 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosY > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX]._code < (int)TILE.CAN_MOVE)
                    moveRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY - 1, GameMng.I.selectedTile.PosX].transform.position;
            }
            if (GameMng.I.selectedTile.PosX > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX - 1]._code < (int)TILE.CAN_MOVE)
                    moveRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX - 1].transform.position;
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1]._code < (int)TILE.CAN_MOVE)
                    moveRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX - 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosX <= nwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX]._code < (int)TILE.CAN_MOVE)
                    moveRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX].transform.position;
            }
            if (GameMng.I.selectedTile.PosX < nwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1]._code < (int)TILE.CAN_MOVE)
                    moveRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY, GameMng.I.selectedTile.PosX + 1].transform.position;
            }
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
        if ((GameMng.I.targetTile.PosY % 2) == 1)
        {
            if (GameMng.I.targetTile.PosY < nHeight)
            {
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY + 1, GameMng.I.targetTile.PosX]._unitObj != null)
                    attackRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY + 1, GameMng.I.targetTile.PosX].transform.position;
            }
            if (GameMng.I.targetTile.PosY < nHeight && GameMng.I.targetTile.PosX < nwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY + 1, GameMng.I.targetTile.PosX + 1]._unitObj != null)
                    attackRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY + 1, GameMng.I.targetTile.PosX + 1].transform.position;
            }
            if (GameMng.I.targetTile.PosY > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY - 1, GameMng.I.targetTile.PosX]._unitObj != null)
                    attackRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY - 1, GameMng.I.targetTile.PosX].transform.position;
            }
            if (GameMng.I.targetTile.PosY > 0 && GameMng.I.targetTile.PosX < nwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY - 1, GameMng.I.targetTile.PosX + 1]._unitObj != null)
                    attackRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY - 1, GameMng.I.targetTile.PosX + 1].transform.position;
            }
            if (GameMng.I.targetTile.PosX > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY, GameMng.I.targetTile.PosX - 1]._unitObj != null)
                    attackRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY, GameMng.I.targetTile.PosX - 1].transform.position;
            }
            if (GameMng.I.targetTile.PosX < nwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY, GameMng.I.targetTile.PosX + 1]._unitObj != null)
                    attackRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY, GameMng.I.targetTile.PosX + 1].transform.position;
            }
        }
        else
        {
            if (GameMng.I.targetTile.PosY > 0 && GameMng.I.targetTile.PosX > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY - 1, GameMng.I.targetTile.PosX - 1]._unitObj != null)
                    attackRangeTr[4].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY - 1, GameMng.I.targetTile.PosX - 1].transform.position;
            }
            if (GameMng.I.targetTile.PosY > 0)
            {
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY - 1, GameMng.I.targetTile.PosX]._unitObj != null)
                    attackRangeTr[5].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY - 1, GameMng.I.targetTile.PosX].transform.position;
            }
            if (GameMng.I.targetTile.PosX > 0)                                                                                                                             //���ݹ����� �Ѱ��� �ȳ����� �� �� ��
            {
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY + 1, GameMng.I.targetTile.PosX]._unitObj != null)
                    attackRangeTr[0].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY + 1, GameMng.I.targetTile.PosX].transform.position;
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY, GameMng.I.targetTile.PosX - 1]._unitObj != null)
                    attackRangeTr[2].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY, GameMng.I.targetTile.PosX - 1].transform.position;
            }
            if (GameMng.I.selectedTile.PosX <= nwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX]._unitObj != null)
                    attackRangeTr[1].transform.position = GameMng.I.mapTile[GameMng.I.selectedTile.PosY + 1, GameMng.I.selectedTile.PosX].transform.position;
            }
            if (GameMng.I.targetTile.PosX < nwidth)
            {
                if (GameMng.I.mapTile[GameMng.I.targetTile.PosY, GameMng.I.targetTile.PosX + 1]._unitObj != null)
                    attackRangeTr[3].transform.position = GameMng.I.mapTile[GameMng.I.targetTile.PosY, GameMng.I.targetTile.PosX + 1].transform.position;
            }
        }
    }
    /**
* @brief ���� ���� Ÿ�� ��ġ ����
*/
    public void AttackrangeTileReset()
    {
        for (int i = 0; i < attackRangeTr.Length; i++)
        {
            attackRangeTr[i].transform.localPosition = Vector2.zero;                           //����Ÿ�� ��ġ �ʱ�ȭ
        }
    }
}
