using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMng : MonoBehaviour
{
    // 채팅
    public UnityEngine.UI.Text chatLogs;
    [SerializeField]
    UnityEngine.UI.InputField chatInput;
    [SerializeField]
    Animator chatAnim;
    [SerializeField]
    GameObject chatPanel;       // 채팅 전체 패널
    public string myChatField;

    public bool isWriting = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //if (!chatPanel.activeSelf)
            if (!isWriting)
            {
                isWriting = true;
                chatAnim.SetTrigger("ChatOpen");
                chatInput.Select();
                //chatInput.ActivateInputField();
                //chatInput.next();
                //chatPanel.SetActive(true);
            }
            else if (myChatField == "" && isWriting)
            {
                isWriting = false;
                chatAnim.SetTrigger("ChatClose");
                //chatPanel.SetActive(false);
            }
            else
            {
                isWriting = false;
                chatAnim.SetTrigger("MessageOpen");
                NetworkMng.getInstance.SendMsg(string.Format("CHAT:{0}", myChatField));
                chatLogs.text += string.Format("\n[{0}] : {1} ({2})", NetworkMng.getInstance.nickName, myChatField, System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute);
                myChatField = "";
                chatInput.text = "";
            }
        }
    }

    public void InputChat()
    {
        myChatField = chatInput.text;
    }

    public void newMessage(string nickName, string msg)
    {
        chatLogs.text += string.Format("\n[{0}] : {1} ({2})", nickName, msg, System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute);
        if (!isWriting)
            chatAnim.SetTrigger("MessageOpen");
    }
}
