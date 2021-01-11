using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int making;  // ��� ���귮
    public static int cost = 1;   // �Ǽ� ���

    void Start()
    {
        _name = "����";
        _desc = "��带 Ķ �� �ִ�";
        _hp = 10;
        _code = (int)BUILT.MINE;
        making = 5;
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    void Update()
    {
        MakingGold();
    }

    /**
     * @brief ��� ����
     */
    void MakingGold()
    {
        if (Input.GetKeyDown("g"))
            GameMng.I.addGold(making);
    }
}
