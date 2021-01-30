using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMng : MonoBehaviour
{
    /**********
     * 방 생성시 UI
     */
    [SerializeField]
    UnityEngine.UI.Text roomNameTxt;            // 방 생성시 이름
    [SerializeField]
    UnityEngine.UI.Text roomPWTxt;              // 방 비밀번호 이름

    /**********
     * 방 찾기 UI
     */
    [SerializeField]
    GameObject roomPrefab;                      // 방 찾기시 생성될 방 버튼들
    [SerializeField]
    GameObject roomList;                        // 발견된 방 리스트들
    [SerializeField]
    GameObject checkPWpop;                      // 비밀번호 체크 팝업
    [SerializeField]
    Animator pwPopAnim;

    /**********
     * 방 데이터
     */
    //[HideInInspector]
    public string roomPW;
    //[HideInInspector]
    public string inputPW;
    //[HideInInspector]
    public string roomIdx;
    public string roomName;
    string nowMem;  // 현재 방에 있는 유저 수

    /**********
     * 로비 UI
     */
    [SerializeField]
    GameObject roomPanel;
    [SerializeField]
    GameObject lobbyPanel;
    
    // Room일때(같은 방에 있는 유저 정보 : 0은 나임)
    [SerializeField]
    UnityEngine.UI.Text roomInfo;
    [SerializeField]
    UnityEngine.UI.Button gameStartBT;          // 방장만 사용할 수 있는 버튼
    [SerializeField]
    GameObject readyUI;                         // 방장이 아닌 유저들에게 보여줄 이미지
    [SerializeField]
    UnityEngine.UI.Image[] tribeImages;         // 종족 이미지들
    [SerializeField]
    UnityEngine.UI.Image[] colorImages;         // 색깔 이미지들
    [SerializeField]
    UnityEngine.UI.Button[] colorBT;            // 색깔 버튼
    [SerializeField]
    Sprite[] tribeSprites;                      // 종족 스프라이트
    public GameObject[] players;                // 플레이어들
    public UnityEngine.UI.Text[] playersName;   // 플레이어 이름 텍스트


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

            NetworkMng.getInstance.v_user.Clear();
            UserInfo userInfo = new UserInfo
            {
                nickName = NetworkMng.getInstance.nickName,
                uniqueNumber = NetworkMng.getInstance.uniqueNumber,
                tribe = 0,
                color = 0
            };
            NetworkMng.getInstance.v_user.Add(userInfo);

            NetworkMng.getInstance.roomOwner = true;
        }
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

    /**
     * @brief 게임 시작 버튼 클릭.  조건 확인해서 시작하기
     */
    public void gameStart()
    {
        //if (!nowMem.Equals("1"))
        //{       
            // 혼자만 아니면 게임 시작 가능
            NetworkMng.getInstance.SendMsg(string.Format("GAME_START"));
        //}
    }

    /**
     * @brief 빠른 방 검색 시도했을때 호출
     */
    public void fastGameStart()
    {
        NetworkMng.getInstance.SendMsg(string.Format("FAST_ROOM"));
    }

    /**
     * @brief 들어가 있던 방을 나올때 호출
     */
    public void exitRoom()
    {
        gameStartBT.gameObject.SetActive(false);
        readyUI.SetActive(true);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
            playersName[i].text = "";
        }

        if (NetworkMng.getInstance.roomOwner)
        {
            for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
            {
                if (!NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
                {
                    NetworkMng.getInstance.SendMsg(string.Format("NOW_ROOM_OWNER:{0}",NetworkMng.getInstance.v_user[i].uniqueNumber));
                    break;
                }
            }
        }

        NetworkMng.getInstance.v_user.Clear();

        NetworkMng.getInstance.SendMsg(string.Format("ROOM_EXIT:{0}", NetworkMng.getInstance.uniqueNumber));

        roomPanel.SetActive(false);
        
        NetworkMng.getInstance._soundGM.loginBGM();

        NetworkMng.getInstance.roomOwner = false;
    }

    /**
     * @brief 방 주인이 내가 됬을때
     */
    public void nowRoomOwner()
    {
        gameStartBT.gameObject.SetActive(true);
        readyUI.SetActive(false);
    }

    /**
     * @brief 다른 사람 방에 들어가고 나서 호출
     */
    public void intoRoom()
    {
        checkPWpop.SetActive(false);
        roomPanel.SetActive(true);
        players[0].SetActive(true);
        playersName[0].text = NetworkMng.getInstance.nickName;
        roomInfo.text = roomName;

        NetworkMng.getInstance._soundGM.roomBGM();
    }

    /**
     * @brief 다른 사람 방에 들어가고 나서 호출
     * @param roomName 방 제목
     */
    public void intoRoom(string roomName)
    {
        intoRoom();
        roomInfo.text = roomName;
    }

    /**
     * @brief 스스로 방 만들고 나서 호출
     */
    public void changeRoom()
    {
        roomPanel.SetActive(true);
        players[0].SetActive(true);
        playersName[0].text = NetworkMng.getInstance.nickName;
        nowMem = "1";
        roomInfo.text = roomNameTxt.text;
        gameStartBT.gameObject.SetActive(true);
        readyUI.SetActive(false);
        NetworkMng.getInstance._soundGM.roomBGM();
    }

    /**
     * @brief 방 정보를 새로고침
     */
    public void roomRefresh()
    {
        for (int i = 1; i < players.Length; i++)
            players[i].SetActive(false);

        int countingMember = 1;

        // 색 순서만큼
        for (int i = 0; i < 9; i++)
        {
            colorBT[i].interactable = true;
        }

        Color color;
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            ColorUtility.TryParseHtmlString(CustomColor.TransColor((COLOR)NetworkMng.getInstance.v_user[i].color), out color);
            colorBT[NetworkMng.getInstance.v_user[i].color].interactable = false;

            // 다른사람꺼
            if (!NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
            {
                players[countingMember].SetActive(true);
                playersName[countingMember].text = NetworkMng.getInstance.v_user[i].nickName;
                tribeImages[countingMember].sprite = tribeSprites[NetworkMng.getInstance.v_user[i].tribe];
                colorImages[countingMember].color = color;
                countingMember++;
            }
            // 내꺼
            else
            {
                tribeImages[0].sprite = tribeSprites[NetworkMng.getInstance.v_user[i].tribe];
                colorImages[0].color = color;
            }
        }
        nowMem = (NetworkMng.getInstance.v_user.Count) + "";
    }

    /**
     * @brief 종족을 변경 (내가 변경했을때. 클라용)
     * @param tribeNum 변경할 종족
     */
    public void wantChangeTribe(int tribeNum)
    {
        NetworkMng.getInstance.myTribe = (TRIBE)tribeNum;
        NetworkMng.getInstance.SendMsg(string.Format("TRIBE:{0}", tribeNum));
    }

    /**
     * @brief 색상을 변경 (내가 변경했을때. 클라용)
     * @param tribeNum 변경할 색상
     */
    public void wantChangeColor (int colorNum)
    {
        NetworkMng.getInstance.myColor = (COLOR)colorNum;
        NetworkMng.getInstance.SendMsg(string.Format("COLOR:{0}", colorNum));
    }

    /**
     * @brief 종족을 변경 (다른 유저가 변경했을때. 서버용)
     * @param uniqueCode 대상
     * @param tribeNum 종족
     */
    public void changeTribe(int uniqueCode, int tribeNum)
    {
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(uniqueCode))
            {
                UserInfo info = NetworkMng.getInstance.v_user[i];
                info.tribe = tribeNum;
                NetworkMng.getInstance.v_user[i] = info;
                break;
            }
        }
        roomRefresh();
    }

    /**
     * @brief 색상을 변경 (다른 유저가 변경했을때. 서버용)
     * @param uniqueCode 대상
     * @param colorNum 색상
     */
    public void changeColor(int uniqueCode, int colorNum)
    {
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(uniqueCode))
            {
                UserInfo info = NetworkMng.getInstance.v_user[i];
                info.color = colorNum;
                NetworkMng.getInstance.v_user[i] = info;
                break;
            }
        }
        roomRefresh();
    }
}
