using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Built : DynamicObject
{
    public int _turn = 0;   // �Ǽ��� �Ҹ��ϴ� �ϼ�
    void Start()
    {
        _code = 199;
        Debug.Log("128921uwqd");
    }

    public Mine mine = null;

    public Farm farm = null;

    public Turret turret = null;

    /**
     * @brief �ǹ��� ����
     * @param GameObject built     ������ �ǹ� ���ӿ�����Ʈ
     * @param int cost             ������ �ǹ��� �Ǽ����
     */

}