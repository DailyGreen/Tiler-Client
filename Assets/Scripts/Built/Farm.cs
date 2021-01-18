using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Built
{
    public int making;              // �ķ� ���귮
    public static int cost = 4;     // �Ǽ� ���

    void Awake()
    {
        _name = "����";
        _desc = "�ķ��� �����Ѵ�";
        _hp = 10;
        _code = (int)BUILT.FARM;
        making = 2;
        GameMng.I.AddDelegate(MakingFood);
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    /**
     * @brief �ķ� ����
     */
    void MakingFood()
    {
        Debug.Log("MAKING FOOD CALL !!!!!!!!");
        GameMng.I.addFood(making);
    }

    /**
     * @brief  �ķ� ��Ż ���ݷ� �ۼ�Ʈ (�뷱�� ���� �ʿ�)
     * @param attactdmg �������� �����
     */
    public void FoodPlunder(int attactdmg)
    {
        GameMng.I._food = GameMng.I._food * attactdmg / 100;
    }

    void OnDestroy()
    {
        GameMng.I.RemoveDelegate(MakingFood);
    }
}
