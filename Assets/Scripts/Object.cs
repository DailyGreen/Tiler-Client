using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    /*
     * ������Ʈ ���� �ڵ�
     * �ش� �ڵ�� � ������Ʈ���� ������ ���������ϴ�.
     */
    public int _code = 0;

    /*
     * ������Ʈ ����
     * ������Ʈ�� Ŭ�������� �������ִ� �����Դϴ�. ��ӹ޴� ���� �ڽ��� Start ���� ������ �ֽñ� �ٶ��ϴ�.
     */
    public string _name = "";       // ������Ʈ ���� ��Ī
    public string _desc = "";       // ������Ʈ ����

    //public int uniqueNumber;      // �÷��̾� ���� �ڵ�

    public List<ACTIVITY> _activity = new List<ACTIVITY>();

}