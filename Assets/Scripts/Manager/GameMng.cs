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
    private const int mapWidth = 50;            // 맵 가로
    private const int mapHeight = 50;            // 맵 높이
    public Tile[,] mapTile = new Tile[mapWidth, mapHeight];     // 타일의 2차원 배열 값
    public float unitSpeed = 3.0f;
    public float distanceOfTiles = 0.0f;

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
     * 게임 인터페이스
     */
    [SerializeField]
    UnityEngine.UI.Text objectNameTxt;          // 선택 오브젝트 이름
    [SerializeField]
    UnityEngine.UI.Text objectDescTxt;        // 선택 오브젝트 디테일
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
            NetworkMng.getInstance._soundGM.tileClick();
            return;
        }
        Object obj;

        if (tile._unitObj)
            obj = tile._unitObj;
        else
            obj = tile._builtObj;

        objectNameTxt.text = obj._name;
        objectDescTxt.text = obj._desc;

        hpText.text = (tile._unitObj ? tile._unitObj._hp : tile._builtObj._hp) + "";
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
    }

    /**
     * @brief 선택한것들을 지울때
     */
    public void cleanSelected()
    {
        selectedTile = null;
        targetTile = null;
    }
}