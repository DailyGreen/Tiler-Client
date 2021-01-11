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
    public int _nowMem = 0;
    public int _maxMem = 0;
    private const int mapWidth = 50;                             // 맵 가로
    private const int mapHeight = 50;                            // 맵 높이
    public Tile[,] mapTile = new Tile[mapWidth, mapHeight];      // 타일의 2차원 배열 값
    public float unitSpeed = 3.0f;
    public float distanceOfTiles = 0.0f;

    public int myTurnCount = 0;                     // 내 차례
    public int myMaxTurnCount = 10;                 // 최대 차례
    public bool myTurn = false;                     // 내 차례인지

    /**********
     * 게임 서브 매니저
     */
    public UnitMng _UnitGM;
    public BuiltMng _BuiltGM;
    public RangeControl _range;

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
    UnityEngine.UI.Text memText;                // 인원
    [SerializeField]
    UnityEngine.UI.Button[] actList;            // 행동
    [SerializeField]
    UnityEngine.UI.Text damageText;             // 데미지
    [SerializeField]
    UnityEngine.UI.Image objImage;              // 오브젝트이미지
    [SerializeField]
    UnityEngine.UI.Image[] logoImage;           //메인바 로고 이미지         0: HP로고 1: 데미지 로고
    [SerializeField]
    UnityEngine.UI.Text turnCountText;          // 턴 수
    [SerializeField]
    UnityEngine.UI.Text turnDescText;           // 누구 턴인지 설명

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
        setMainInterface(false);
        if (NetworkMng.getInstance.uniqueNumber == NetworkMng.getInstance.firstPlayerUniqueNumber)
            myTurn = true;
    }

    /**
     * @brief 골드량을 추가했을 때
     * @param addGold 추가할 골드량
     */
    public void addGold(int gold)
    {
        _gold += gold;
        goldText.text = _gold + "";
    }
    /**
     * @brief 골드를 사용했을 때
     * @param usedGold 사용한 골드량
     */
    public void minGold(int gold)
    {
        _gold -= gold;
        goldText.text = _gold + "";
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

    public void turnManage(string uniqueNumber)
    {
        if (NetworkMng.getInstance.uniqueNumber == int.Parse(uniqueNumber))
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
     * @brief 오브젝트를 클릭했을때
     * @param tile 클릭한 타일 오브젝트
     */
    public void clickTile(Tile tile)
    {
        cleanActList();
        // 유닛이 없다면 정적인 타일이란 뜻
        
        if (tile._unitObj == null && tile._builtObj == null)
        {
            cleanActList();

            objectNameTxt.text = tile._name;
            objectDescTxt.text = tile._desc;
            hpText.enabled = false;
            damageText.enabled = false;
            logoImage[0].enabled = false;                                                   //Hp로고 이미지 꺼둠
            logoImage[1].enabled = false;                                                   //데미지 로고 이미지 꺼둠
            
            NetworkMng.getInstance._soundGM.tileClick();

            objImage.enabled = true;
            objectNameTxt.enabled = true;
            objectDescTxt.enabled = true;

            switch (tile._code)                                                              //클릭한 타일의 코드에 따른 스프라이트값 조정
            {
                case (int)TILE.GRASS:
                    objImage.sprite = objSprite[4];
                    break;
                case (int)TILE.SAND:
                    objImage.sprite = objSprite[5];
                    break;
                case (int)TILE.DIRT:
                    objImage.sprite = objSprite[6];
                    break;
                case (int)TILE.MARS:
                    objImage.sprite = objSprite[7];
                    break;
                case (int)TILE.STONE:
                    objImage.sprite = objSprite[8];
                    break;
                case (int)TILE.SEA_01:
                    objImage.sprite = objSprite[9];
                    break;
                case (int)TILE.SEA_02:
                    objImage.sprite = objSprite[9];
                    break;
                case (int)TILE.SEA_03:
                    objImage.sprite = objSprite[9];
                    break;
            }
            return;
        }
        Object obj;

        if (tile._unitObj)
        {
            obj = tile._unitObj;
            objImage.sprite = objSprite[10];    // 스프라이트 일꾼으로 변경 (나중에 유닛 추가시 switch로 변경)

            setMainInterface(true);
        }
        else
        {
            obj = tile._builtObj;
            switch(tile._builtObj._code)        //타일에 있는 건물의 코드의 따른 스프라이트 변경, 로고 text 켜고 끄기
            {
                case (int)BUILT.MINE:
                    objImage.sprite = objSprite[0];
                    hpText.enabled = true;
                    damageText.enabled = false;
                    logoImage[0].enabled = true;
                    logoImage[1].enabled = false;
                    objImage.enabled = true;
                    objectNameTxt.enabled = true;
                    objectDescTxt.enabled = true;
                    break;
                case (int)BUILT.FARM:
                    objImage.sprite = objSprite[1];
                    hpText.enabled = true;
                    damageText.enabled = false;
                    logoImage[0].enabled = true;
                    logoImage[1].enabled = false;
                    objImage.enabled = true;
                    objectNameTxt.enabled = true;
                    objectDescTxt.enabled = true;
                    break;
                case (int)BUILT.ATTACK_BUILDING:
                    objImage.sprite = objSprite[2];
                    hpText.enabled = true;
                    damageText.enabled = true;
                    logoImage[0].enabled = true;
                    logoImage[1].enabled = true;
                    objImage.enabled = true;
                    objectNameTxt.enabled = true;
                    objectDescTxt.enabled = true;
                    break;
                case (int)BUILT.CASTLE:
                    objImage.sprite = objSprite[3];
                    hpText.enabled = true;
                    damageText.enabled = false;
                    logoImage[0].enabled = true;
                    logoImage[1].enabled = false;
                    objImage.enabled = true;
                    objectNameTxt.enabled = true;
                    objectDescTxt.enabled = true;
                    break;
            }
        }

        objectNameTxt.text = obj._name;
        objectDescTxt.text = obj._desc;

        hpText.text = (tile._unitObj ? tile._unitObj._hp : tile._builtObj._hp) + "" + " / " + (tile._unitObj ? tile._unitObj._hp : tile._builtObj._hp); //나중에 최대체력, 현재체력 구분할 것
        NetworkMng.getInstance._soundGM.unitClick(UNIT.WORKER);
        //damageText.text = tile._unitObj._damage + "";

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
                checkActivity(obj._activity[i], actList[i], childsTxt[0], childsTxt[1]);
            }
            catch
            {
                Debug.LogError("childTxt 의 인덱스 값이 옳지 않음");
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
    public void checkActivity(ACTIVITY activity, UnityEngine.UI.Button actButton, UnityEngine.UI.Text actName, UnityEngine.UI.Text actDesc)
    {
        switch (activity)
        {
            case ACTIVITY.MOVE:
                actName.text = "이동";
                actDesc.text = "한 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Worker.Move(); });
                break;
            case ACTIVITY.BUILD_MINE:
                actName.text = "광산";
                actDesc.text = "한 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Worker.buildMine(); });
                break;
            case ACTIVITY.BUILD_FARM:
                actName.text = "농장";
                actDesc.text = "한 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Worker.buildFarm(); });
                break;
            case ACTIVITY.BUILD_ATTACK_BUILDING:
                actName.text = "터렛";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Worker.buildAttackBuilding(); });
                break;
            case ACTIVITY.BUILD_CREATE_UNIT_BUILDING:
                actName.text = "유닛 건물";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Worker.buildCreateUnitBuilding(); });
                break;
            case ACTIVITY.BUILD_SHIELD_BUILDING:
                actName.text = "방어 건물";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Worker.buildShieldBuilding(); });
                break;
            case ACTIVITY.BUILD_UPGRADE_BUILDING:
                actName.text = "강화 건물";
                actDesc.text = "세 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Worker.buildUpgradeBuilding(); });
                break;
            case ACTIVITY.WORKER_UNIT_CREATE:
                actName.text = "일꾼 생성";
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; Castle.CreateUnit(); });
                break;
            case ACTIVITY.DESTROY_BUILT:
                actName.text = "건물 파괴";
                actButton.onClick.AddListener(delegate { _BuiltGM.act = activity; _BuiltGM.DestroyBuilt(); });
                break;
            case ACTIVITY.ATTACK:                                                               // 임시입니다!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                actName.text = "공격";
                actDesc.text = "두 턴 소요";
                actButton.onClick.AddListener(delegate { _UnitGM.act = activity; Worker.unitAttacking(); });
                break;
            default:
                break;
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
            else selectedTile = hit.collider.gameObject.GetComponent<Tile>();
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
    }

    /**
     * @brief 메인인터페이스 설정
     */
    public void setMainInterface(bool isShow)
    {
        hpText.enabled = isShow;
        objImage.enabled = isShow;
        damageText.enabled = isShow;
        objectNameTxt.enabled = isShow;
        objectDescTxt.enabled = isShow;
        logoImage[0].enabled = isShow;
        logoImage[1].enabled = isShow;
    }

    /**
     * @brief 선택한것들을 지울때
     */
    public void cleanSelected()
    {
        selectedTile = null;
        targetTile = null;
    }

    public void uiClickBT()
    {
        NetworkMng.getInstance._soundGM.uiBTClick();
    }
}

