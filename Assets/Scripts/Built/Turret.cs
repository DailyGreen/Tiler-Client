using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Built
{
    public int attack;   // ���ݷ�
    public static int cost = 5;   // �Ǽ� ���
    // Start is called before the first frame update
    void Start()
    {
        _name = "�ͷ�";
        _desc = "���� ���� �� �����Ÿ� ���� ���� �����Ѵ�";
        _hp = 7;
        attack = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
