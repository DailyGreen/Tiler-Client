using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMng : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text roomNameTxt;        // 방 생성시 이름
    [SerializeField]
    UnityEngine.UI.Text roomPWTxt;          // 방 비밀번호 이름
    //[SerializeField]
    //UnityEngine.UI.Text roomLimitMem;          // 방 비밀번호 이름

    [SerializeField]
    GameObject roomPrefab;                // 방 찾기시 생성될 방 버튼들
    [SerializeField]
    GameObject roomList;               // 발견된 방 리스트들
    [SerializeField]
    GameObject checkPWpop;                  // 비밀번호 체크 팝업
    [SerializeField]
    Animator pwPopAnim;

    //[HideInInspector]
    public string roomPW;
    //[HideInInspector]
    public string inputPW;
    //[HideInInspector]
    public string roomIdx;
    public string roomName;

    [SerializeField]
    GameObject roomPanel;
    [SerializeField]
    GameObject lobbyPanel;
    
    // Room일때(같은 방에 있는 유저 정보 : 0은 나임)
    [SerializeField]
    UnityEngine.UI.Text roomInfo;
    public GameObject[] players;
    public UnityEngine.UI.Text[] playersName;
    [SerializeField]
    UnityEngine.UI.Button gameStartBT;

    //public string roomName;
    string nowMem;  // 현재 방에 있는 유저 수

    void Start()
    {
        roomPW = "";
        inputPW = "";
        NetworkMng.getInstance._roomGM = this;
    }

    /**
     * @brief 방 생성
     */
    public void createRoom()
    {
        if (!roomNameTxt.text.Equals(""))
        {
            NetworkMng.getInstance.SendMsg(string.Format("CREATE_ROOM:{0}:{1}:{2}", roomNameTxt.text, roomPWTxt.text, 3));
        }
    }

    public void writeRoomName(string roomName)
    {
        //this.roomName = roomName;
    }

    /**
     * @brief 방 찾기
     */
    public void foundRoom()
    {
        foreach (Transform child in roomList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        NetworkMng.getInstance.SendMsg("FOUND_ROOM");
    }

    /**
     * @brief 방 입장 게이트 생성
     */
    public void makeGate(string roomIdx, string roomName, string roomPW, string nowMem, string limitMem)
    {
        GameObject go = Instantiate(roomPrefab) as GameObject;
        go.GetComponent<Room>().roomSetting(roomIdx, roomName, nowMem, limitMem, roomPW);
        Debug.Log(go.transform.position);

        //AddListener(go.GetComponent<UnityEngine.UI.Button>(), roomName, roomIdx, roomPW);
        go.transform.SetParent(roomList.transform);
        go.transform.localScale = Vector2.one;
        go.transform.localPosition = new Vector3(0, 0, 0);
    }

    //void AddListener(UnityEngine.UI.Button b, string roomName, string roomIdx, string roomPW)
    //{
    //    b.onClick.AddListener(() => checkRoomPW(roomName, roomIdx, roomPW));
    //}

    /**
     * @brief 방 암호 확인
     * @param roomPW 방 암호
     */
    public void checkRoomPW(string roomName, string roomIdx, string roomPW)
    {
        this.roomName = roomName;
        // 암호가 존재하는 방
        if (!roomPW.Equals(""))
        {
            this.roomIdx = roomIdx;
            this.roomPW = roomPW;
            checkPWpop.SetActive(true);
        }
        else
        {
            this.roomIdx = roomIdx;
            NetworkMng.getInstance.SendMsg(string.Format("INTO_ROOM:{0}", roomIdx));
        }
    }

    /**
     * @brief 입력한 비밀번호 확인
     * @param inputPW 입력한 비밀번호
     */
    public void checkPWCorrect(UnityEngine.UI.Text inputPW)
    {
        // 방 암호와 내가 입력한 암호가 일치 할 때
        if (this.roomPW.Equals(inputPW.text))
        {
            this.inputPW = "";
            this.roomPW = "";
            NetworkMng.getInstance.SendMsg(string.Format("INTO_ROOM:{0}", roomIdx));
        }
        else
        {
            pwPopAnim.SetTrigger("Wrong");
        }
    }

    /**
     * @brief 입력하는 비밀번호 변경
     * @param inputPW 입력한 비밀번호
     */
    public void setPassword(string inputPW)
    {
        this.inputPW = inputPW;
    }

    public void gameStart()
    {
        if (!nowMem.Equals("1") || true)
        {
            // 혼자만 아니면 게임 시작 가능
            NetworkMng.getInstance.SendMsg(string.Format("GAME_START"));
        }
    }

    public void fastGameStart()
    {
        NetworkMng.getInstance.SendMsg(string.Format("FAST_ROOM"));
    }

    public void exitRoom()
    {
        gameStartBT.interactable = false;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
            playersName[i].text = "";
        }
        NetworkMng.getInstance.v_user.Clear();
        NetworkMng.getInstance.SendMsg(string.Format("ROOM_EXIT"));
        roomPanel.SetActive(false);
        NetworkMng.getInstance._soundGM.loginBGM();
        //lobbyPanel.SetActive(true);
    }

    // 다른 사람 방에 들어가고 나서 호출
    public void intoRoom()
    {
        checkPWpop.SetActive(false);
        roomPanel.SetActive(true);
        players[0].SetActive(true);
        playersName[0].text = NetworkMng.getInstance.nickName;
        roomInfo.text = roomName;
        NetworkMng.getInstance._soundGM.roomBGM();
    }
    public void intoRoom(string roomName)
    {
        intoRoom();
        roomInfo.text = roomName;
    }

    // 스스로 방 만들고 나서 호출
    public void changeRoom()
    {
        //lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        players[0].SetActive(true);
        playersName[0].text = NetworkMng.getInstance.nickName;
        nowMem = "1";
        roomInfo.text = roomNameTxt.text;
        gameStartBT.interactable = true;
        NetworkMng.getInstance._soundGM.roomBGM();
    }

    public void roomRefresh()
    {
        for (int i = 1; i < players.Length; i++)
            players[i].SetActive(false);
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            players[1 + i].SetActive(true);
            playersName[1 + i].text = NetworkMng.getInstance.v_user[i].nickName;
        }
        nowMem = (1 + NetworkMng.getInstance.v_user.Count) + "";
    }
}
