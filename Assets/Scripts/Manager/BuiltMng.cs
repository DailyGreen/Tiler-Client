using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuiltMng : MonoBehaviour
{
    public ACTIVITY act = ACTIVITY.NONE;

    public GameObject[] unitobj = null;

    [SerializeField]
    private GameObject AirDropobj = null;

    private int nAirDropCount = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && act != ACTIVITY.ACTING && GameMng.I._UnitGM.act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (act)
            {
                case ACTIVITY.WORKER_UNIT_CREATE:
                    //GameMng.I._hextile.GetCell(GameMng.I.CastlePosX, GameMng.I.CastlePosZ)._builtObj._anim.SetTrigger("isMaking");
                    CreateUnit(GameMng.I.WORKER_COST, (int)UNIT.FOREST_WORKER + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
                case ACTIVITY.SOLDIER_0_UNIT_CREATE:
                    CreateUnit(GameMng.I.SOLDIER_0_COST, (int)UNIT.FOREST_SOLDIER_0 + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
                case ACTIVITY.SOLDIER_1_UNIT_CREATE:
                    CreateUnit(GameMng.I.SOLDIER_1_COST, (int)UNIT.FOREST_SOLDIER_1 + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
                case ACTIVITY.SOLDIER_2_UNIT_CREATE:
                    CreateUnit(GameMng.I.SOLDIER_2_COST, (int)UNIT.FOREST_SOLDIER_2 + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
                case ACTIVITY.WITCH_0_UNIT_CREATE:
                    CreateUnit(GameMng.I.WITCH_0_COST, (int)UNIT.FOREST_WITCH_0 + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
                case ACTIVITY.WITCH_1_UNIT_CREATE:
                    CreateUnit(GameMng.I.WITCH_1_COST, (int)UNIT.FOREST_WITCH_1 + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
            }
            GameMng.I._range.SelectTileSetting(true);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            CreateAirDrop();
        }
    }

    /**
     * @brief 유닛을 생성함 (클라 전용)
     * @param cost 사용된 코스트
     * @param index 유닛 코드
     */
    public void CreateUnit(int cost, int index)
    {
        GameMng.I.mouseRaycast(true);                       //캐릭터 정보와 타일 정보를 알아와야해서 false에서 true로 변경
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            if (GameMng.I._gold >= cost)
            {
                GameMng.I.minGold(cost);
                if (index != (int)UNIT.FOREST_WORKER && index != (int)UNIT.SEA_WORKER && index != (int)UNIT.DESERT_WORKER)      // 일꾼 유닛들을 제외하고 유닛 생성중일때 해당 건물 행동 불가 상태 적용
                    GameMng.I.selectedTile._builtObj._bActAccess = false;

                GameMng.I.selectedTile._builtObj._anim.SetTrigger("isMaking");

                GameObject Child = Instantiate(unitobj[index - 300], GameMng.I.targetTile.transform) as GameObject;                 // enum 값 - 300
                Child.transform.parent = transform.parent;
                GameMng.I.targetTile._unitObj = Child.GetComponent<Unit>();
                GameMng.I.targetTile._unitObj.SaveX = GameMng.I.selectedTile.PosX;              // 생성하는 유닛에게 있는 위치 저장 변수에 해당 건물 위치값을 저장해줌
                GameMng.I.targetTile._unitObj.SaveY = GameMng.I.selectedTile.PosZ;
                GameMng.I.targetTile._code = index;
                GameMng.I.targetTile._unitObj._uniqueNumber = NetworkMng.getInstance.uniqueNumber;


                if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)                  // 군사기지에서 유닛 생성 시
                {
                    MillitaryBase SaveUnitData = GameMng.I.selectedTile._builtObj.GetComponent<MillitaryBase>();
                    SaveUnitData.CreatingUnitobj = GameMng.I.targetTile._unitObj.gameObject;
                    SaveUnitData.CreatingUnitX = GameMng.I.targetTile.PosX;
                    SaveUnitData.CreatingUnitY = GameMng.I.targetTile.PosZ;
                }

                act = ACTIVITY.NONE;

                NetworkMng.getInstance.SendMsg(string.Format("CREATE_UNIT:{0}:{1}:{2}:{3}:{4}:{5}", GameMng.I.targetTile.PosX, GameMng.I.targetTile.PosZ, index, NetworkMng.getInstance.uniqueNumber, GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosZ));

                GameMng.I.cleanActList();
                GameMng.I._range.rangeTileReset();
                GameMng.I.cleanSelected();

                NetworkMng.getInstance.SendMsg("TURN");
            }
        }
        else                                     // 범위가 아닌 다른 곳을 누름
        {
            act = ACTIVITY.NONE;
            GameMng.I.selectedTile = GameMng.I.targetTile;
            GameMng.I.targetTile = null;
            GameMng.I._range.rangeTileReset();
        }
    }

    /**
     * @brief 유닛을 생성함 (서버에서 클라로 정보를 보낼때 호출됨)
     * @param posX 생성할 X 위치
     * @param posY 생성할 Y 위치
     * @param index 유닛 코드
     * @param uniqueNumber 생성자
     * @param byX 생성을 지시한 X 위치
     * @param byY 생성을 지시한 Y 위치
     */
    public void CreateUnit(int posX, int posY, int index, int uniqueNumber, int byX, int byY)
    {
        GameObject Child = Instantiate(unitobj[index - 300], GameMng.I._hextile.GetCell(posX, posY).transform) as GameObject;
        Child.transform.parent = transform.parent;
        GameMng.I._hextile.GetCell(posX, posY)._unitObj = Child.GetComponent<Unit>();
        GameMng.I._hextile.GetCell(posX, posY)._code = GameMng.I._hextile.GetCell(posX, posY)._unitObj._code;
        GameMng.I._hextile.GetCell(posX, posY)._unitObj._uniqueNumber = uniqueNumber;
        GameMng.I._hextile.GetCell(byX, byY)._builtObj._bActAccess = false;
        GameMng.I._hextile.GetCell(posX, posY)._unitObj.SaveX = byX;
        GameMng.I._hextile.GetCell(posX, posY)._unitObj.SaveY = byY;

        GameMng.I.addActMessage(string.Format("{0}님이 일꾼을 생성하고 있습니다.", GameMng.I.getUserName(uniqueNumber)), posX, posY);

        if (GameMng.I._hextile.GetCell(byX, byY)._builtObj._code == (int)BUILT.MILLITARY_BASE)
        {
            MillitaryBase SaveData = GameMng.I._hextile.GetCell(byX, byY)._builtObj.GetComponent<MillitaryBase>();
            SaveData.CreatingUnitobj = GameMng.I._hextile.GetCell(posX, posY)._unitObj.gameObject;
            SaveData.GetComponent<MillitaryBase>().CreatingUnitX = posX;
            SaveData.GetComponent<MillitaryBase>().CreatingUnitY = posY;
        }
    }

    /**
     * @brief 보급 생성
     */
    public void CreateAirDrop()
    {
        int nPosX, nPosY;
        nPosX = Random.Range(0, GameMng.I.GetMapWidth);
        nPosY = Random.Range(0, GameMng.I.GetMapHeight);

        if (GameMng.I._hextile.GetCell(nPosX, nPosY)._builtObj == null && GameMng.I._hextile.GetCell(nPosX, nPosY)._unitObj == null && GameMng.I._hextile.GetCell(nPosX, nPosY)._code < (int)TILE.CAN_MOVE)
        {
            GameObject Child = Instantiate(AirDropobj, GameMng.I._hextile.GetCell(nPosX, nPosY).transform) as GameObject;
            GameMng.I._hextile.GetCell(nPosX, nPosY)._code = (int)TILE.CAN_MOVE;
            GameMng.I._hextile.GetCell(nPosX, nPosY)._builtObj = Child.GetComponent<AirDrop>();
        }
        else
        {
            if (nAirDropCount < 5)
            {
                nAirDropCount++;
                CreateAirDrop();
            }
            else
                nAirDropCount = 0;
        }
    }

    /**
     * @brief 건물 파괴될때 호출됨 로컬
     */
    public void DestroyBuilt()
    {
        if (GameMng.I.selectedTile == null) { act = ACTIVITY.NONE; return; }

        NetworkMng.getInstance.SendMsg(string.Format("DESTROY_BUILT:{0}:{1}", GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosZ));

        if (GameMng.I.selectedTile._builtObj._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.AttackrangeTileReset();
            GameMng.I.RemoveDelegate(GameMng.I.selectedTile._builtObj.GetComponent<Turret>().Attack);
        }

        GameMng.I.selectedTile._builtObj.DestroyMyself();

        act = ACTIVITY.NONE;

        GameMng.I.selectedTile._builtObj = null;

        GameMng.I._hextile.TilecodeClear(GameMng.I.selectedTile);

        GameMng.I._range.SelectTileSetting(true);

        GameMng.I.setMainInterface();

        GameMng.I.cleanActList();
        GameMng.I.cleanSelected();

        NetworkMng.getInstance.SendMsg("TURN");
    }

    /**
     * @brief 건물 파괴될때 호출됨 서버 받아옴
     */
    public void DestroyBuilt(int posX, int posY)
    {
        GameMng.I._hextile.GetCell(posX, posY)._builtObj.DestroyMyself();
        GameMng.I._hextile.GetCell(posX, posY)._builtObj = null;
        GameMng.I._hextile.GetCell(posX, posY)._code = (int)TILE.GRASS;
    }
}
