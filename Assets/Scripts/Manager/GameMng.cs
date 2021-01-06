using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMng : MonoBehaviour
{
    public delegate void CountTurn();
    public CountTurn countDel;
    private static GameMng _Instance = null;

    public int _gold = 0;
    public int _nowMem = 0;
    public int _maxMem = 0;

    public UnitMng _UnitGM;

    //public bool wantToBuilt = false;
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

    private const int mapWidth = 20;            // 맵 가로
    private const int mapHeight = 5;            // 맵 높이

    public Tile[,] mapTile = new Tile[mapHeight, mapWidth];     // 타일의 2차원 배열 값

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
     * @brief : 골드량을 추가했을 때
     * @param addGold : 추가할 골드량
     */
    public void addGold(int gold)
    {
        _gold += gold;
        goldText.text = _gold + "";
    }
    /**
     * @brief : 골드를 사용했을 때
     * @param usedGold : 사용한 골드량
     */
    public void minGold(int gold)
    {
        _gold -= gold;
        goldText.text = _gold + "";
    }
    /**
     * @brief : 현재 유닛 수를 추가했을 때
     * @param mem : 추가할 유닛 수
     */
    public void addNowMem(int mem)
    {
        _nowMem += mem;
        memText.text = _nowMem + " / " + _maxMem;
    }
    /**
     * @brief : 현재 유닛 수가 줄었을 때
     * @param mem : 줄일 유닛 수
     */
    public void minNowMem(int mem)
    {
        _nowMem -= mem;
        memText.text = _nowMem + " / " + _maxMem;
    }
    /**
     * @brief : 최대 유닛 수를 추가했을 때
     * @param mem : 추가할 유닛 수
     */
    public void addMaxMem(int mem)
    {
        _maxMem += mem;
        memText.text = _nowMem + " / " + _maxMem;
    }
    /**
     * @brief : 최대 유닛 수가 줄었을 때
     * @param mem : 줄일 유닛 수
     */
    public void minMaxMem(int mem)
    {
        _maxMem -= mem;
        memText.text = _nowMem + " / " + _maxMem;
    }

    /**
     * @brief : 턴 세기
     * @param Method : 턴에 추가할 함수
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
        // 유닛이 없다면 정적인 타일이란 뜻
        if (tile._unitObj == null)
        {
            objectNameTxt.text = tile._name;
            objectDescTxt.text = tile._desc;
            NetworkMng.getInstance._soundGM.tileClick();
            return;
        }
        objectNameTxt.text = tile._unitObj._name;
        objectDescTxt.text = tile._unitObj._desc;
        hpText.text = tile._unitObj._hp + "";
        NetworkMng.getInstance._soundGM.unitClick(UNIT.WORKER);
        //damageText.text = tile._unitObj._damage + "";


        // 행동을 가진 오브젝트는 actList 를 뿌려줘야 함
        // 1. _unitObj 로 부터 해당 유닛이 가진 행동의 량을 가져옴
        for (int i = 0; i < tile._unitObj._activity.Count; i++)
        {
            // 2. 그만큼 actList 를 active 함
            actList[i].gameObject.SetActive(true);
            UnityEngine.UI.Text[] childsTxt = actList[i].GetComponentsInChildren<UnityEngine.UI.Text>();
            try
            {
                // 3. actList 의 내용들을 변경해 줘야함
                checkActivity(tile._unitObj._activity[i], actList[i], childsTxt[0], childsTxt[1]);
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
            default:
                break;
        }
    }

    // 레이케스트 위한 변수
    Vector2 pos;
    Ray2D ray;
    public RaycastHit2D hit;
    // 클릭시 Tile.cs 받아오는 곳
    public Tile GetTileCs = null;

    /**
    * @brief 레이케스트 레이저 생성 및 hit 리턴
    */
    public void MouseLaycast()
    {
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ray = new Ray2D(pos, Vector2.zero);

        hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            GetTileCs = hit.collider.gameObject.GetComponent<Tile>();
            Debug.Log(GetTileCs._code);
        }
    }

    // 유닛 움직일때 필요한 변수들
    public Tile NowTile = null;
    GameObject TileGams = null;
    //범위 타일 스크립트 변수
    [SerializeField]
    public RangeScrp RangeSc = null;

    [SerializeField]
    private const float fUnitSpeed = 3.0f;
    [SerializeField]
    private float TileDistance = 0.0f;

    public bool bUnitMoveCheck = false;

    /**
     * @brief RaycastHit2D로 타일 정보 알아와서 캐릭터 이동 계산
     * @param UnitGame      유닛 캐릭터 GameObject
     * @param unitClass     캐릭터 Unit 스크립트
     */
    public void UnitClickMove(GameObject unitGame, Unit unitscrp)
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseLaycast();
            if (hit.collider != null)
            {
                if (hit.collider.tag.Equals("Tile"))
                {
                    if (NowTile != null)
                    {
                        NowTile._unitObj = null;        // 떠났을때 지워짐
                    }

                    GetTileCs = hit.collider.gameObject.GetComponent<Tile>();
                    NowTile = GetTileCs;

                    TileGams = hit.collider.gameObject;
                    TileDistance = Vector2.Distance(unitGame.transform.localPosition, TileGams.transform.localPosition);                  // 타일이 눌렸을때 캐릭터와 클릭한 타일간 거리 계산
                    RangeSc.RangeTileReset();                                                                                             // 범위 타일 위치 초기화
                    if (!bUnitMoveCheck)
                    {
                        bUnitMoveCheck = true;
                    }
                }
            }
        }

        if (bUnitMoveCheck)
        {
            NowTile._unitObj = unitscrp;    // 세팅
            if (Vector2.Distance(unitGame.transform.localPosition, TileGams.transform.localPosition) >= 0.01f && TileDistance <= 6f)      // 캐릭터와 타일간 거리가 1.5 * a 이하일시 움직일수 있음 (거리 한칸당 1.24?정도 되드라)
            {
                unitGame.transform.localPosition = Vector2.Lerp(unitGame.transform.localPosition, GetTileCs.GetTileVec2, fUnitSpeed * Time.deltaTime);        //타일 간 부드러운 이동
                if (unitGame.transform.localPosition.x < TileGams.transform.localPosition.x)                                                                 //가는 방향 회전 오른쪽
                {
                    unitGame.transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
                }
                else if (unitGame.transform.localPosition.x == TileGams.transform.localPosition.x)                                                           //방향 변동 X
                {

                }
                else
                {
                    unitGame.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));                                                                 //가는 방향 회전 왼쪽
                }
            }
            else if (TileDistance <= 6f)                                                                                                            //남은 거리가 좁아지면 타일 위치로 자동 이동
            {
                unitGame.transform.localPosition = GetTileCs.GetTileVec2;
                bUnitMoveCheck = false;
            }
        }
    }
}