using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int making;              // ��� ���귮
    public static int cost = 1;     // �Ǽ� ���

    void Awake()
    {
        _name = "����";
        _desc = "�������� " + (3 - createCount) + "�� ����";
        _hp = 10;
        _code = (int)BUILT.MINE;
        making = 5;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (3 - createCount) + "�� ����";

        // 2�� �Ŀ� ������
        if (createCount > 2)
        {
            _desc = "��带 Ķ �� �ִ�";

            _anim.SetTrigger("isSpawn");

            GameMng.I.RemoveDelegate(this.waitingCreate);

            // �������
            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();

                GameMng.I.AddDelegate(MakingGold);
            }
        }
    }

    /**
     * @brief  ��� ��Ż ���ݷ� �ۼ�Ʈ (�뷱�� ���� �ʿ�)
     * @param attactdmg �������� �����
     * @param attacker ������ ����� ���� ��ȣ
     * @param attected ���� ���� ��� ���� ��ȣ
     */
    public void GoldPlunder(int attactdmg, int attacker, int attected)
    {
        if (GameMng.I.selectedTile._builtObj._uniqueNumber.Equals(attected))
            NetworkMng.getInstance.SendMsg(string.Format("GOLD_PLUNDER:{0}:{1}:{2}", attacker, attected, GameMng.I._gold * attactdmg / 100));
    }

    /**
     * @brief ��� ����
     */
    void MakingGold()
    {
        _anim.SetTrigger("isMaking");
        GameMng.I.addGold(making);
    }

    void OnDestroy()
    {
        if (createCount > 2 && NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            GameMng.I.RemoveDelegate(MakingGold);
        else
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
