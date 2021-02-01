using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameMng : MonoBehaviour
{
    public delegate void CountTurn();
    public CountTurn countDel;
    private static GameMng _Instance = null;

    /**********
     * 게임 세팅 값
     */
    [HideInInspector]
    public int _gold = 0;
    [HideInInspector]
    public int _food = 0;
    [HideInInspector]
    public int _nowMem = 0;
    [HideInInspector]
    public int _maxMem = 0;
    private const int mapWidth = 50;                             // 맵 가로
    private const int mapHeight = 50;                            // 맵 높이
    [HideInInspector]
    public Vector3 CastlePos;                                   // 내 성의 Transform 위치값
    [HideInInspector]
    public int CastlePosX, CastlePosZ;                          // 내 성의 X, Z 값
    [HideInInspector]
    public bool isWriting = false;                  // 채팅을 치고 있는지

    //public int myTurnCount = 0;                     // 내 차례
    //public int myMaxTurnCount = 10;                 // 최대 차례


    /**********
     * 오브젝트 데이터
     */
    [HideInInspector]
    public int WORKER_COST = 0;
    [HideInInspector]
    public int SOLDIER_0_COST = 0;
    [HideInInspector]
    public int SOLDIER_1_COST = 0;
    [HideInInspector]
    public int SOLDIER_2_COST = 0;
    [HideInInspector]
    public int WITCH_0_COST = 0;
    [HideInInspector]
    public int WITCH_1_COST = 0;

    [HideInInspector]
    public int TURRET_COST = 0;
    [HideInInspector]
    public int FARM_COST = 0;
    [HideInInspector]
    public int MINE_COST = 0;
    [HideInInspector]
    public int MILITARYBASE_COST = 0;

    [HideInInspector]
    public float unitSpeed = 3.0f;

    /**********
     * 턴 관련 데이터
     */
    public int TurnCount = 0;                       // 몇턴이 지났는지
    public int RandomCount = 0;                     // 몇턴 후 보급이 생성 되는지
    public bool myTurn = false;                     // 내 차례인지
    public int countHungry = 0;                     // 몇턴이나 굶었는지


    /**********
     * 게임 서브 매니저
     */
    public UnitMng _UnitGM;
    public BuiltMng _BuiltGM;
    public RangeControl _range;
    public ChatMng _chat;
    public HexTileCreate _hextile;
    public MainCamera _mainCamera;

    /**********
     * 타일 인풋 관련 데이터
     */
    [HideInInspector]
    public RaycastHit2D hit;
    [HideInInspector]
    public Tile selectedTile = null;
    [HideInInspector]
    public Tile targetTile = null;
    [HideInInspector]
    public float distanceOfTiles = 0.0f;

    /**********
     * UI적용을 위한 변수
     */
    [SerializeField]
    Sprite[] objSprite;                         // UI 이미지 적용을 위한 스프라이트 
    //0:광산 1: 농장 2: 터렛 3: 성 4: 풀 5: 모래 6: 흙 7: 화성? 8: 돌 9: 바다 10: 일꾼
    [SerializeField]
    GameObject mainBarObj;                      // 메인 바 UI
    [SerializeField]
    GameObject loseUI;                          // 패배 UI
    public GameObject winUI;                    // 승리 UI

    /**********
     * 게임 인터페이스
     */
    [SerializeField]
    UnityEngine.UI.Text objectNameTxt;          // 선택 오브젝트 이름
    [SerializeField]
    UnityEngine.UI.Text objectDescTxt;          // 선택 오브젝트 디테일
    [SerializeField]
    UnityEngine.UI.Text hpText;                 // HP 디테일
    [SerializeField]
    UnityEngine.UI.Text goldText;               // 골드
    [SerializeField]
    UnityEngine.UI.Text foodText;               // 식량
    [SerializeField]
    UnityEngine.UI.Image foodImage;             // 식량 이미지
    [SerializeField]
    UnityEngine.UI.Text memText;                // 인원
    [SerializeField]
    UnityEngine.UI.Button[] actList;            // 행동
    [SerializeField]
    UnityEngine.UI.Text damageText;             // 데미지
    [SerializeField]
    UnityEngine.UI.Text maintCostText;          // 유지 비용 UI
    [SerializeField]
    UnityEngine.UI.Image maskImage;             // 오브젝트 이미지 배경
    [SerializeField]
    UnityEngine.UI.Image objImage;              // 오브젝트이미지
    [SerializeField]
    UnityEngine.UI.Image[] logoImage;           // 메인바 로고 이미지         0: HP로고 1: 데미지 로고 2: 유지비 로고
    [SerializeField]
    UnityEngine.UI.Text turnCountText;          // 턴 수
    [SerializeField]
    UnityEngine.UI.Image turnDescImage;         // 누구 턴인지 이미지 (색 입혀서)
    [SerializeField]
    UnityEngine.UI.Text turnDescText;           // 누구 턴인지 설명
    [SerializeField]
    UnityEngine.UI.Image[] frameImg;            // 버튼별 클릭 불가 이미지
    [SerializeField]
    UnityEngine.UI.Image[] costImg;             // 버튼별 코스트(골드) 아이콘
    [SerializeField]
    ActMessage[] actMessages;                   // 행동 도우미 메세지들
    [SerializeField]
    UnityEngine.UI.Image[] playerListImg;       // 유저 목록들 이미지   (tab키 UI)
    [SerializeField]
    UnityEngine.UI.Image[] playerListTribe;     // 유저 목록들 종족   (tab키 UI)
    [SerializeField]
    UnityEngine.UI.Text[] playerListName;       // 유저 목록들 이름   (tab키 UI)
    [SerializeField]
    GameObject[] playerListExit;                // 유저 목록들 사망 이미지   (tab키 UI)
    [SerializeField]
    GameObject enemySelectedTile;               // 적이 선택한 타일을 보여주는 오브젝트
    [SerializeField]
    Sprite[] tribeSprites;                      // 종족 이미지들
    [SerializeField]
    UnityEngine.UI.Image myFlagImage;           // 내 프로필을 보여주는 이미지 (색)
    [SerializeField]
    UnityEngine.UI.Image myFlagTribe;           // 내 프로필을 보여주는 이미지 (종족)
    [SerializeField]
    UnityEngine.UI.Text myFlagNickname;         // 내 프로필을 보여주는 이미지 (이름)
    [SerializeField]
    UnityEngine.UI.Slider audioSlider;          // 오디오 볼륨 세팅
    [SerializeField]
    UnityEngine.UI.Slider effectSlider;         // 이펙트 볼륨 세팅
    [SerializeField]
    GameObject myTurnEffect;                    // 내 턴 이펙트    
    [SerializeField]
    UnityEngine.UI.Text logsText;               // 로그 텍스트

    // ---- 맵의 가로 세로 크기 읽기
    public int GetMapWidth
    {
        get
        {
            return mapWidth;
        }
    }
    public int GetMapHeight
    {
        get
        {
            return mapHeight;
        }
    }
    // ----
    public static GameMng I
    {
        get
        {
            if (_Instance.Equals(null))
            {
                Debug.Log("instance is null");
            }
            return _Instance;
        }
    }

    void Awake()
    {
        _Instance = this;
    }

    void Start()
    {
        init();
    }

    public void init()
    {
        _gold = 100;
        _nowMem = 0;
        _maxMem = 0;

        setMainInterface(false, false, false);

        if (NetworkMng.getInstance.uniqueNumber == NetworkMng.getInstance.firstPlayerUniqueNumber)
            myTurn = true;

        // 턴제 함수에 빈 함수 넣어줌 (안 넣어주면 초기 실행시 에러나기 때문)
        AddDelegate(SampleTurnFunc);

        // 같이 플레이 중인 유저 목록들 보여주기
        UserListRefresh(NetworkMng.getInstance.firstPlayerUniqueNumber);

        audioSlider.value = NetworkMng.getInstance._soundGM.audioVolume;
        effectSlider.value = NetworkMng.getInstance._soundGM.effectVolume;

        // 누구 턴인지 색 변경
        Color color;
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(NetworkMng.getInstance.firstPlayerUniqueNumber))
            {
                ColorUtility.TryParseHtmlString(CustomColor.TransColor((COLOR)NetworkMng.getInstance.v_user[i].color), out color);
                turnDescImage.color = color;
                break;
            }
        }

        // 내 깃발 정보를 표시해줌
        myFlagNickname.text = NetworkMng.getInstance.nickName;
        ColorUtility.TryParseHtmlString(CustomColor.TransColor((COLOR)NetworkMng.getInstance.myColor), out color);
        myFlagImage.color = color;
        myFlagTribe.sprite = tribeSprites[(int)NetworkMng.getInstance.myTribe];

        // 게임 시작 로그 남기기
        addLogMessage("시스템", "게임이 시작되었습니다.");

        // 내 종족 오브젝트의 코스트를 설정
        switch ((int)NetworkMng.getInstance.myTribe)
        {
            case 0:    // 숲 종족
                WORKER_COST = 4;
                SOLDIER_0_COST = 4;
                SOLDIER_1_COST = 5;
                SOLDIER_2_COST = 3;
                WITCH_0_COST = 6;
                WITCH_1_COST = 7;
                TURRET_COST = 3;
                FARM_COST = 2;
                MINE_COST = 2;
                MILITARYBASE_COST = 4;
                break;
            case 1:    // 물 종족
                WORKER_COST = 4;
                SOLDIER_0_COST = 5;
                SOLDIER_1_COST = 4;
                SOLDIER_2_COST = 6;
                WITCH_0_COST = 6;
                WITCH_1_COST = 8;
                TURRET_COST = 3;
                FARM_COST = 3;
                MINE_COST = 2;
                MILITARYBASE_COST = 5;
                break;
            case 2:    // 사막 종족
                WORKER_COST = 4;
                SOLDIER_0_COST = 5;
                SOLDIER_1_COST = 5;
                SOLDIER_2_COST = 4;
                WITCH_0_COST = 5;
                WITCH_1_COST = 6;
                TURRET_COST = 4;
                FARM_COST = 2;
                MINE_COST = 2;
                MILITARYBASE_COST = 5;
                break;
        }

    }

    void SampleTurnFunc()
    {
        Debug.Log("countTurn 호출됨! ! !");
    }

    /**
     * @brief 골드량을 추가했을 때
     * @param gold 추가할 골드량
     */
    public void addGold(int gold)
    {
        _gold += gold;
        goldText.text = _gold + "";
    }
    /**
     * @brief 골드를 사용했을 때
     * @param gold 사용한 골드량
     */
    public void minGold(int gold)
    {
        _gold -= gold;
        goldText.text = _gold + "";
    }
    /**
     * @brief 식량을 추가했을 때
     * @param food 추가할 식량
     */
    public void addFood(int food)
    {
        _food += food;
        foodText.text = _food + "";
    }
    /**
     * @brief 식량을 사용했을 때
     * @param food 사용한 식량량
     */
    public void minFood(int food)
    {
        _food -= food;
        foodText.text = _food + "";
    }
    /**
     * @brief 현재 유닛 수를 추가했을 때
     * @param mem 추가할 유닛 수
     */
    public void addNowMem(int mem)
    {
        _nowMem += mem;
        memText.text = _nowMem + " / " + _maxMem;
    }
    /**
     * @brief 현재 유닛 수가 줄었을 때
     * @param mem 줄일 유닛 수
     */
    public void minNowMem(int mem)
    {
        _nowMem -= mem;
        memText.text = _nowMem + " / " + _maxMem;
    }
    /**
     * @brief 최대 유닛 수를 추가했을 때
     * @param mem 추가할 유닛 수
     */
    public void addMaxMem(int mem)
    {
        _maxMem += mem;
        memText.text = _nowMem + " / " + _maxMem;
    }
    /**
     * @brief 최대 유닛 수가 줄었을 때
     * @param mem 줄일 유닛 수
     */
    public void minMaxMem(int mem)
    {
        _maxMem -= mem;
        memText.text = _nowMem + " / " + _maxMem;
    }

    /**
     * @brief 턴 세기
     * @param Method 턴에 추가할 함수
     */
    public void AddDelegate(CountTurn Method)
    {
        this.countDel += Method;
    }

    public void RemoveDelegate(CountTurn Method)
    {
        this.countDel -= Method;
    }

    // 버그 없으면 지울것
    //public void imActing()
    //{
    //    this.myTurnCount++;

    //    if (this.myMaxTurnCount == this.myTurnCount)
    //    {
    //        this.myTurnCount = 0;
    //        // 턴 교체
    //        // 원래라면 인원수에 따라 바뀌지만 테스트 용으로 2인플레이라 생각하고 turn 바꿔주는중
    //        this.myTurn = !this.myTurn;
    //    }
    //    this.turnCountText.text = this.myTurnCount + " / " + this.myMaxTurnCount;
    //    this.turnDescText.text = this.myTurn ? "내 차례" : "상대 차례";
    //}

    /**
     * @brief 턴이 변경 되었을때 호출
     * @param uniqueNumber 변경될 유저의 고유 번호
     */
    public void turnManage(int uniqueNumber)
    {
        Color color;

        countDel();
        refreshMainUI();
        UserListRefresh(uniqueNumber);

        // 유지비가 - 가 되었다면 디버프 행동 추가
        if (_food < 0)
            countHungry++;
        else
            countHungry = 0;

        if (RandomCount == 0)
        {
            // 1~10턴 중에 랜덤
            RandomCount = Random.Range(1, 11);
        }
        else
        {
            // 나중에 더 좋은 방법이 있으면 변경
            if (TurnCount % RandomCount == 0)
            {
                // 턴을 카운트해서 특정 턴마다 생성
                _BuiltGM.CreateAirDrop(Random.Range(0, _hextile.cells.Length));
                RandomCount = 0;
            }
        }

        TurnCount++;

        /// 유지비가 - 가 되었다면 디버프 행동 추가
        // 행동 불능으로 만든다거나
        // 죽게 만든다던가

        // 누구 차례인지 뿌려주기
        if (NetworkMng.getInstance.uniqueNumber == uniqueNumber)
        {
            this.myTurn = true;
            this.turnDescText.text = "내 차례";

            ColorUtility.TryParseHtmlString(CustomColor.TransColor(NetworkMng.getInstance.myColor), out color);
            turnDescImage.color = color;

            enemySelectedTile.gameObject.transform.position = new Vector3(-100, -100, 0);

            if (selectedTile != null)
                NetworkMng.getInstance.SendMsg(string.Format("SELECTING:{0}:{1}", selectedTile.PosX, selectedTile.PosZ));

            myTurnEffect.SetActive(true);

            NetworkMng.getInstance._soundGM.myTurnEffect();

            return;
        }
        cleanActList();
        this.myTurn = false;
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(uniqueNumber))
            {
                this.turnDescText.text = NetworkMng.getInstance.v_user[i].nickName + " 차례";

                ColorUtility.TryParseHtmlString(CustomColor.TransColor((COLOR)NetworkMng.getInstance.v_user[i].color), out color);
                turnDescImage.color = color;

                myTurnEffect.SetActive(false);

                break;
            }
        }
    }

    /**
     * @brief 메인 UI 
     * @param onActList ActList 활성화를 수락할지
     */
    public void refreshMainUI(bool onActList = true)
    {
        if (selectedTile == null)
            return;

        // 누르고 있던 오브젝트가 있다면 턴이 지나고 바꼈을 가능성이 있으니 새로고침 해주기
        DynamicObject obj = null;
        if (selectedTile._unitObj != null) { obj = selectedTile._unitObj; }
        else if (selectedTile._builtObj != null) { obj = selectedTile._builtObj; }


        // 내 턴이 아닐떄 건물이나 유닛이 어떤 행동을 했는지 새로고침
        if (obj != null)
        {
            objImage.sprite = getObjSprite(obj._code);
            objectNameTxt.text = obj._name;
            objectDescTxt.text = obj._desc;
            hpText.text = obj._hp + " / " + obj._max_hp;
            setMainInterface();

            if (onActList && myTurn)
            {
                for (int i = 0; i < obj._activity.Count; i++)
                {
                    actList[i].gameObject.SetActive(true);
                    UnityEngine.UI.Text[] childsTxt = actList[i].GetComponentsInChildren<UnityEngine.UI.Text>();
                    try
                    {
                        checkActivity(obj._activity[i], actList[i], childsTxt[0], childsTxt[1], frameImg[i], childsTxt[2], costImg[i]);
                    }
                    catch
                    {
                        Debug.LogError("childTxt 의 인덱스 값이 옳지 않음");
                    }
                }
            }
        }
        // 내 턴이 아닐때 타일에 어떤 반응이 있는지 새로고침
        else
        {
            // 이동했거나 사망했음. 빈타일임 (내가 한 행동이였다면 바꿀 필요가 없음)
            objImage.sprite = getObjSprite(selectedTile._code);
            maskImage.color = Color.white;      // TODO : 오브젝트 주인꺼 색 뿌리기
            objectNameTxt.text = selectedTile._name;
            objectDescTxt.text = selectedTile._desc;
            setMainInterface(false, false);
        }
    }

    /**
     * @brief 턴 UI 새로고침
     */
    public void refreshTurn()
    {
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(NetworkMng.getInstance.firstPlayerUniqueNumber))
            {
                if (NetworkMng.getInstance.firstPlayerUniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
                {
                    this.turnDescText.text = "내 차례";
                    break;
                }
                this.turnDescText.text = NetworkMng.getInstance.v_user[i].nickName + " 차례";
                break;
            }
        }
    }

    /**
    * @brief 오브젝트를 클릭했을때
    * @param tile 클릭한 타일 오브젝트
    */
    public void clickTile(Tile tile)
    {
        cleanActList();
        mainBarObj.SetActive(true);

        objImage.transform.localScale = Vector3.one;

        // 유닛이 없다면 정적인 타일이란 뜻
        if (tile._unitObj == null && tile._builtObj == null)
        {
            maskImage.color = Color.white;
            objectNameTxt.text = tile._name;
            objectDescTxt.text = tile._desc;

            objImage.sprite = getObjSprite(tile._code);
            setMainInterface(false, false);
            objImage.SetNativeSize();

            NetworkMng.getInstance._soundGM.tileClick();

            return;
        }

        DynamicObject obj;
        if (tile._unitObj)
        {
            obj = tile._unitObj;
            objImage.sprite = getObjSprite(tile._unitObj._code);

            setMainInterface();
            objImage.SetNativeSize();
            NetworkMng.getInstance._soundGM.unitClick(obj._code);

            damageText.text = tile._unitObj._damage.ToString();
            maintCostText.text = tile._unitObj.maintenanceCost.ToString();
        }
        else
        {
            logoImage[2].sprite = foodImage.sprite;
            obj = tile._builtObj;
            //타일에 있는 건물의 코드의 따른 스프라이트 변경, 로고 text 켜고 끄기
            objImage.sprite = getObjSprite(tile._builtObj._code);
            NetworkMng.getInstance._soundGM.builtClick();

            if (obj._code == (int)BUILT.ATTACK_BUILDING)
            {
                damageText.text = Turret.attack.ToString();
                maintCostText.text = Turret.maintenanceCost.ToString();
                logoImage[2].sprite = costImg[0].sprite;
            }

            if (selectedTile._code == (int)BUILT.CASTLE)
            {
                objImage.transform.localScale = Vector3.one;
                objImage.SetNativeSize();
            }
            else
            {
                objImage.transform.localScale = new Vector3(2f, 2f, 1f);
                objImage.SetNativeSize();
            }
        }

        objectNameTxt.text = obj._name;
        objectDescTxt.text = obj._desc;

        hpText.text = (tile._unitObj ? tile._unitObj._hp : tile._builtObj._hp) + "" + " / " + (tile._unitObj ? tile._unitObj._max_hp : tile._builtObj._max_hp);
        //damageText.text = tile._unitObj._damage + "";

        Color color;
        if (tile._builtObj != null || tile._unitObj != null)
        {
            for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
            {
                if (obj._uniqueNumber.Equals(NetworkMng.getInstance.v_user[i].uniqueNumber))
                {
                    ColorUtility.TryParseHtmlString(CustomColor.TransColor((COLOR)NetworkMng.getInstance.v_user[i].color), out color);
                    maskImage.color = color;
                }
            }
        }

        if (obj._uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber) && myTurn)
        {
            // 행동을 가진 오브젝트는 actList 를 뿌려줘야 함
            // 1. _unitObj 로 부터 해당 유닛이 가진 행동의 량을 가져옴
            for (int i = 0; i < obj._activity.Count; i++)
            {
                // 2. 그만큼 actList 를 active 함
                actList[i].gameObject.SetActive(true);
                UnityEngine.UI.Text[] childsTxt = actList[i].GetComponentsInChildren<UnityEngine.UI.Text>();
                try
                {
                    // 3. actList 의 내용들을 변경해 줘야함
                    checkActivity(obj._activity[i], actList[i], childsTxt[0], childsTxt[1], frameImg[i], childsTxt[2], costImg[i]);
                }
                catch
                {
                    Debug.LogError("childTxt 의 인덱스 값이 옳지 않음");
                }
            }
        }
    }

    /**
     * @brief 어떤 행동인지 체크
     * @param activity 행동 코드
     * @param actButton 행동 버튼
     * @param actName 행동 이름
     * @param actDesc 행동 설명
     * @param costText 행동 비용
     */
    public void checkActivity(ACTIVITY activity, UnityEngine.UI.Button actButton, UnityEngine.UI.Text actName, UnityEngine.UI.Text actDesc, UnityEngine.UI.Image Frame, UnityEngine.UI.Text costText, UnityEngine.UI.Image costImg)
    {
        switch (activity)
        {
            case ACTIVITY.MOVE:
                actName.text = "이동";
                actDesc.text = "한 턴 소요";
                costText.enabled = false;
                costImg.enabled = false;
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; _range.AttackrangeTileReset(); _UnitGM.Move(); });
                canUseActivity(actButton, Frame, 0);
                break;
            case ACTIVITY.BUILD_MINE:
                actName.text = "광산";
                actDesc.text = "한 턴 소요";
                costText.enabled = true;
                costImg.enabled = true;
                costText.text = MINE_COST.ToString();
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; _UnitGM.buildMine(); });
                canUseActivity(actButton, Frame, MINE_COST);
                break;
            case ACTIVITY.BUILD_FARM:
                actName.text = "농장";
                actDesc.text = "한 턴 소요";
                costText.enabled = true;
                costImg.enabled = true;
                costText.text = FARM_COST.ToString();
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; _UnitGM.buildFarm(); });
                canUseActivity(actButton, Frame, FARM_COST);
                break;
            case ACTIVITY.BUILD_ATTACK_BUILDING:
                actName.text = "터렛";
                actDesc.text = "두 턴 소요";
                costText.enabled = true;
                costImg.enabled = true;
                costText.text = TURRET_COST.ToString();
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; _UnitGM.buildAttackBuilding(); });
                canUseActivity(actButton, Frame, TURRET_COST);
                break;
            case ACTIVITY.BUILD_MILLITARY_BASE:
                actName.text = "군사 기지";
                actDesc.text = "두 턴 소요";
                costText.enabled = true;
                costImg.enabled = true;
                costText.text = MILITARYBASE_COST.ToString();
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; _UnitGM.buildMillitaryBaseBuilding(); });
                canUseActivity(actButton, Frame, MILITARYBASE_COST);
                break;
            case ACTIVITY.BUILD_SHIELD_BUILDING:
                actName.text = "방어 건물";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; _UnitGM.buildShieldBuilding(); });
                break;
            case ACTIVITY.BUILD_UPGRADE_BUILDING:
                actName.text = "강화 건물";
                actDesc.text = "세 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; _UnitGM.buildUpgradeBuilding(); });
                break;
            case ACTIVITY.WORKER_UNIT_CREATE:
                actName.text = "일꾼 생성";
                costText.enabled = true;
                costImg.enabled = true;
                costText.text = WORKER_COST.ToString();
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; Castle.CreateUnitBtn(); });
                canUseActivity(actButton, Frame, WORKER_COST);
                break;
            case ACTIVITY.DESTROY_BUILT:
                actName.text = "건물 파괴";
                costText.enabled = false;
                costImg.enabled = false;
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; _BuiltGM.DestroyBuilt(); });
                canUseActivity(actButton, Frame, 0);
                break;
            case ACTIVITY.ATTACK:
                actName.text = "공격";
                actDesc.text = "두 턴 소요";
                costText.enabled = false;
                costImg.enabled = false;
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; _range.rangeTileReset(); _UnitGM.unitAttacking(); });
                canUseActivity(actButton, Frame, 0);
                break;
            case ACTIVITY.SOLDIER_0_UNIT_CREATE:
                actName.text = "전사1 생성";
                actDesc.text = "두 턴 소요";
                costText.enabled = true;
                costImg.enabled = true;
                costText.text = SOLDIER_0_COST.ToString();
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; MillitaryBase.CreateAttackFirstUnitBtn(); });
                canUseActivity(actButton, Frame, SOLDIER_0_COST);
                break;
            case ACTIVITY.SOLDIER_1_UNIT_CREATE:
                actName.text = "전사2 생성";
                actDesc.text = "두 턴 소요";
                costText.enabled = true;
                costImg.enabled = true;
                costText.text = SOLDIER_1_COST.ToString();
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; MillitaryBase.CreateAttackSecondUnitBtn(); });
                canUseActivity(actButton, Frame, SOLDIER_1_COST);
                break;
            case ACTIVITY.SOLDIER_2_UNIT_CREATE:
                actName.text = "전사3 생성";
                actDesc.text = "두 턴 소요";
                costText.enabled = true;
                costImg.enabled = true;
                costText.text = SOLDIER_2_COST.ToString();
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; MillitaryBase.CreateAttackThirdUnitBtn(); });
                canUseActivity(actButton, Frame, SOLDIER_2_COST);
                break;
            case ACTIVITY.WITCH_0_UNIT_CREATE:
                actName.text = "마법사1 생성";
                actDesc.text = "두 턴 소요";
                costText.enabled = true;
                costImg.enabled = true;
                costText.text = WITCH_0_COST.ToString();
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; MillitaryBase.CreateAttackFourthUnitBtn(); });
                canUseActivity(actButton, Frame, WITCH_0_COST);
                break;
            case ACTIVITY.WITCH_1_UNIT_CREATE:
                actName.text = "마법사2 생성";
                actDesc.text = "두 턴 소요";
                costText.enabled = true;
                costImg.enabled = true;
                costText.text = WITCH_1_COST.ToString();
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; MillitaryBase.CreateAttackFifthUnitBtn(); });
                canUseActivity(actButton, Frame, WITCH_1_COST);
                break;
            default:
                break;
        }
    }

    /**
     * @brief 사용할 수 있는 행동인지 확인(골드량 비교)
     * @param actButton 버튼
     * @param Frame 비활성화 이미지
     * @param cost 비용
     */
    public void canUseActivity(UnityEngine.UI.Button actButton, UnityEngine.UI.Image Frame, int cost)
    {
        if (_gold >= cost)
        {
            actButton.interactable = true;
            Frame.enabled = false;
        }
        else
        {
            actButton.interactable = false;
            Frame.enabled = true;
        }
        if (selectedTile._unitObj != null)
        {
            if (!selectedTile._unitObj._bActAccess)
            {
                actButton.interactable = false;
                Frame.enabled = true;
            }
            else
            {
                actButton.interactable = true;
                Frame.enabled = false;
            }
        }
        else if (selectedTile._builtObj != null)
        {
            if (!selectedTile._builtObj._bActAccess)
            {
                actButton.interactable = false;
                Frame.enabled = true;
            }
            else
            {
                actButton.interactable = true;
                Frame.enabled = false;
            }
        }
    }

    /**
    * @brief 레이케스트 레이저 생성 및 hit 리턴
    * @param isTarget 레이케스트 타겟을 변경할때 사용. targetTile 값을 받아올때 true 해주면 됨
    */
    public void mouseRaycast(bool isTarget = false)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray2D ray = new Ray2D(pos, Vector2.zero);

        hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if (isTarget) targetTile = hit.collider.gameObject.GetComponent<Tile>();
            else
            {
                selectedTile = hit.collider.gameObject.GetComponent<Tile>();
                _hextile.FindDistancesTo(selectedTile);
                _range.SelectTileSetting(false);
                if (myTurn)
                    NetworkMng.getInstance.SendMsg(string.Format("SELECTING:{0}:{1}", selectedTile.PosX, selectedTile.PosZ));
                Debug.Log("타일 선택");
            }
        }
    }

    /**
     * @brief 행동 UI 를 지울때 사용
     */
    public void cleanActList()
    {
        for (int i = 0; i < actList.Length; i++)
        {
            actList[i].onClick.RemoveAllListeners();
            actList[i].gameObject.SetActive(false);
        }
        setMainInterface(false);

        if (myTurn)
            mainBarObj.SetActive(false);
    }

    /**
     * @brief 유저 이름을 uniqueNumber로 알아올때 사용
     * @param uniqueNumber 알아올 유저의 고유 번호
     * @return 유저 이름
     */
    public string getUserName(int uniqueNumber)
    {
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(uniqueNumber))
            {
                return NetworkMng.getInstance.v_user[i].nickName;
            }
        }
        return "(알수없음)";
    }

    /**
     * @brief 유저 종족을 uniqueNumber로 알아올때 사용
     * @param uniqueNumber 알아올 유저의 고유 번호
     * @return 유저 종족
     */
    public string getUserTribe(int uniqueNumber)
    {
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(uniqueNumber))
            {
                switch (NetworkMng.getInstance.v_user[i].tribe)
                {
                    case 0:
                        return "숲";
                    case 1:
                        return "물";
                    case 2:
                        return "사막";
                }
            }
        }
        return "(알수없음)";
    }

    /**
     * @brief 행동 메세지를 추가할때
     * @param msg 메세지 내용
     * @param posX 대상 위치
     * @param posY 대상 위치
     */
    public void addActMessage(string msg, int posX, int posY)
    {
        NetworkMng.getInstance._soundGM.newActMsg();

        for (int i = 0; i < 5; i++)
        {
            if (actMessages[i].gameObject.activeSelf == false)
            {
                actMessages[i].gameObject.SetActive(true);
                actMessages[i].setMessage(msg);
                actMessages[i].posX = posX;
                actMessages[i].posY = posY;
                return;
            }
        }
        // 모두 활성화가 되 있는 상태면 이쪽으로 오게 되어 있음
        for (int i = 0; i < 4; i++)
        {
            actMessages[i].setMessage(actMessages[i + 1].msg.text);
            actMessages[i].posX = actMessages[i + 1].posX;
            actMessages[i].posY = actMessages[i + 1].posY;
        }
        actMessages[4].setMessage(msg);
        actMessages[4].posX = posX;
        actMessages[4].posY = posY;
    }

    /**
     * @brief 로그 메세지를 추가할때
     * @param name 대상 이름
     * @param msg 내용
     */
    public void addLogMessage(string name, string msg)
    {
        logsText.text += string.Format("\n[{0}] : {1} ({2})", name, msg, System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute);
    }

    /**
     * @brief 메인인터페이스 설정
     */
    public void setMainInterface(bool showHP = true, bool showDamage = true, bool showObj = true)
    {
        hpText.enabled = showHP;
        logoImage[0].enabled = showHP;
        damageText.enabled = showDamage;
        logoImage[1].enabled = showDamage;
        maintCostText.enabled = showDamage;    // 유지비는 대미지와 마찬가지로 유닛만 가지고 있음
        logoImage[2].enabled = showDamage;

        objImage.enabled = showObj;
        objectNameTxt.enabled = showObj;
        objectDescTxt.enabled = showObj;
    }

    /**
     * @brief 같이 플레이 중인 유저 목록들 보여주기
     * @param uniqueNumber 현재 턴의 유저
     */
    public void UserListRefresh(int uniqueNumber)
    {
        Color color;
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            ColorUtility.TryParseHtmlString(CustomColor.TransColor((COLOR)NetworkMng.getInstance.v_user[i].color), out color);
            playerListImg[i].gameObject.SetActive(true);
            playerListImg[i].color = color;
            playerListTribe[i].sprite = tribeSprites[(int)NetworkMng.getInstance.v_user[i].tribe];
            playerListName[i].text = NetworkMng.getInstance.v_user[i].nickName;
            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(uniqueNumber))
                playerListImg[i].transform.localScale = new Vector3(1.3f, 1.3f, 1);
            else
                playerListImg[i].transform.localScale = new Vector3(1, 1, 1);
        }
    }

    /**
     * @brief 같이 플레이 중이던 유저가 나가거나 죽었을때
     * @param uniqueNumber 대상 유저 번호
     */
    public void UserExit(int uniqueNumber)
    {
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(uniqueNumber))
            {
                playerListExit[i].SetActive(true);
                playerListImg[i].transform.localScale = new Vector3(1, 1, 1);
                break;
            }
        }

        // 내가 죽었다면 패배 UI 켜기
        if (uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
        {
            loseUI.SetActive(true);
        }
    }

    /**
     * @brief 선택한것들을 지울때
     */
    public void cleanSelected()
    {
        selectedTile = null;
        targetTile = null;
    }

    /**
     * @brief 유저 이름 변경
     */
    public void attack(int posX, int posY, int toX, int toY, int damage)
    {

        // 공격하는 대상이 공격하는 애니메이션을 취하도록 해줌
        DynamicObject obj = null;
        if (_hextile.GetCell(posX, posY)._unitObj != null) obj = _hextile.GetCell(posX, posY)._unitObj;
        else if (_hextile.GetCell(posX, posY)._builtObj != null) obj = _hextile.GetCell(posX, posY)._builtObj;
        else return;
        if (_hextile.GetCell(posX, posY)._builtObj == null)
            _UnitGM.reversalUnit(obj.transform, _hextile.GetCell(toX, toY).transform);
        obj._anim.SetTrigger("isAttacking");

        addActMessage(string.Format("{0}님이 공격했습니다.", getUserName(obj._uniqueNumber)), posX, posY);

        addLogMessage(getUserName(obj._uniqueNumber), "공격을 시도했습니다.");

        // 공격받는 대상의 HP 가 줄어들게 해줌
        obj = null;
        if (_hextile.GetCell(toX, toY)._unitObj != null) obj = _hextile.GetCell(toX, toY)._unitObj;
        else if (_hextile.GetCell(toX, toY)._builtObj != null) obj = _hextile.GetCell(toX, toY)._builtObj;
        else return;

        if (_hextile.GetCell(toX, toY)._builtObj != null)
        {
            if (obj._uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber) && _hextile.GetCell(toX, toY)._code.Equals((int)BUILT.MINE))
            {
                NetworkMng.getInstance.SendMsg(string.Format("PLUNDER:{0}:{1}:{2}:{3}",
                    _hextile.GetCell(posX, posY)._unitObj._uniqueNumber, _hextile.GetCell(toX, toY)._builtObj._uniqueNumber, _gold * (damage * 2) / 100, 1));
            }
            else
            {
                NetworkMng.getInstance.SendMsg(string.Format("PLUNDER:{0}:{1}:{2}:{3}",
                    _hextile.GetCell(posX, posY)._unitObj._uniqueNumber, _hextile.GetCell(toX, toY)._builtObj._uniqueNumber, _food * (damage * 2) / 100, 1));
            }
        }

        if (_hextile.GetCell(posX, posY)._unitObj._code == (int)UNIT.FOREST_WITCH_0 + (int)(NetworkMng.getInstance.myTribe) * 6 ||
                    _hextile.GetCell(posX, posY)._unitObj._code == (int)UNIT.FOREST_WITCH_1 + (int)(NetworkMng.getInstance.myTribe) * 6)
            EffectSave(_hextile.GetCell(posX, posY)._unitObj._code, _hextile.GetCell(toX, toY).GetTileVec2.x, _hextile.GetCell(toX, toY).GetTileVec2.y);

        obj._hp -= damage;
        if (obj._hp <= 0)
        {
            // 내 성이 파괴되었다면 서버에게 말해줌
            if (obj._code.Equals(BUILT.CASTLE) && obj._uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber))
            {
                NetworkMng.getInstance.SendMsg(string.Format("LOSE:{0}", NetworkMng.getInstance.uniqueNumber));
            }

            // 파괴
            obj.DestroyMyself();
            //Destroy(obj.gameObject);
            _hextile.GetCell(toX, toY)._unitObj = null;
            _hextile.GetCell(toX, toY)._builtObj = null;
            _hextile.TilecodeClear(toX, toY);            // TODO : 코드 값 원래 값으로
        }
    }

    /**
     * @brief 터랫 공격
     * @param posX 터렛의 posX
     * @param posY 터렛의 posY
     * @param damage 터렛의 대미지
     * @param uniqenum 공격한 터렛의 uniqenum
     */
    public void TurretAttack(int posX, int posY, int damage, int uniqenum)
    {
        Tile turrettile = null;
        int count = 0;

        turrettile = _hextile.GetCell(posX, posY);
        _hextile.FindDistancesTo(turrettile);
        for (int i = 0; i < _hextile.cells.Length; i++)
        {
            if (count >= 18) break;
            if (_hextile.cells[i].Distance <= 2)
            {
                count++;
                if (_hextile.cells[i]._unitObj != null && !_hextile.cells[i]._unitObj._uniqueNumber.Equals(uniqenum))
                {
                    turrettile._builtObj.GetComponent<Turret>()._anim.SetTrigger("isAttacking");
                    _hextile.cells[i]._unitObj._hp -= damage;
                    refreshMainUI();
                    if (_hextile.cells[i]._unitObj._hp <= 0)
                    {
                        // 파괴
                        _hextile.cells[i]._unitObj.DestroyMyself();
                        _hextile.cells[i]._unitObj = null;
                        _hextile.TilecodeClear(_hextile.cells[i]);            // TODO : 코드 값 원래 값으로
                    }
                }
            }
        }
    }
    /**
     * @brief 클릭한 타일의 코드에 따른 스프라이트값 조정
     * @param unitcode 코드
     * @param targetX, targetY targetTile의 트랜스폼 X,Y값
     */
    void EffectSave(int unitcode, float targetX, float targetY)
    {
        GameObject EffectParent = GameObject.Find("AttackEffect");
        switch (unitcode)
        {
            case (int)UNIT.FOREST_WITCH_0:
                _UnitGM.AttackEffect = EffectParent.transform.Find("Forest").transform.Find("ForestEffect_0").gameObject;
                break;
            case (int)UNIT.FOREST_WITCH_1:
                _UnitGM.AttackEffect = EffectParent.transform.Find("Forest").transform.Find("ForestEffect_1").gameObject;
                break;
            case (int)UNIT.SEA_WITCH_0:
                _UnitGM.AttackEffect = EffectParent.transform.Find("Sea").transform.Find("SeaEffect_0").gameObject;
                break;
            case (int)UNIT.SEA_WITCH_1:
                _UnitGM.AttackEffect = EffectParent.transform.Find("Sea").transform.Find("SeaEffect_2").gameObject;
                break;
            case (int)UNIT.DESERT_WITCH_0:
                _UnitGM.AttackEffect = EffectParent.transform.Find("Desert").transform.Find("DesertEffect_0").gameObject;
                break;
            case (int)UNIT.DESERT_WITCH_1:
                _UnitGM.AttackEffect = EffectParent.transform.Find("Desert").transform.Find("DesertEffect_2").gameObject;
                break;
            default:
                break;
        }
        _UnitGM.AttackEffect.SetActive(true);
        _UnitGM.AttackEffect.transform.position = new Vector3(targetX, targetY, -10f);
        StartCoroutine("SaveEffectReset");
    }

    /**
     * @brief 파티클이 끝난 이펙트 위치 초기화
     */
    IEnumerator SaveEffectReset()
    {
        yield return new WaitForSeconds(2.3f);                      // 현재 있는 이펙트중 제일 오래 유지되는 이펙트가 2.3초임
        _UnitGM.AttackEffect.transform.localPosition = new Vector3(0f, 0f, 10f);
        _UnitGM.AttackEffect.SetActive(false);
        _UnitGM.AttackEffect = null;
    }

    /**
     * @brief UI 버튼 눌렀을때
     */
    public void uiClickBT()
    {
        NetworkMng.getInstance._soundGM.uiBTClick();
    }

    /**
     * @brief 상대방이 클릭한 타일 세팅
     * @param posX 상대방이 클릭한 타일 X
     * @param posZ 상대방이 클릭한 타일 Y
     */
    public void enemyClickTile(int posX, int posZ)
    {
        enemySelectedTile.transform.position = _hextile.GetCell(posX, posZ).transform.position;
    }

    /**
     * @brief 항복
     */
    public void surrender()
    {
        NetworkMng.getInstance.SendMsg(string.Format("LOSE:{0}", NetworkMng.getInstance.uniqueNumber));
        SceneManager.LoadScene("Lobby");    // TODO : Networkmange가 중복되서 버그남
    }

    /**
     * @brief 오디오 볼륨 사이즈 조절
     */
    public void changeAudioVolume(float vol)
    {
        NetworkMng.getInstance._soundGM.changeAudioVolume(vol);
    }

    /**
     * @brief 효과음 볼륨 사이즈 조절
     */
    public void changeEffectVolume(float vol)
    {
        NetworkMng.getInstance._soundGM.changeEffectVolume(vol);
    }

    /**
     * @brief 클릭한 타일의 코드에 따른 스프라이트값 조정
     * @param code 코드
     * @return 스프라이트 이미지
     */
    public Sprite getObjSprite(int code)
    {
        switch (code)
        {
            case (int)TILE.GRASS:
                return objSprite[4];
            case (int)TILE.SAND:
                return objSprite[5];
            case (int)TILE.DIRT:
                return objSprite[6];
            case (int)TILE.MARS:
                return objSprite[7];
            case (int)TILE.STONE:
                return objSprite[8];
            case (int)TILE.SEA_01:
                return objSprite[9];
            case (int)TILE.SEA_02:
                return objSprite[9];
            case (int)TILE.SEA_03:
                return objSprite[9];
            case (int)UNIT.FOREST_WORKER:
                return objSprite[12];
            case (int)UNIT.FOREST_SOLDIER_0:
                return objSprite[13];
            case (int)UNIT.FOREST_SOLDIER_1:
                return objSprite[14];
            case (int)UNIT.FOREST_SOLDIER_2:
                return objSprite[15];
            case (int)UNIT.FOREST_WITCH_0:
                return objSprite[16];
            case (int)UNIT.FOREST_WITCH_1:
                return objSprite[17];
            case (int)UNIT.SEA_WORKER:
                return objSprite[18];
            case (int)UNIT.SEA_SOLDIER_0:
                return objSprite[19];
            case (int)UNIT.SEA_SOLDIER_1:
                return objSprite[20];
            case (int)UNIT.SEA_SOLDIER_2:
                return objSprite[21];
            case (int)UNIT.SEA_WITCH_0:
                return objSprite[22];
            case (int)UNIT.SEA_WITCH_1:
                return objSprite[23];
            case (int)UNIT.DESERT_WORKER:
                return objSprite[24];
            case (int)UNIT.DESERT_SOLDIER_0:
                return objSprite[25];
            case (int)UNIT.DESERT_SOLDIER_1:
                return objSprite[26];
            case (int)UNIT.DESERT_SOLDIER_2:
                return objSprite[27];
            case (int)UNIT.DESERT_WITCH_0:
                return objSprite[28];
            case (int)UNIT.DESERT_WITCH_1:
                return objSprite[29];
            case (int)BUILT.MINE:
                setMainInterface(true, false);
                return objSprite[0];
            case (int)BUILT.FARM:
                setMainInterface(true, false);
                return objSprite[1];
            case (int)BUILT.ATTACK_BUILDING:
                setMainInterface();
                return objSprite[2];
            case (int)BUILT.CASTLE:
                setMainInterface(true, false);
                return objSprite[3];
            case (int)BUILT.AIRDROP:
                setMainInterface(false, false);
                return objSprite[10];
            case (int)BUILT.MILLITARY_BASE:
                setMainInterface(true, false);
                return objSprite[11];
        }
        return null;
    }
}