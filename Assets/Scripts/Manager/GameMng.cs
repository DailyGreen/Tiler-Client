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

    // 레이케스트 위한 변수
    Vector2 pos;
    Ray2D ray;
    RaycastHit2D hit;
    public Tile GetTileCs = null;

    private const int mapWidth = 20;
    private const int mapHeight = 5;
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
        _gold = 0;
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
     * @param 으로 클릭한 오브젝트가 뭔지 알려줘야됨
     */
    public void clickTile(Tile tile)
    {
        // 유닛이 없다면 정적인 타일이란 뜻
        if (tile._unitObj == null)
        {
            objectNameTxt.text = tile._name;
            objectDescTxt.text = tile._desc;
            return;
        }
        objectNameTxt.text = tile._unitObj._name;
        objectDescTxt.text = tile._unitObj._desc;
        hpText.text = tile._unitObj._hp + "";
    }
    public Tile[,] mapTile = new Tile[mapHeight, mapWidth];

    /**
    * @brief 레이케스트 레이저 생성 및 hit 리턴
    */
    public RaycastHit2D MouseLaycast()
    {
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ray = new Ray2D(pos, Vector2.zero);
        
        hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider.tag.Equals("Tile"))
            GetTileCs = hit.collider.gameObject.GetComponent<Tile>();

        return hit;
    }
}