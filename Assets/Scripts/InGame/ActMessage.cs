using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActMessage : MonoBehaviour
{
    // ī�޶� �̵���ų, �޼��� �ൿ�� ���
    public int posX;
    public int posY;

    public UnityEngine.UI.Text msg;

    public void setMessage(string msg)
    {
        this.msg.text = msg;
        if (this.msg.text != "")
            StopCoroutine(waiting());
        StartCoroutine(waiting());
    }

    public void moveToTarget()
    {
        // ��ư �������� ȣ��. posX, posY ��ǥ�� ī�޶� �̵�
    }

    IEnumerator waiting()
    {
        yield return new WaitForSeconds(30);

        this.msg.text = "";     // ���� ���� �Ǿ������� �� �ؽ�Ʈ�� �ʱ�ȭ
        this.gameObject.SetActive(false);
    }
}
