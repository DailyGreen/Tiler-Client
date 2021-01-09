using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
public struct UserInfo
{
    public string nickName;     // 이름
    public int tribe;           // 종족
    public int startPos;        // 성 시작지점
}

public class NetworkMng : MonoBehaviour
{
    static Socket socket = null;
    public string address = "127.0.0.1";   // 주소, 서버 주소와 같게 할 것
    int port = 8080;               // 포트 번호, 서버포트와 같게 할 것
    byte[] buf = new byte[4096];
    int recvLen = 0;
    public int myRoom = 0;
    public int uniqueNumber = 0;        // 나 자신을 가리키는 고유 숫자
    public Dictionary<string, UserInfo> _users = new Dictionary<string, UserInfo>();  // 플레이 하는 유저들 정보

    public GameObject mainPanel;
    public GameObject loadingPanel;
    public GameObject loginPanel;
    public Animator loadingAnim;

    public string nickName;
    public List<User> v_user = new List<User>();
    public int firstPlayerUniqueNumber = -1;

    public RoomMng _roomGM;
    public SoundMng _soundGM;

    [SerializeField]
    GameObject profile;
    [SerializeField]
    UnityEngine.UI.Text profileNickname;

    static NetworkMng _instance;
    public static NetworkMng getInstance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        _instance = this;
    }

    IEnumerator LoginWaiting()
    {
        yield return new WaitForSeconds(3);

        IPAddress serverIP = IPAddress.Parse(address);
        int serverPort = Convert.ToInt32(port);
        Debug.Log(address);
        Debug.Log(serverPort);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);      // 송신 제한시간 10초
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);   // 수신 제한시간 10초

        // 서버가 닫혀 있을것을 대비하여 예외처리
        try
        {
            socket.Connect(new IPEndPoint(serverIP, serverPort));
            StartCoroutine("PacketProc");

            loadingPanel.SetActive(false);
            mainPanel.SetActive(true);
            profile.SetActive(true);
        }
        catch (SocketException err)
        {
            Debug.Log("서버가 닫혀있습니다. : " + err.ToString());
            Logout();
        }
        catch (Exception ex)
        {
            Debug.Log("ERROR 개반자에게 문의 : " + ex.ToString());
            Logout();
        }
    }


    /**
     * @brief 서버에 접속 
     */
    public void Login()
    {
        if (!nickName.Equals("_") && !nickName.Equals(""))
        {
            loginPanel.SetActive(false);
            loadingPanel.SetActive(true);
            loadingAnim.SetTrigger("Loading");

            if (checkNetwork())
            {
                Logout();       // 이중 접속 방지

                StartCoroutine("LoginWaiting");
            }
            else
            {
                loginPanel.SetActive(true);
                loadingPanel.SetActive(false);
            }
        }
    }

    /**
     * @brief 접속 종료 
     */
    public void Logout()
    {
        if (socket != null && socket.Connected)
            socket.Close();
        StopCoroutine("PacketProc");
    }

    /**
     * @brief 채팅
     * @param input 내용
     */
    public void Chat(InputField input)
    {
        SendMsg(string.Format("CHAT:{0}", input.text));
    }

    /**
     * @brief 접속 종료 
     * @param nickName 이름
     * @param pos 생성 위치
     * @param isPlayer 나 인가 아닌가
     */
    //public void CreateUser(string nickName, Vector3 pos, bool isPlayer)
    //{
    //    GameObject obj = Instantiate(playerPrefs, pos, Quaternion.identity) as GameObject;
    //    User player = obj.GetComponent<User>();

    //    player.nickName.text = nickName;
    //    player.isPlayer = isPlayer;
    //    player.myMove = moveC;
    //    player.setDirection(seeDir);

    //    v_user.Add(player);
    //}

    /**
     * @brief 서버에게 패킷 전달
     * @param txt 패킷 내용
     */
    public void SendMsg(string txt)
    {
        try
        {
            if (socket != null && socket.Connected)
            {
                byte[] buf = new byte[4096];

                Buffer.BlockCopy(ShortToByte(Encoding.UTF8.GetBytes(txt).Length + 2), 0, buf, 0, 2);

                Buffer.BlockCopy(Encoding.UTF8.GetBytes(txt), 0, buf, 2, Encoding.UTF8.GetBytes(txt).Length);

                socket.Send(buf, Encoding.UTF8.GetBytes(txt).Length + 2, 0);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    /**
     * @brief 패킷 처리 업데이트
     */
    IEnumerator PacketProc()
    {
        while (true)
        {
            if (socket.Connected)
                if (socket.Available > 0)
                {
                    byte[] buf = new byte[4096];
                    int nRead = socket.Receive(buf, socket.Available, 0);

                    if (nRead > 0)
                    {
                        Buffer.BlockCopy(buf, 0, this.buf, recvLen, nRead);
                        recvLen += nRead;

                        while (true)
                        {
                            int len = BitConverter.ToInt16(this.buf, 0);

                            if (len > 0 && recvLen >= len)
                            {
                                ParsePacket(len);
                                recvLen -= len;
                                Buffer.BlockCopy(this.buf, len, this.buf, 0, recvLen);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            yield return null;
        }
    }

    /**
     * @brief 패킷 분석
     */
    public void ParsePacket(int len)
    {
        string msg = Encoding.UTF8.GetString(this.buf, 2, len - 2);
        string[] txt = msg.Split(':');

        // 접속 - 첫 로그인
        if (txt[0].Equals("CONNECT"))
        {
            Debug.Log("Connected.");
            SendMsg(string.Format("LOGIN:{0}", nickName));
            profileNickname.text = nickName;
        }
        // 방에 있던 사람 중 누군가 나감
        else if (txt[0].Equals("SOMEONE_EXIT"))
        {
            v_user.Clear();

            if (!txt[1].Equals("_"))
            {
                User tempUser = new User();
                tempUser.nickName = txt[1];
                v_user.Add(tempUser);
            }
            _roomGM.roomRefresh();
        }
        else if (txt[0].Equals("GAME_START"))
        {
            Debug.Log(msg);

            uniqueNumber = int.Parse(txt[1]);

            Debug.Log("GAME START !!!");
            _soundGM.waitBGM();
            Debug.Log("txt 메세지 사이즈 (2개 + 3*인원수 여야됨) : " + txt.Length);

            // 2: 고유번호, 3: 닉네임, 4: 첫 시작 위치
            for (int k = 0; k < (txt.Length - 2) / 3; k++)
            {
                UserInfo userInfo = new UserInfo
                {
                    nickName = txt[3 + k * 3],
                    startPos = int.Parse(txt[4 + k * 3])
                };
                _users.Add(txt[2 + k * 3], userInfo);
            }
            firstPlayerUniqueNumber = int.Parse(txt[2]);

            SceneManager.LoadScene("InGame");

        }
        else if (txt[0].Equals("TURN"))
        {
            GameMng.I.turnManage(txt[1]);
        }
        // 직접 방 생성후 이동
        else if (txt[0].Equals("CHANGE_ROOM"))
        {
            myRoom = int.Parse(txt[1]);
            // 광장이 아닐때
            if (!myRoom.Equals(0))
            {
                _roomGM.changeRoom();
            }
        }
        // 방 정보 받음
        else if (txt[0].Equals("FOUND_ROOM"))
        {
            _roomGM.makeGate(txt[1], txt[2], txt[3], txt[4], txt[5]);
        }
        // 방에 접속 시도후 받음
        else if (txt[0].Equals("ENTER_ROOM"))
        {
            if (txt[1].Equals("IN"))
            {
                v_user.Clear();

                if (!txt[2].Equals("_"))
                {
                    User tempUser = new User();
                    tempUser.nickName = txt[2];
                    v_user.Add(tempUser);
                }
                if (!txt[3].Equals("_"))
                {
                    User tempUser = new User();
                    tempUser.nickName = txt[3];
                    v_user.Add(tempUser);
                }
                _roomGM.intoRoom(txt[4]);
                _roomGM.roomRefresh();
            }
            else if (txt[1].Equals("LIMIT"))
            {
                myRoom = 0;
            }
            else if (txt[1].Equals("MISS"))
            {
                myRoom = 0;
            }
        }
        // 누군가 들어올때 받음
        else if (txt[0].Equals("SOMEONE_ENTER"))
        {
            Debug.Log("SOMEONE ENTER");
            User tempUser = new User();
            tempUser.nickName = txt[1];
            v_user.Add(tempUser);
            _roomGM.roomRefresh();
        }
    }

    /**
     * @brief 기기에서 접속을 끊었을때 
     */
    void OnDestroy()
    {
        if (socket != null && socket.Connected)
        {
            // 광장이 아니였을 때
            if (myRoom != 0)
                SendMsg(string.Format("ROOM_EXIT"));
            SendMsg("DISCONNECT");
            Thread.Sleep(500);
            socket.Close();
        }
        StopCoroutine("PacketProc");
    }

    /**
     * @brief 유저 이름 변경
     */
    public void setNickName(string nickName)
    {
        this.nickName = nickName;
    }

    /**
     * @brief 인터넷 연결되어 있는지 확인
     */
    public bool checkNetwork()
    {
        string HtmlText = GetHtmlFromUri("http://google.com");
        if (HtmlText.Equals(""))
        {
            // 연결 실패
            Debug.Log("인터넷 연결 실패");
        }
        else if (!HtmlText.Contains("schema.org/WebPage"))
        {
            // 비정상적인 루트일때
            Debug.Log("인터넷 연결 실패");
        }
        else
        {
            // 성공적인 연결
            Debug.Log("인터넷 연결 되있음");
            return true;
        }

        return false;
    }

    /**
     * @brief html 받아오기
     * @param resource url
     */
    public string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

    /**
     * @brief int 를 2바이트 데이터로 변환
     * @param val 변경할 변수
     */
    public static byte[] ShortToByte(int val)
    {
        byte[] temp = new byte[2];
        temp[1] = (byte)((val & 0x0000ff00) >> 8);
        temp[0] = (byte)((val & 0x000000ff));
        return temp;
    }
}
