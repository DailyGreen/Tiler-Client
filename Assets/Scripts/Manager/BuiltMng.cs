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
                    CreateUnit((int)UNIT.FOREST_WORKER);
                    break;
                case ACTIVITY.ATTACK_UNIT_CREATE:
                    CreateUnit((int)UNIT.FORSET_SOILDER);
                    break;
            }
            GameMng.I._range.SelectTileSetting(true);
        }


        if (Input.GetMouseButtonDown(0) && GameMng.I._UnitGM.act == ACTIVITY.NONE && act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            GameMng.I._range.AttackrangeTileReset();                                                     //Ŭ���� �ͷ� ���� ���� �ʱ�ȭ
            GameMng.I.mouseRaycast();
            if (GameMng.I.selectedTile)
                if (GameMng.I.selectedTile._builtObj != null)
                {
                    if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING)
                    {
                        GameMng.I.selectedTile._builtObj.GetComponent<Turret>().Attack();
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
    public void CreateUnit(/*int cost,*/ int index)
    {
        GameMng.I.minGold(3);       // TODO : �ڽ�Ʈ�� ����
        GameMng.I.mouseRaycast(true);                       //ĳ���� ������ Ÿ�� ������ �˾ƿ;��ؼ� false���� true�� ����
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            GameObject Child = Instantiate(unitobj[index - 300], GameMng.I.targetTile.transform) as GameObject;                 // enum �� - 100
            Child.transform.parent = transform.parent;
            GameMng.I.targetTile._unitObj = Child.GetComponent<Unit>();
            GameMng.I.targetTile._code = index;       // ������ Awake��
            GameMng.I.targetTile._unitObj._uniqueNumber = NetworkMng.getInstance.uniqueNumber;
            GameMng.I._range.rangeTileReset();
            act = ACTIVITY.ACTING;

            NetworkMng.getInstance.SendMsg(string.Format("CREATE_UNIT:{0}:{1}:{2}:{3}", GameMng.I.targetTile.PosX, GameMng.I.targetTile.PosY, index, NetworkMng.getInstance.uniqueNumber));

            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();
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
     */
    public void CreateUnit(int posX, int posY, int index, int uniqueNumber)
    {
        GameObject Child = Instantiate(unitobj[index - 300], GameMng.I.mapTile[posY, posX].transform) as GameObject;
        Child.transform.parent = transform.parent;
        GameMng.I.mapTile[posY, posX]._unitObj = Child.GetComponent<Unit>();
        GameMng.I.mapTile[posY, posX]._code = GameMng.I.mapTile[posY, posX]._unitObj._code;
        GameMng.I.mapTile[posY, posX]._unitObj._uniqueNumber = uniqueNumber;
    }

    /**
     * @brief ���� ����
     */
    public void CreateAirDrop()
    {
        Debug.Log(nAirDropCount);
        int nPosX, nPosY;
        nPosX = Random.Range(0, GameMng.I.GetMapWidth);
        nPosY = Random.Range(0, GameMng.I.GetMapHeight);
        if (GameMng.I.mapTile[nPosY, nPosX]._builtObj == null && GameMng.I.mapTile[nPosY, nPosX]._unitObj == null && GameMng.I.mapTile[nPosY, nPosX]._code < (int)TILE.CAN_MOVE)
        {
            GameObject Child = Instantiate(AirDropobj, GameMng.I.mapTile[nPosY, nPosX].transform) as GameObject;
            GameMng.I.mapTile[nPosY, nPosX]._code = (int)TILE.CAN_MOVE;
            GameMng.I.mapTile[nPosY, nPosX]._builtObj = Child.GetComponent<AirDrop>();
        }
        else
        {
            if (nAirDropCount < 5)
            {
                Debug.Log("��ġ �� ����");
                nAirDropCount++;
                CreateAirDrop();
            }
            else
                nAirDropCount = 0;
        }
        Debug.Log(nPosY + " , " + nPosX);

    }

    /**
     * @brief �ǹ� �ı��ɶ� ȣ���
     */
    public void DestroyBuilt()
    {
        NetworkMng.getInstance.SendMsg(string.Format("DESTROY_BUILT:{0}:{1}", GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY));
        Destroy(GameMng.I.selectedTile._builtObj.gameObject);
        if (GameMng.I.selectedTile._builtObj._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.AttackrangeTileReset();
        }
        act = ACTIVITY.NONE;
        GameMng.I.selectedTile._builtObj = null;
        Debug.Log("���� �����ؾ���!!!!!");
        GameMng.I.selectedTile._code = (int)TILE.GRASS;                                                             // ���߿� ���� Ÿ�� �˾ƿ��¹� ��������
        GameMng.I._range.SelectTileSetting(true);
        GameMng.I.cleanActList();
        GameMng.I.cleanSelected();
    }

    /**
     * @brief �ǹ� �ı��ɶ� ȣ���
     */
    public void DestroyBuilt(int posX, int posY)
    {
        Destroy(GameMng.I.mapTile[posY, posX]._builtObj.gameObject);
        GameMng.I.mapTile[posY, posX]._builtObj = null;
        GameMng.I.mapTile[posY, posX]._code = (int)TILE.GRASS;
    }
}
