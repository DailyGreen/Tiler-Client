using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : Object
{
    public int _hp;
    public int _cost;

    public int _uniqueNumber;      // �÷��̾� ���� �ڵ�

    /*
     * ���� �ִϸ��̼� ����
     */
    public Animator _anim;

    public int createCount = 0;            // ������ �ɸ��� �� �� 
}