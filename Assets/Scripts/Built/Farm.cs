using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Built
{
    public int making;  // �ķ� ���귮
    public static int cost = 4;   // �Ǽ� ���

    void Start()
    {
        _name = "����";
        _desc = "�ķ��� �����Ѵ�";
        _hp = 10;
        _code = (int)BUILT.FARM;
        making = 2;
        _activity.Add(ACTIVITY.DESTROY_BUILT);
        GameMng.I.AddDelegate(MakingFood);
    }

    void Update()
    {
        if (Input.GetKeyDown("f"))
            MakingFood();
    }

    /**
     * @brief �ķ� ����
     */
    void MakingFood()
    {
        Debug.Log("MAKING FOOD CALL !!!!!!!!");
        GameMng.I.addMaxMem(making);
    }
}
