using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text mapTxt;
    [SerializeField]
    UnityEngine.UI.Text titleTxt;
    [SerializeField]
    UnityEngine.UI.Text memberTxt;
    [SerializeField]
    GameObject lockObj;
    string roomIdx;
    string pw;

    public void roomSetting(string roomIdx, string title, string nowUser, string limitUser, string pw)
    {
        this.roomIdx = roomIdx;
        mapTxt.text = "MAP_0";
        titleTxt.text = title;
        memberTxt.text = nowUser + "/" + limitUser;
        this.pw = pw;
        if (pw.Equals(""))
            lockObj.SetActive(false);
    }
    
    public void clicked()
    {
        NetworkMng.getInstance._roomGM.checkRoomPW(titleTxt.text, roomIdx, pw);
    }
}
