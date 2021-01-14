using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int making;              // ��� ���귮
    public static int cost = 1;     // �Ǽ� ���

    void Start()
    {
        init();
        GameMng.I.AddDelegate(MakingGold);
    }

    void init()
    {
        _name = "����";
        _desc = "��带 Ķ �� �ִ�";
        _hp = 10;
        _code = (int)BUILT.MINE;
        making = 5;
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    /**
     * @brief ��� ����
     */
    void MakingGold()
    {
        Debug.Log("MINE CALLING");
        GameMng.I.addGold(making);
    }

    void OnDestroy()
    {
        GameMng.I.RemoveDelegate(MakingGold);
    }
}
