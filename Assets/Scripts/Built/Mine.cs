using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int making;  // ��� ���귮
    public static int cost = 1;   // �Ǽ� ���

    // Start is called before the first frame update
    void Start()
    {
        _name = "����";
        _desc = "��带 Ķ �� �ִ�";
        _hp = 10;
        making = 5;
    }

    // Update is called once per frame
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
