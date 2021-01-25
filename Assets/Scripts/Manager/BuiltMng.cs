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
                    GameMng.I._hextile.GetCell(GameMng.I.CastlePosX, GameMng.I.CastlePosZ)._builtObj._anim.SetTrigger("isMaking");
                    CreateUnit(Forest_Worker.cost, (int)UNIT.FOREST_WORKER + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
                case ACTIVITY.SOLDIER_0_UNIT_CREATE:
                    CreateUnit(Forest_Soldier_0.cost, (int)UNIT.FOREST_SOLDIER_0 + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
                case ACTIVITY.SOLDIER_1_UNIT_CREATE:
                    CreateUnit(Forest_Soldier_1.cost, (int)UNIT.FOREST_SOLDIER_1 + (int)(NetworkMng.getInstance.myTribe) * 6);
                    break;
            }
            GameMng.I._range.SelectTileSetting(true);
        }

        if (Input.GetMouseButtonDown(0) && GameMng.I._UnitGM.act == ACTIVITY.NONE && act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            GameMng.I._range.AttackrangeTileReset();                                                     //Ŭ���� �ͷ� ���� ���� �ʱ�ȭ
            GameMng.I.mouseRaycast();
            if (GameMng.I.selectedTile)
            {
                if (GameMng.I.selectedTile._builtObj != null)
                {
                    if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING && NetworkMng.getInstance.uniqueNumber.Equals(GameMng.I.selectedTile._builtObj._uniqueNumber))
                    {
                        GameMng.I._range.attackRange(GameMng.I.selectedTile._builtObj.GetComponent<Turret>()._attackdistance);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            CreateAirDrop();
        }
    }

    /**
     * @brief ������ ������ (Ŭ�� ����)
     * @param cost ���� �ڽ�Ʈ
     * @param index ���� �ڵ�
     */
    public void CreateUnit(int cost, int index)
    {

        GameMng.I.minGold(3);       // TODO : �ڽ�Ʈ�� ����
        GameMng.I.mouseRaycast(true);                       //ĳ���� ������ Ÿ�� ������ �˾ƿ;��ؼ� false���� true�� ����
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            if (GameMng.I._gold >= cost)
            {
                if (index > (int)UNIT.FOREST_WORKER && (int)UNIT.SEA_WORKER > index && index > (int)UNIT.DESERT_WORKER)
                {
                    GameMng.I.selectedTile._builtObj._bActAccess = false;
                }
                GameObject Child = Instantiate(unitobj[index - 300], GameMng.I.targetTile.transform) as GameObject;                 // enum �� - 300
                Child.transform.parent = transform.parent;
                GameMng.I.targetTile._unitObj = Child.GetComponent<Unit>();
                GameMng.I.targetTile._unitObj.gameObject.GetComponent<Unit>().SaveX = GameMng.I.selectedTile.PosX;              // �����ϴ� ���ֿ��� �ִ� ��ġ ���� ������ �ش� �ǹ� ��ġ���� ��������
                GameMng.I.targetTile._unitObj.gameObject.GetComponent<Unit>().SaveY = GameMng.I.selectedTile.PosZ;
                GameMng.I.targetTile._code = index;       // ������ Awake��
                GameMng.I.targetTile._unitObj._uniqueNumber = NetworkMng.getInstance.uniqueNumber;
                act = ACTIVITY.NONE;

                NetworkMng.getInstance.SendMsg(string.Format("CREATE_UNIT:{0}:{1}:{2}:{3}:{4}:{5}", GameMng.I.targetTile.PosX, GameMng.I.targetTile.PosZ, index, NetworkMng.getInstance.uniqueNumber, GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosZ));

                GameMng.I.cleanActList();
                GameMng.I.cleanSelected();

                NetworkMng.getInstance.SendMsg("TURN");
            }
        }
        else                                     // ������ �ƴ� �ٸ� ���� ����
        {
            act = ACTIVITY.NONE;
            GameMng.I.selectedTile = GameMng.I.targetTile;
            GameMng.I.targetTile = null;
            GameMng.I._range.rangeTileReset();
        }
    }

    /**
     * @brief ������ ������ (�������� Ŭ��� ������ ������ ȣ���)
     * @param posX ������ X ��ġ
     * @param posY ������ Y ��ġ
     * @param index ���� �ڵ�
     * @param uniqueNumber ������
     * @param byX ������ ������ X ��ġ
     * @param byY ������ ������ Y ��ġ
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
    }

    /**
     * @brief ���� ����
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
     * @brief �ǹ� �ı��ɶ� ȣ��� ����
     */
    public void DestroyBuilt()
    {
        if (GameMng.I.selectedTile == null) { act = ACTIVITY.NONE; return; }

        Debug.Log(GameMng.I.selectedTile);
        NetworkMng.getInstance.SendMsg(string.Format("DESTROY_BUILT:{0}:{1}", GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosZ));
        if (GameMng.I.selectedTile._builtObj._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.AttackrangeTileReset();
            GameMng.I.RemoveDelegate(GameMng.I.selectedTile._builtObj.GetComponent<Turret>().Attack);
        }
        GameMng.I.selectedTile._builtObj.DestroyMyself();
        //Destroy(GameMng.I.selectedTile._builtObj.gameObject);
        act = ACTIVITY.NONE;
        GameMng.I.selectedTile._builtObj = null;
        Debug.Log("���� �����ؾ���!!!!!");
        GameMng.I._hextile.TilecodeClear(GameMng.I.selectedTile);
        GameMng.I._range.SelectTileSetting(true);
        GameMng.I.setMainInterface();
        GameMng.I.cleanActList();
        GameMng.I.cleanSelected();
        NetworkMng.getInstance.SendMsg("TURN");
    }

    /**
     * @brief �ǹ� �ı��ɶ� ȣ��� ���� �޾ƿ�
     */
    public void DestroyBuilt(int posX, int posY)
    {
        GameMng.I._hextile.GetCell(posX, posY)._builtObj.DestroyMyself();
        GameMng.I._hextile.GetCell(posX, posY)._builtObj = null;
        GameMng.I._hextile.GetCell(posX, posY)._code = (int)TILE.GRASS;
    }
}
