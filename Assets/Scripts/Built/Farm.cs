using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Built
{
    public int making;  // �ķ� ���귮
    public static int cost = 4;   // �Ǽ� ���

    // Start is called before the first frame update
    void Start()
    {
        _name = "����";
        _desc = "�ķ��� �����Ѵ�";
        _hp = 10;
        making = 2;
    }

    // Update is called once per frame
    void Update()
    {
        MakingFood();
    }

    /**
     * @brief �ķ� ����
     */
    void MakingFood()
    {
        if (Input.GetKeyDown("f"))
            GameMng.I.addMaxMem(making);
    }
}
