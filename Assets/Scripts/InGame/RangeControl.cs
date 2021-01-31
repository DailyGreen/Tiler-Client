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

    /**
     * @brief �̵� ���� ���
     * @param distance �̵� ����
     */
    public void moveRange(int distance)
    {
        int count = 0;
        for (int i = 0; i < GameMng.I._hextile.cells.Length; i++)
        {
            if (count >= moveRangeTr.Length) break;

            if (GameMng.I._hextile.cells[i].Distance <= distance && !GameMng.I._hextile.cells[i].Distance.Equals(0) && GameMng.I._hextile.cells[i]._code < (int)TILE.CAN_MOVE)
            {
                moveRangeTr[count].transform.position = GameMng.I._hextile.cells[i].transform.position;
                count++;
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
            // ����Ÿ�� ��ġ �ʱ�ȭ
            moveRangeTr[i].transform.localPosition = Vector2.zero;
        }
    }

    /**
     * @brief ���� ���� ���
     * @param distance ���� ����
     */
    public void attackRange(int distance)
    {
        int count = 0;
        for (int i = 0; i < GameMng.I._hextile.cells.Length; i++)
        {
            if (count >= attackRangeTr.Length) { break; }

            if (GameMng.I._hextile.cells[i].Distance <= distance && !GameMng.I._hextile.cells[i].Distance.Equals(0))
            {
                if (GameMng.I.selectedTile._code.Equals((int)BUILT.ATTACK_BUILDING))
                {
                    attackRangeTr[count].transform.position = GameMng.I._hextile.cells[i].transform.position;
                    count++;
                }
                else if (GameMng.I._hextile.cells[i]._code < (int)TILE.CAN_MOVE && GameMng.I._hextile.cells[i]._builtObj == null && GameMng.I._hextile.cells[i]._unitObj == null)
                {
                    attackRangeTr[count].transform.position = GameMng.I._hextile.cells[i].transform.position;
                    count++;
                }
                else if (GameMng.I._hextile.cells[i]._unitObj != null && !GameMng.I._hextile.cells[i]._unitObj._uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
                {
                    attackRangeTr[count].transform.position = GameMng.I._hextile.cells[i].transform.position;
                    count++;
                }
                else if (GameMng.I._hextile.cells[i]._builtObj != null && !GameMng.I._hextile.cells[i]._builtObj._uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
                {
                    attackRangeTr[count].transform.position = GameMng.I._hextile.cells[i].transform.position;
                    count++;
                }
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
            // ����Ÿ�� ��ġ �ʱ�ȭ
            attackRangeTr[i].transform.localPosition = Vector2.zero;
        }
    }

    /**
     * @brief ������ Ÿ�Ϸ� ��ġ ����
     * @param TrSetting �ڱⰡ ������ Ÿ���� ����
     */
    public void SelectTileSetting(bool TrSetting = false)
    {
        if (TrSetting)
            selectRangeTr.localPosition = Vector2.zero;
        else
            selectRangeTr.position = GameMng.I.selectedTile.transform.position;
    }

}
