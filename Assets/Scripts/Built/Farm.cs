using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Built
{
    public int making;              // �ķ� ���귮
    public static int cost = 1;     // �Ǽ� ���

    void Awake()
    {
        _name = "����";
        _desc = "�������� " + (3 - createCount) + "�� ����";
        _hp = 10;
        _code = (int)BUILT.FARM;
        making = 2;
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
            _desc = "�ķ��� �����Ѵ�";

            //_anim.SetTrigger("isSpawn");

            GameMng.I.RemoveDelegate(this.waitingCreate);

            // �������
            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();

                GameMng.I.AddDelegate(MakingFood);
            }
        }
    }

    /**
     * @brief �ķ� ����
     */
    void MakingFood()
    {
        Debug.Log("MAKING FOOD CALL !!!!!!!!");
        GameMng.I.addFood(making);
    }
    void OnDestroy()
    {
        if (createCount > 2 && NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            GameMng.I.RemoveDelegate(MakingFood);
        else
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
