using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int stack;   // ���� ��差
    public int making;  // ���� ��差

    // Start is called before the first frame update
    void Start()
    {
        _name = "����";
        _desc = "��带 Ķ �� �ִ�";
        _hp = 10;
        _cost = 0;

        stack = 0;
        making = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("g"))
            stack += making;
    }
}
