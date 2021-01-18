using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitMng : MonoBehaviour
{
    public HexTileCreate Hc = null;

    public ACTIVITY act = ACTIVITY.NONE;

    public Worker worker = null;

    public GameObject[] builtObj = null;

    private void Start()
    {
        act = ACTIVITY.NONE;
    }

    void Update()
    {
        //Debug.Log(Vector2.Distance(GameMng.I.mapTile[GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY].transform.localPosition,
        //    GameMng.I.mapTile[GameMng.I.targetTile.PosX, GameMng.I.targetTile.PosY].transform.localPosition));
        if (Input.GetMouseButtonDown(0) && act != ACTIVITY.ACTING && GameMng.I._BuiltGM.act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (act)
            {
                case ACTIVITY.MOVE:
                    CheckMove();
                    break;
                case ACTIVITY.BUILD_MINE:
                    Building(Mine.cost, (int)BUILT.MINE);
                    break;
                case ACTIVITY.BUILD_FARM:
                    Building(Farm.cost, (int)BUILT.FARM);
                    break;
                case ACTIVITY.BUILD_ATTACK_BUILDING:
                    Building(Turret.cost, (int)BUILT.ATTACK_BUILDING);
                    break;
                case ACTIVITY.BUILD_MILLITARY_BASE:
                    Building(MillitaryBase.cost, (int)BUILT.MILLITARY_BASE);
                    break;
                case ACTIVITY.ATTACK:
                    UnitAttack();
                    break;
            }
            GameMng.I._range.SelectTileSetting(true);
        }
        else if (Input.GetMouseButtonUp(1) && act != ACTIVITY.ACTING)
        {
            act = ACTIVITY.NONE;
            GameMng.I._BuiltGM.act = ACTIVITY.NONE;
            GameMng.I._range.rangeTileReset();
            GameMng.I._range.AttackrangeTileReset();                                                     //�߰�
            GameMng.I._range.SelectTileSetting(true);
            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();
        }
    }

    /**
     * @brief ���� �̵� �˻� (����� �ùٸ���)
     */
    public void CheckMove()
    {
        GameMng.I.mouseRaycast(true);

        if (GameMng.I.hit.collider != null)
        {
            GameMng.I.cleanActList();
            GameMng.I._range.rangeTileReset();  // ���� Ÿ�� ��ġ �ʱ�ȭ
            GameMng.I.distanceOfTiles = Vector2.Distance(GameMng.I.selectedTile._unitObj.transform.position, GameMng.I.targetTile.transform.position);
            if (GameMng.I.hit.collider.tag.Equals("Tile") && GameMng.I.distanceOfTiles <= 1.5f && Tile.isEmptyTile(GameMng.I.targetTile))
            {
                act = ACTIVITY.ACTING;
                reversalUnit(GameMng.I.selectedTile._unitObj.transform, GameMng.I.targetTile.transform);
                GameMng.I.selectedTile._unitObj._anim.SetTrigger("isRunning");
                NetworkMng.getInstance.SendMsg(string.Format("MOVE_UNIT:{0}:{1}:{2}:{3}", GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY, GameMng.I.targetTile.PosX, GameMng.I.targetTile.PosY));
                StartCoroutine("Moving");
            }
            else                                     // ������ �ƴ� �ٸ� ���� ����
            {
                Debug.Log("else�� ����");
                act = ACTIVITY.NONE;
                //GameMng.I.selectedTile = GameMng.I.targetTile;
                //GameMng.I.targetTile = null;
            }
        }
    }

    /**
     * @brief ���� �̵� (Ŭ�� ����)
     */
    IEnumerator Moving()
    {
        if (Vector2.Distance(GameMng.I.selectedTile._unitObj.transform.localPosition, GameMng.I.targetTile.transform.localPosition) >= 0.01f
            && GameMng.I.distanceOfTiles <= 1.5f) // ĳ���Ϳ� Ÿ�ϰ� �Ÿ��� 1.5 * a �����Ͻ� �����ϼ� ���� (�Ÿ� ��ĭ�� 1.24?���� �ǵ��)
        {
            GameMng.I.selectedTile._unitObj.transform.localPosition = Vector2.Lerp(GameMng.I.selectedTile._unitObj.transform.localPosition, GameMng.I.targetTile.transform.localPosition, GameMng.I.unitSpeed * Time.deltaTime);        //Ÿ�� �� �ε巯�� �̵�
            yield return null;
            StartCoroutine("Moving");
        }
        else if (GameMng.I.distanceOfTiles >= 0.1f)     // ���� �Ÿ��� �������� Ÿ�� ��ġ�� �ڵ� �̵�
        {
            act = ACTIVITY.NONE;

            GameMng.I.selectedTile._unitObj.transform.localPosition = GameMng.I.targetTile.transform.localPosition;
            GameMng.I.targetTile._unitObj = GameMng.I.selectedTile._unitObj;
            GameMng.I.targetTile._code = GameMng.I.selectedTile._unitObj._code;
            GameMng.I.selectedTile._unitObj = null;
            //GameMng.I.selectedTile._code = (int)TILE.CAN_MOVE - 1;
            Hc.TilecodeClear(GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosY);
            NetworkMng.getInstance.SendMsg("TURN");
        }
    }

    /**
     * @brief ���� �̵� (������� �̵�, ���� ����)
     * @param posX ��� x ��ǥ
     * @param posY ��� y ��ǥ
     * @param toX �̵��� x ��ǥ
     * @param toY �̵��� y ��ǥ
     */
    public IEnumerator MovingUnit(int posX, int posY, int toX, int toY)
    {
        bool isRun = true;
        reversalUnit(GameMng.I.mapTile[posY, posX]._unitObj.transform, GameMng.I.mapTile[toY, toX].transform);
        GameMng.I.mapTile[posY, posX]._unitObj._anim.SetTrigger("isRunning");
        GameMng.I.mapTile[toY, toX]._code = GameMng.I.mapTile[posY, posX]._code;
        Hc.TilecodeClear(posX, posY);

        GameMng.I.addActMessage(string.Format("{0}���� ������ �̵��߽��ϴ�.", GameMng.I.mapTile[posY, posX]._unitObj._uniqueNumber), toX, toY);

        while (isRun)
        {
            if (Vector2.Distance(GameMng.I.mapTile[posY, posX]._unitObj.transform.localPosition, GameMng.I.mapTile[toY, toX].transform.localPosition) >= 0.01f)
            {
                GameMng.I.mapTile[posY, posX]._unitObj.transform.localPosition = Vector2.Lerp(GameMng.I.mapTile[posY, posX]._unitObj.transform.localPosition, GameMng.I.mapTile[toY, toX].transform.localPosition, GameMng.I.unitSpeed * Time.deltaTime);        //Ÿ�� �� �ε巯�� �̵�
                yield return null;
            }
            else
            {
                //act = ACTIVITY.NONE;

                GameMng.I.mapTile[posY, posX]._unitObj.transform.localPosition = GameMng.I.mapTile[toY, toX].transform.localPosition;
                GameMng.I.mapTile[toY, toX]._unitObj = GameMng.I.mapTile[posY, posX]._unitObj;
                GameMng.I.mapTile[posY, posX]._unitObj = null;
                //GameMng.I.mapTile[posY, posX]._code = (int)TILE.CAN_MOVE - 1;
                isRun = false;
            }
        }
    }

    /**
     * @brief ������ �̵� �������� ���� ��ȯ
     * @param selectedObj ��� ���� ������Ʈ
     * @param targetObj �̵��� ������Ʈ
     */
    public void reversalUnit(Transform selectedObj, Transform targetObj)
    {
        if (selectedObj.localPosition.x < targetObj.localPosition.x)                // ���� ���� ȸ�� ������
        {
            selectedObj.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
        }
        else if (selectedObj.localPosition.x == targetObj.localPosition.x)          // ���� ���� X
        {
        }
        else
        {
            selectedObj.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));       // ���� ���� ȸ�� ����
        }
    }

    /**
     * @brief �ǹ��� ������ (Ŭ�� ����)
     * @param cost ���� �ڽ�Ʈ
     * @param index �ǹ� �ڵ�
     */
    public void Building(int cost, int index)
    {
        GameMng.I.minGold(3);       // TODO : �ڽ�Ʈ�� ����
        GameMng.I.mouseRaycast(true);                       //ĳ���� ������ Ÿ�� ������ �˾ƿ;��ؼ� false���� true�� ����
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            if (GameMng.I._gold >= cost)
            {
                GameObject Child = Instantiate(builtObj[index - 200], GameMng.I.targetTile.transform) as GameObject;
                GameMng.I.targetTile._builtObj = Child.GetComponent<Built>();
                GameMng.I.targetTile._code = index;
                GameMng.I.minGold(cost);
                GameMng.I._range.rangeTileReset();
                GameMng.I._range.SelectTileSetting(true);
                GameMng.I.targetTile._builtObj._uniqueNumber = NetworkMng.getInstance.uniqueNumber;
                NetworkMng.getInstance.SendMsg(string.Format("CREATE_BUILT:{0}:{1}:{2}:{3}", GameMng.I.targetTile.PosX, GameMng.I.targetTile.PosY, index, NetworkMng.getInstance.uniqueNumber));
                act = ACTIVITY.NONE;
                NetworkMng.getInstance.SendMsg("TURN");
            }
        }
    }

    /**
     * @brief ������ ������ (�������� Ŭ��� ������ ������ ȣ���)
     * @param posX ������ X ��ġ
     * @param posY ������ Y ��ġ
     * @param index ���� �ڵ�
     * @param uniqueNumber ������
     */
    public void CreateBuilt(int posX, int posY, int index, int uniqueNumber)
    {
        GameObject Child = Instantiate(builtObj[index - 200], GameMng.I.mapTile[posY, posX].transform) as GameObject;
        GameMng.I.mapTile[posY, posX]._builtObj = Child.GetComponent<Built>();
        GameMng.I.mapTile[posY, posX]._code = index;
        GameMng.I.mapTile[posY, posX]._builtObj._uniqueNumber = uniqueNumber;
    }

    /**
     * @brief ���� ����
     */
    public void UnitAttack()
    {
        GameMng.I.mouseRaycast(true);
        if (GameMng.I.targetTile._unitObj != null || GameMng.I.targetTile._builtObj != null)
        {
            NetworkMng.getInstance.SendMsg("TURN");
            if (GameMng.I.targetTile._unitObj != null)
            {
                GameMng.I.targetTile._unitObj._hp -= GameMng.I.selectedTile._unitObj._damage;
                if (GameMng.I.targetTile._unitObj._hp <= 0)
                {
                    Destroy(GameMng.I.targetTile._unitObj.gameObject);
                    GameMng.I.targetTile._unitObj = null;
                    GameMng.I.targetTile._code = 0;
                }
            }
            else if (GameMng.I.targetTile._builtObj != null)
            {
                GameMng.I.targetTile._builtObj._hp -= GameMng.I.selectedTile._unitObj._damage;
                if (GameMng.I.targetTile._builtObj._code == (int)BUILT.AIRDROP)
                {
                    Debug.Log("asdf");
                    int nKind = Random.Range(1, 3);            // 1: ���,  2: �ķ�
                    int nResult = Random.Range(20, 60);
                    Debug.Log(nKind + ", " + nResult);
                    if (nKind == 1)
                    {
                        GameMng.I._gold += nResult;
                    }
                    else if (nKind == 2)
                    {
                        Debug.Log("�ķ� + " + nKind);
                    }

                    GameMng.I.targetTile._builtObj._hp -= 1;
                }
                if (GameMng.I.targetTile._builtObj._hp <= 0)
                {
                    Destroy(GameMng.I.targetTile._builtObj.gameObject);
                    GameMng.I.targetTile._builtObj = null;
                    GameMng.I.targetTile._code = 0;
                }
            }
            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();
            act = ACTIVITY.NONE;
        }
    }
}