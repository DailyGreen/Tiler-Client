using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMng : MonoBehaviour
{
    public delegate void CountTurn();
    public CountTurn countDel;
    private static GameMng _Instance = null;

    /**********
     * 게임 세팅 값
     */
    public int _gold = 0;
    public int _food = 0;
    public int _nowMem = 0;
    public int _maxMem = 0;
    private const int mapWidth = 50;                             // 맵 가로
    private const int mapHeight = 50;                            // 맵 높이
    public Tile[,] mapTile = new Tile[mapWidth, mapHeight];      // 타일의 2차원 배열 값
    public float unitSpeed = 3.0f;
    public float distanceOfTiles = 0.0f;
    public Vector3 CastlePos;

    public int myTurnCount = 0;                     // 내 차례
    public int myMaxTurnCount = 10;                 // 최대 차례
    public bool myTurn = false;                     // 내 차례인지

    /**********
     * 게임 서브 매니저
     */
    public UnitMng _UnitGM;
    public BuiltMng _BuiltGM;
    public RangeControl _range;
    public ChatMng _chat;

    /**********
     * 레이케스트 위한 변수
     */
    public RaycastHit2D hit;
    public Tile selectedTile = null;
    public Tile targetTile = null;
    /**********
     * UI적용을 위한 변수
     */
    [SerializeField]
    Sprite[] objSprite;                         //UI 이미지 적용을 위한 스프라이트 
    //0:광산 1: 농장 2: 터렛 3: 성 4: 풀 5: 모래 6: 흙 7: 화성? 8: 돌 9: 바다 10: 일꾼

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
    UnityEngine.UI.Text memText;                // 인원
    [SerializeField]
    UnityEngine.UI.Button[] actList;            // 행동
    [SerializeField]
    UnityEngine.UI.Text damageText;             // 데미지
    [SerializeField]
    UnityEngine.UI.Image maskImage;             // 오브젝트 이미지 배경
    [SerializeField]
    UnityEngine.UI.Image objImage;              // 오브젝트이미지
    [SerializeField]
    UnityEngine.UI.Image[] logoImage;           // 메인바 로고 이미지         0: HP로고 1: 데미지 로고
    [SerializeField]
    UnityEngine.UI.Text turnCountText;          // 턴 수
    [SerializeField]
    UnityEngine.UI.Text turnDescText;           // 누구 턴인지 설명
    [SerializeField]
    UnityEngine.UI.Image[] frameImg;            // 버튼별 클릭 불가 이미지
    [SerializeField]
    ActMessage[] actMessages;                   // 행동 도우미 메세지들


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
        AddDelegate(SampleTurnFunc);
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
    public void imActing()
    {
        this.myTurnCount++;

        if (this.myMaxTurnCount == this.myTurnCount)
        {
            this.myTurnCount = 0;
            // 턴 교체
            // 원래라면 인원수에 따라 바뀌지만 테스트 용으로 2인플레이라 생각하고 turn 바꿔주는중
            this.myTurn = !this.myTurn;
        }
        this.turnCountText.text = this.myTurnCount + " / " + this.myMaxTurnCount;
        this.turnDescText.text = this.myTurn ? "내 차례" : "상대 차례";
    }

    /**
    * @brief 턴이 변경 되었을때 호출
    * @param uniqueNumber 변경될 유저의 고유 번호
    */
    public void turnManage(int uniqueNumber)
    {
        countDel();
        refreshMainUI();

        // 누구 차례인지 뿌려주기
        if (NetworkMng.getInstance.uniqueNumber == uniqueNumber)
        {
            this.myTurn = true;
            this.turnDescText.text = "내 차례";

            return;
        }
        this.myTurn = false;
        for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
        {
            if (NetworkMng.getInstance.v_user[i].uniqueNumber.Equals(uniqueNumber))
            {
                this.turnDescText.text = NetworkMng.getInstance.v_user[i].nickName + " 차례";
                break;
            }
        }
    }

    /**
     * @brief 메인 UI 새로고침
     */
    public void refreshMainUI()
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

            for (int i = 0; i < obj._activity.Count; i++)
            {
                actList[i].gameObject.SetActive(true);
                UnityEngine.UI.Text[] childsTxt = actList[i].GetComponentsInChildren<UnityEngine.UI.Text>();
                try
                {
                    checkActivity(obj._activity[i], actList[i], childsTxt[0], childsTxt[1], frameImg[i]);
                }
                catch
                {
                    Debug.LogError("childTxt 의 인덱스 값이 옳지 않음");
                }
            }
        }
        // 내 턴이 아닐때 타일에 어떤 반응이 있는지 새로고침
        else if (!myTurn)
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

        // 유닛이 없다면 정적인 타일이란 뜻
        if (tile._unitObj == null && tile._builtObj == null)
        {
            maskImage.color = Color.white;
            objectNameTxt.text = tile._name;
            objectDescTxt.text = tile._desc;

            objImage.sprite = getObjSprite(tile._code);
            setMainInterface(false, false);

            NetworkMng.getInstance._soundGM.tileClick();

            return;
        }

        DynamicObject obj;
        if (tile._unitObj)
        {
            obj = tile._unitObj;
            objImage.sprite = getObjSprite(tile._unitObj._code);
            setMainInterface();
        }
        else
        {
            obj = tile._builtObj;
            //타일에 있는 건물의 코드의 따른 스프라이트 변경, 로고 text 켜고 끄기
            objImage.sprite = getObjSprite(tile._builtObj._code);
        }

        objectNameTxt.text = obj._name;
        objectDescTxt.text = obj._desc;

        hpText.text = (tile._unitObj ? tile._unitObj._hp : tile._builtObj._hp) + "" + " / " + (tile._unitObj ? tile._unitObj._max_hp : tile._builtObj._max_hp);
        NetworkMng.getInstance._soundGM.unitClick(UNIT.FOREST_WORKER);
        //damageText.text = tile._unitObj._damage + "";

        Color color;
        if (tile._builtObj != null || tile._unitObj != null)
        {
            for (int i = 0; i < NetworkMng.getInstance.v_user.Count; i++)
            {
                if (obj._uniqueNumber.Equals(NetworkMng.getInstance.v_user[i].uniqueNumber))
                {
                    Debug.Log("in");
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
                    checkActivity(obj._activity[i], actList[i], childsTxt[0], childsTxt[1], frameImg[i]);
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
     */
    public void checkActivity(ACTIVITY activity, UnityEngine.UI.Button actButton, UnityEngine.UI.Text actName, UnityEngine.UI.Text actDesc, UnityEngine.UI.Image Frame)
    {
        switch (activity)
        {
            case ACTIVITY.MOVE:
                actName.text = "이동";
                actDesc.text = "한 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; _range.AttackrangeTileReset(); Unit.Move(); });
                actButton.interactable = true;
                Frame.enabled = false;
                break;
            case ACTIVITY.BUILD_MINE:
                actName.text = "광산";
                actDesc.text = "한 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Unit.buildMine(); });
                canUseActivity(actButton, Frame, Mine.cost);
                break;
            case ACTIVITY.BUILD_FARM:
                actName.text = "농장";
                actDesc.text = "한 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Unit.buildFarm(); });
                canUseActivity(actButton, Frame, Farm.cost);
                break;
            case ACTIVITY.BUILD_ATTACK_BUILDING:
                actName.text = "터렛";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Unit.buildAttackBuilding(); });
                canUseActivity(actButton, Frame, Turret.cost);
                break;
            case ACTIVITY.BUILD_MILLITARY_BASE:
                actName.text = "군사 기지";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Unit.buildMillitaryBaseBuilding(); });
                canUseActivity(actButton, Frame, MillitaryBase.cost);
                break;
            case ACTIVITY.BUILD_SHIELD_BUILDING:
                actName.text = "방어 건물";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Unit.buildShieldBuilding(); });
                break;
            case ACTIVITY.BUILD_UPGRADE_BUILDING:
                actName.text = "강화 건물";
                actDesc.text = "세 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Unit.buildUpgradeBuilding(); });
                break;
            case ACTIVITY.WORKER_UNIT_CREATE:
                actName.text = "일꾼 생성";
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; Castle.CreateUnitBtn(); });
                canUseActivity(actButton, Frame, Forest_Worker.cost);
                break;
            case ACTIVITY.DESTROY_BUILT:
                actName.text = "건물 파괴";
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; _BuiltGM.DestroyBuilt(); });
                break;
            case ACTIVITY.ATTACK:                                                            
                actName.text = "공격";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; _range.rangeTileReset(); Unit.unitAttacking(); });
                break;
            case ACTIVITY.SOLDIER_0_UNIT_CREATE:
                actName.text = "전사1 생성";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; MillitaryBase.CreateAttackFirstUnitBtn(); });
                canUseActivity(actButton, Frame, Forest_Soldier_0.cost);
                break;
            case ACTIVITY.SOLDIER_1_UNIT_CREATE:
                actName.text = "전사2 생성";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; MillitaryBase.CreateAttackSecondUnitBtn(); });
                canUseActivity(actButton, Frame, Forest_Soldier_1.cost);
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
                _range.SelectTileSetting(false);
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
        setMainInterface(false, false, false);
    }

    /**
    * @brief 레이케스트 레이저 생성 및 hit 리턴
    * @param isTarget 레이케스트 타겟을 변경할때 사용. targetTile 값을 받아올때 true 해주면 됨
    */
    public void addActMessage(string msg, int posX, int posY)
    {
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
     * @brief 메인인터페이스 설정
     */
    public void setMainInterface(bool showHP = true, bool showDamage = true, bool showObj= true)
    {
        hpText.enabled = showHP;
        logoImage[0].enabled = showHP;
        damageText.enabled = showDamage;
        logoImage[1].enabled = showDamage;

        objImage.enabled = showObj;
        objectNameTxt.enabled = showObj;
        objectDescTxt.enabled = showObj;
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
        if (mapTile[posY, posX]._unitObj != null) obj = mapTile[posY, posX]._unitObj;
        else if (mapTile[posY, posX]._builtObj != null) obj = mapTile[posY, posX]._builtObj;
        else return;
        _UnitGM.reversalUnit(obj.transform, mapTile[toY, toX].transform);
        obj._anim.SetTrigger("isAttacking");

        // 공격받는 대상의 HP 가 줄어들게 해줌
        obj = null;
        if (mapTile[toY, toX]._unitObj != null) obj = mapTile[toY, toX]._unitObj;
        else if (mapTile[toY, toX]._builtObj != null) obj = mapTile[toY, toX]._builtObj;
        else return;

        if (mapTile[toY, toX]._builtObj != null)
        {
            if (obj._uniqueNumber.Equals(NetworkMng.getInstance.uniqueNumber) && mapTile[toY, toX]._code.Equals((int)BUILT.MINE))
            {
                NetworkMng.getInstance.SendMsg(string.Format("PLUNDER:{0}:{1}:{2}:{3}",
                    mapTile[posY, posX]._unitObj._uniqueNumber, mapTile[toY, toX]._builtObj._uniqueNumber, _gold * (damage * 2) / 100, 1));
            }
            else
            {
                NetworkMng.getInstance.SendMsg(string.Format("PLUNDER:{0}:{1}:{2}:{3}",
                    mapTile[posY, posX]._unitObj._uniqueNumber, mapTile[toY, toX]._builtObj._uniqueNumber, _food * (damage * 2) / 100, 1));
            }
        }

        obj._hp -= damage;
        if (obj._hp <= 0)
        {
            // 파괴
            obj.DestroyMyself();
            //Destroy(obj.gameObject);
            mapTile[toY, toX]._unitObj = null;
            mapTile[toY, toX]._builtObj = null;
            mapTile[toY, toX]._code = 0;            // TODO : 코드 값 원래 값으로
        }

    }
    public void uiClickBT()
    {
        NetworkMng.getInstance._soundGM.uiBTClick();
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

