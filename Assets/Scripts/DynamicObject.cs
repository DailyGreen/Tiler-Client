using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : Object
{
    public int _hp;
    public int _max_hp;
    //public int _cost;

    public int _uniqueNumber;      // �÷��̾� ���� �ڵ�
    public SpriteRenderer _emoteSide;      // ��ǳ�� �̸��� ��

    /**
     * ���� �ִϸ��̼� ����
     */
    public Animator _anim;
    public AnimationClip dyingClip;

    /**
     * ���� ���� ����
     */
    public int createCount = 0;             // ������ �ɸ��� �� �� 
    public int maxCreateCount = 0;          // �����Ǵ� �� �� (�ѹ� �������� �����Ǽ� �ȵ�)

    /**
     * ���� �Ÿ� ����
     */
    public int _basedistance = 0;           // �̵� �Ÿ� �Ǵ� ���� �Ÿ�
    public int _attackdistance = 0;         // ���� �Ÿ�

    public int SaveX, SaveY;                // ����� ���� ������Ʈ�� ��ġ�� ������ ����

    public bool _bActAccess = true;         // ������Ʈ���� �ൿ ����

    public void DestroyMyself()
    {
        _anim.SetTrigger("isDying");

        Destroy(this.gameObject, dyingClip.length - .2f);
    }

    public Color GetUserColor()
    {
        Color color;
        ColorUtility.TryParseHtmlString(CustomColor.TransColor(NetworkMng.getInstance.myColor), out color);
        return color;
    }
}