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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && act != ACTIVITY.ACTING && GameMng.I._UnitGM.act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (act)
            {
                case ACTIVITY.WORKER_UNIT_CREATE:
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
    }

    /**
     * @brief ������ ������ (Ŭ�� ����)
     * @param cost ���� �ڽ�Ʈ
     * @param index ���� �ڵ�
     */
    public void CreateUnit(int cost, int index)
    {
        GameMng.I.mouseRaycast(true);                       //ĳ���� ������ Ÿ�� ������ �˾ƿ;��ؼ� false���� true�� ����
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            if (GameMng.I._gold >= cost)
            {
                GameMng.I.minGold(cost);
                if (index != (int)UNIT.FOREST_WORKER && index != (int)UNIT.SEA_WORKER && index != (int)UNIT.DESERT_WORKER)      // �ϲ� ���ֵ��� �����ϰ� ���� �������϶� �ش� �ǹ� �ൿ �Ұ� ���� ����
                    GameMng.I.selectedTile._builtObj._bActAccess = false;

                GameMng.I.selectedTile._builtObj._anim.SetTrigger("isMaking");

                GameObject Child = Instantiate(unitobj[index - 300], GameMng.I.targetTile.transform) as GameObject;                 // enum �� - 300
                Child.transform.parent = transform.parent;
                GameMng.I.targetTile._unitObj = Child.GetComponent<Unit>();
                GameMng.I.targetTile._unitObj.SaveX = GameMng.I.selectedTile.PosX;              // �����ϴ� ���ֿ��� �ִ� ��ġ ���� ������ �ش� �ǹ� ��ġ���� ��������
                GameMng.I.targetTile._unitObj.SaveY = GameMng.I.selectedTile.PosZ;
                GameMng.I.targetTile._code = index;
                GameMng.I.targetTile._unitObj._uniqueNumber = NetworkMng.getInstance.uniqueNumber;


                if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)                  // ����������� ���� ���� ��
                {
                    MillitaryBase SaveUnitData = GameMng.I.selectedTile._builtObj.GetComponent<MillitaryBase>();
                    SaveUnitData.CreatingUnitobj = GameMng.I.targetTile._unitObj.gameObject;
                    SaveUnitData.CreatingUnitX = GameMng.I.targetTile.PosX;
                    SaveUnitData.CreatingUnitY = GameMng.I.targetTile.PosZ;
                }

                act = ACTIVITY.NONE;

                NetworkMng.getInstance.SendMsg(string.Format("CREATE_UNIT:{0}:{1}:{2}:{3}:{4}:{5}", GameMng.I.targetTile.PosX, GameMng.I.targetTile.PosZ, index, NetworkMng.getInstance.uniqueNumber, GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosZ));

                GameMng.I.addLogMessage(NetworkMng.getInstance.nickName, "������ �����ϰ� �ֽ��ϴ�");

                GameMng.I.cleanActList();
                GameMng.I._range.rangeTileReset();
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
        Debug.Log(GameMng.I._hextile.GetCell(byX, byY)._builtObj);
        GameMng.I._hextile.GetCell(byX, byY)._builtObj._bActAccess = false;
        GameMng.I._hextile.GetCell(posX, posY)._unitObj.SaveX = byX;
        GameMng.I._hextile.GetCell(posX, posY)._unitObj.SaveY = byY;

        GameMng.I.addActMessage(string.Format("{0}���� �ϲ��� �����ϰ� �ֽ��ϴ�.", GameMng.I.getUserName(uniqueNumber)), posX, posY);

        GameMng.I.addLogMessage(GameMng.I.getUserName(uniqueNumber), "�ϲ��� �����ϰ� �ֽ��ϴ�.");

        if (GameMng.I._hextile.GetCell(byX, byY)._builtObj._code == (int)BUILT.MILLITARY_BASE)
        {
            MillitaryBase SaveData = GameMng.I._hextile.GetCell(byX, byY)._builtObj.GetComponent<MillitaryBase>();
            SaveData.CreatingUnitobj = GameMng.I._hextile.GetCell(posX, posY)._unitObj.gameObject;
            SaveData.GetComponent<MillitaryBase>().CreatingUnitX = posX;
            SaveData.GetComponent<MillitaryBase>().CreatingUnitY = posY;
        }
    }

    /**
     * @brief ���޿� ���� ������ ������ ����
     */
    public void CreateAirDrop(int index)
    {
        if (NetworkMng.getInstance.roomOwner)
        {
            if (GameMng.I._hextile.cells[index]._builtObj == null && GameMng.I._hextile.cells[index]._unitObj == null && GameMng.I._hextile.cells[index]._code < (int)TILE.CAN_MOVE)
            {
                NetworkMng.getInstance.SendMsg(string.Format("CREATE_AIRDROP:{0}:{1}", index, (int)TILE.CAN_MOVE));
                GameObject Child = Instantiate(AirDropobj, GameMng.I._hextile.cells[index].transform) as GameObject;
                GameMng.I._hextile.cells[index]._code = (int)TILE.CAN_MOVE;
                GameMng.I._hextile.cells[index]._builtObj = Child.GetComponent<AirDrop>();
            }
            else
            {
                CreateAirDrop(Random.Range(0, GameMng.I._hextile.cells.Length));
            }
        }
    }

    /**
     * @brief �������� ���޿� ���� ���� �޾ƿ��� �Լ�
     */
    public void CreateAirDrop(int index, int code)
    {
        GameObject Child = Instantiate(AirDropobj, GameMng.I._hextile.cells[index].transform) as GameObject;
        GameMng.I._hextile.cells[index]._code = (int)TILE.CAN_MOVE;
        GameMng.I._hextile.cells[index]._builtObj = Child.GetComponent<AirDrop>();

        GameMng.I.addActMessage(
            "��    ����ǰ�� ���������ϴ�!    ��",
            GameMng.I._hextile.cells[index].PosX,
            GameMng.I._hextile.cells[index].PosZ);
        GameMng.I.addLogMessage("�ý���", "����ǰ�� ���������ϴ�.");
    }

    /**
     * @brief �ǹ� �ı��ɶ� ȣ��� ����
     */
    public void DestroyBuilt()
    {
        if (GameMng.I.selectedTile == null) { act = ACTIVITY.NONE; return; }

        GameMng.I.addLogMessage(NetworkMng.getInstance.nickName, "�ǹ��� �ı��߽��ϴ�.");
        NetworkMng.getInstance.SendMsg(string.Format("DESTROY_BUILT:{0}:{1}", GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosZ));

        if (GameMng.I.selectedTile._builtObj._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.AttackrangeTileReset();
            GameMng.I.RemoveDelegate(GameMng.I.selectedTile._builtObj.GetComponent<Turret>().Attack);
        }

        switch (GameMng.I.selectedTile._builtObj._code)
        {
            case (int)BUILT.MINE:
                GameMng.I.addGold((GameMng.I.MINE_COST / 4) * 3);
                break;
            case (int)BUILT.FARM:
                GameMng.I.addGold((GameMng.I.FARM_COST / 4) * 3);
                break;
            case (int)BUILT.ATTACK_BUILDING:
                GameMng.I.addGold((GameMng.I.TURRET_COST / 4) * 3);
                break;
            case (int)BUILT.MILLITARY_BASE:
                GameMng.I.addGold((GameMng.I.MILITARYBASE_COST / 4) * 3);
                break;
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
     * @brief �ǹ� �ı��ɶ� ȣ��� ���� �޾ƿ�
     */
    public void DestroyBuilt(int posX, int posY)
    {
        GameMng.I._hextile.GetCell(posX, posY)._builtObj.DestroyMyself();
        GameMng.I._hextile.GetCell(posX, posY)._builtObj = null;
        GameMng.I._hextile.GetCell(posX, posY)._code = (int)TILE.GRASS;
    }
}
