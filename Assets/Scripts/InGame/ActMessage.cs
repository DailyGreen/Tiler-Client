using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActMessage : MonoBehaviour
{
    // 카메라를 이동시킬, 메세지 행동의 대상
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
        // 버튼 눌렀을때 호출. posX, posY 좌표로 카메라 이동
        Vector3 pos = GameMng.I._hextile.GetCell(posX, posY).transform.position;
        pos.z = -10;
        Camera.main.transform.position = pos;
    }

    IEnumerator waiting()
    {
        yield return new WaitForSeconds(5);

        this.msg.text = "";     // 정상 종료 되었을때만 빈 텍스트로 초기화
        this.gameObject.SetActive(false);
    }
}
