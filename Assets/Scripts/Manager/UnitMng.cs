using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitMng : MonoBehaviour
{
    public ACTIVITY act = ACTIVITY.NONE;

    public GameObject[] builtObj = null;

    public GameObject AttackEffect = null;

    private void Start()
    {
        act = ACTIVITY.NONE;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    NetworkMng.getInstance.SendMsg("TURN");

        if (Input.GetMouseButtonDown(0) && act != ACTIVITY.ACTING && GameMng.I._BuiltGM.act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (act)
            {
                case ACTIVITY.MOVE:
                    CheckMove();
                    break;
                case ACTIVITY.BUILD_MINE:
                    Building(Mine.cost, (int)BUILT.MINE, GameMng.I.selectedTile._code);
                    break;
                case ACTIVITY.BUILD_FARM:
                    Building(Farm.cost, (int)BUILT.FARM, GameMng.I.selectedTile._code);
                    break;
                case ACTIVITY.BUILD_ATTACK_BUILDING:
                    Building(Turret.cost, (int)BUILT.ATTACK_BUILDING, GameMng.I.selectedTile._code);
                    break;
                case ACTIVITY.BUILD_MILLITARY_BASE:
                    Building(MillitaryBase.cost, (int)BUILT.MILLITARY_BASE, GameMng.I.selectedTile._code);
                    break;
                case ACTIVITY.ATTACK:
                    UnitAttack(GameMng.I.selectedTile._unitObj._attackdistance);
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
        float movedis;
        GameMng.I.mouseRaycast(true);

        if (GameMng.I.hit.collider != null)
        {
            GameMng.I.cleanActList();
            GameMng.I._range.rangeTileReset();  // ���� Ÿ�� ��ġ �ʱ�ȭ

            if (GameMng.I.selectedTile._code == (int)UNIT.FOREST_WITCH_1 || GameMng.I.selectedTile._code == (int)UNIT.SEA_WITCH_1)
            {
                movedis = 3.0f;
                GameMng.I.unitSpeed = 6.0f;
            }
            else
            {
                movedis = 1.5f;
                GameMng.I.unitSpeed = 3.0f;
            }

            GameMng.I.distanceOfTiles = Vector2.Distance(GameMng.I.selectedTile._unitObj.transform.position, GameMng.I.targetTile.transform.position);

            if (GameMng.I.hit.collider.tag.Equals("Tile") && GameMng.I.distanceOfTiles <= movedis && Tile.isEmptyTile(GameMng.I.targetTile))
            {
                act = ACTIVITY.ACTING;

                reversalUnit(GameMng.I.selectedTile._unitObj.transform, GameMng.I.targetTile.transform);

                GameMng.I.selectedTile._unitObj._anim.SetTrigger("isRunning");

                NetworkMng.getInstance.SendMsg(string.Format("MOVE_UNIT:{0}:{1}:{2}:{3}", GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosZ, GameMng.I.targetTile.PosX, GameMng.I.targetTile.PosZ));
                
                StartCoroutine("Moving", movedis);
            }
            else                                     // ������ �ƴ� �ٸ� ���� ����
            {
                act = ACTIVITY.NONE;
                //GameMng.I.selectedTile = GameMng.I.targetTile;
                //GameMng.I.targetTile = null;
            }
        }
    }

    /**
     * @brief ���� �̵� (Ŭ�� ����)
     */
    IEnumerator Moving(float movedis)
    {
        if (Vector2.Distance(GameMng.I.selectedTile._unitObj.transform.localPosition, GameMng.I.targetTile.transform.localPosition) >= 0.01f
            && GameMng.I.distanceOfTiles <= movedis) // ĳ���Ϳ� Ÿ�ϰ� �Ÿ��� 1.5 * a �����Ͻ� �����ϼ� ���� (�Ÿ� ��ĭ�� 1.24?���� �ǵ��)
        {
            GameMng.I.selectedTile._unitObj.transform.localPosition = Vector2.Lerp(GameMng.I.selectedTile._unitObj.transform.localPosition, GameMng.I.targetTile.transform.localPosition, GameMng.I.unitSpeed * Time.deltaTime);        //Ÿ�� �� �ε巯�� �̵�
            
            yield return null;

            StartCoroutine("Moving", movedis);
        }
        else if (GameMng.I.distanceOfTiles >= 0.1f)     // ���� �Ÿ��� �������� Ÿ�� ��ġ�� �ڵ� �̵�
        {
            GameMng.I.selectedTile._unitObj.transform.localPosition = GameMng.I.targetTile.transform.localPosition;
            GameMng.I.targetTile._unitObj = GameMng.I.selectedTile._unitObj;
            GameMng.I.targetTile._code = GameMng.I.selectedTile._unitObj._code;
            GameMng.I.selectedTile._unitObj = null;

            GameMng.I._hextile.TilecodeClear(GameMng.I.selectedTile);
            
            GameMng.I.selectedTile = GameMng.I.targetTile;

            GameMng.I._range.SelectTileSetting(false);

            GameMng.I._hextile.FindDistancesTo(GameMng.I.selectedTile);

            GameMng.I.myTurn = false;

            NetworkMng.getInstance.SendMsg("TURN");

            act = ACTIVITY.NONE;
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

        reversalUnit(GameMng.I._hextile.GetCell(posX, posY)._unitObj.transform, GameMng.I._hextile.GetCell(toX, toY).transform);

        GameMng.I._hextile.GetCell(posX, posY)._unitObj._anim.SetTrigger("isRunning");
        GameMng.I._hextile.GetCell(toX, toY)._code = GameMng.I._hextile.GetCell(posX, posY)._code;

        GameMng.I._hextile.TilecodeClear(posX, posY);

        GameMng.I.addActMessage(string.Format("{0}���� ������ �̵��߽��ϴ�.", GameMng.I.getUserName(GameMng.I._hextile.GetCell(posX, posY)._unitObj._uniqueNumber)), toX, toY);

        while (isRun)
        {
            if (Vector2.Distance(GameMng.I._hextile.GetCell(posX, posY)._unitObj.transform.localPosition, GameMng.I._hextile.GetCell(toX, toY).transform.localPosition) >= 0.01f)
            {
                GameMng.I._hextile.GetCell(posX, posY)._unitObj.transform.localPosition = Vector2.Lerp(GameMng.I._hextile.GetCell(posX, posY)._unitObj.transform.localPosition, GameMng.I._hextile.GetCell(toX, toY).transform.localPosition, GameMng.I.unitSpeed * Time.deltaTime);        //Ÿ�� �� �ε巯�� �̵�
                yield return null;
            }
            else
            {
                //act = ACTIVITY.NONE;

                GameMng.I._hextile.GetCell(posX, posY)._unitObj.transform.localPosition = GameMng.I._hextile.GetCell(toX, toY).transform.localPosition;
                GameMng.I._hextile.GetCell(toX, toY)._unitObj = GameMng.I._hextile.GetCell(posX, posY)._unitObj;
                GameMng.I._hextile.GetCell(posX, posY)._unitObj = null;

                GameMng.I.refreshMainUI();

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
     * @param unitindex �ǹ��� ���� ���� �ڵ�
     */
    public void Building(int cost, int index, int unitindex)
    {
        GameMng.I.mouseRaycast(true);                       //ĳ���� ������ Ÿ�� ������ �˾ƿ;��ؼ� false���� true�� ����
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            if (GameMng.I._gold >= cost)
            {
                GameMng.I.selectedTile._unitObj._bActAccess = false;

                GameObject Child = Instantiate(builtObj[index - 200], GameMng.I.targetTile.transform) as GameObject;
                GameMng.I.targetTile._builtObj = Child.GetComponent<Built>();
                GameMng.I.targetTile._builtObj.SaveX = GameMng.I.selectedTile.PosX;                // �����ϴ� �ǹ����� �ִ� ��ġ ���� ������ �ش� ���� ��ġ���� ��������
                GameMng.I.targetTile._builtObj.SaveY = GameMng.I.selectedTile.PosZ;
                GameMng.I.targetTile._builtObj._uniqueNumber = NetworkMng.getInstance.uniqueNumber;
                GameMng.I.targetTile._code = index;

                GameMng.I.minGold(cost);

                GameMng.I._range.rangeTileReset();
                GameMng.I._range.SelectTileSetting(true);


                NetworkMng.getInstance.SendMsg(string.Format("CREATE_BUILT:{0}:{1}:{2}:{3}:{4}:{5}", GameMng.I.targetTile.PosX, GameMng.I.targetTile.PosZ, index, NetworkMng.getInstance.uniqueNumber, GameMng.I.selectedTile.PosX, GameMng.I.selectedTile.PosZ));

                switch (unitindex)
                {
                    case (int)UNIT.FOREST_WORKER:
                        GameMng.I.selectedTile._unitObj.GetComponent<Forest_Worker>().working();
                        break;
                    case (int)UNIT.DESERT_WORKER:
                        //GameMng.I.selectedTile._unitObj.GetComponent<Desert_Worker>().working();
                        break;
                    case (int)UNIT.SEA_WORKER:
                        GameMng.I.selectedTile._unitObj.GetComponent<Sea_Worker>().working();
                        break;
                }

                GameMng.I.cleanSelected();
                GameMng.I.cleanActList();

                act = ACTIVITY.NONE;

                NetworkMng.getInstance.SendMsg("TURN");
            }
        }
        else if (Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) >= 1.5f)
        {
            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();
            GameMng.I._range.rangeTileReset();

            act = ACTIVITY.NONE;
        }
        else if (GameMng.I.targetTile._builtObj != null || GameMng.I.targetTile._unitObj != null)
        {
            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();
            GameMng.I._range.rangeTileReset();

            act = ACTIVITY.NONE;
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
    public void CreateBuilt(int posX, int posY, int index, int uniqueNumber, int byX, int byY)
    {
        GameObject Child = Instantiate(builtObj[index - 200], GameMng.I._hextile.GetCell(posX, posY).transform) as GameObject;

        GameMng.I._hextile.GetCell(posX, posY)._builtObj = Child.GetComponent<Built>();
        GameMng.I._hextile.GetCell(posX, posY)._code = index;
        GameMng.I._hextile.GetCell(posX, posY)._builtObj._uniqueNumber = uniqueNumber;

        GameMng.I._hextile.GetCell(byX, byY)._unitObj._bActAccess = false;
        GameMng.I._hextile.GetCell(posX, posY)._builtObj.SaveX = byX;
        GameMng.I._hextile.GetCell(posX, posY)._builtObj.SaveY = byY;

        switch (GameMng.I._hextile.GetCell(byX, byY)._code)
        {
            case (int)UNIT.FOREST_WORKER:
                reversalUnit(GameMng.I._hextile.GetCell(byX, byY)._unitObj.transform, GameMng.I._hextile.GetCell(posX, posY).transform);
                GameMng.I._hextile.GetCell(byX, byY)._unitObj.GetComponent<Forest_Worker>()._anim.SetBool("isWorking", true);
                break;
            case (int)UNIT.DESERT_WORKER:
                //GameMng.I.selectedTile._unitObj.GetComponent<>();
                break;
            case (int)UNIT.SEA_WORKER:
                reversalUnit(GameMng.I._hextile.GetCell(byX, byY)._unitObj.transform, GameMng.I._hextile.GetCell(posX, posY).transform);
                GameMng.I._hextile.GetCell(byX, byY)._unitObj.GetComponent<Sea_Worker>()._anim.SetBool("isWorking", true);
                break;
        }

        GameMng.I.addActMessage(string.Format("{0}���� �ǹ��� �������� �ֽ��ϴ�.", GameMng.I.getUserName(uniqueNumber)), posX, posY);
    }

    /**
     * @brief ���� ���� ���Ÿ� ���� �Ϸ��� ���� �ʿ�
     * @param distance ���� ��Ÿ�
     */
    public void UnitAttack(int distance)
    {
        GameMng.I.mouseRaycast(true);
        if (GameMng.I.targetTile._unitObj != null || GameMng.I.targetTile._builtObj != null)
        {
            if (GameMng.I.targetTile._unitObj != null && GameMng.I.targetTile._unitObj._uniqueNumber != NetworkMng.getInstance.uniqueNumber && GameMng.I.targetTile.Distance <= distance)
            {
                GameMng.I.targetTile._unitObj._hp -= GameMng.I.selectedTile._unitObj._damage;

                AttackEffectSetting(GameMng.I.selectedTile._unitObj._code); ;                           // �����ϴ� ���ֿ� �´� ����Ʈ�� �����Ŵ

                if (GameMng.I.targetTile._unitObj._hp <= 0)
                {
                    GameMng.I.targetTile._unitObj.DestroyMyself();
                    GameMng.I.targetTile._unitObj = null;
                    GameMng.I._hextile.TilecodeClear(GameMng.I.targetTile);
                }

                EnforceAttack();
            }
            else if (GameMng.I.targetTile._builtObj != null && GameMng.I.targetTile._builtObj._uniqueNumber != NetworkMng.getInstance.uniqueNumber && GameMng.I.targetTile.Distance <= distance)
            {
                GameMng.I.targetTile._builtObj._hp -= GameMng.I.selectedTile._unitObj._damage;
                
                AttackEffectSetting(GameMng.I.selectedTile._unitObj._code); ;                           // �����ϴ� ���ֿ� �´� ����Ʈ�� �����Ŵ

                if (GameMng.I.targetTile._builtObj._code == (int)BUILT.AIRDROP)
                {
                    int nKind = Random.Range(1, 3);            // 1: ���,  2: �ķ�
                    int nResult = Random.Range(20, 60);

                    Debug.Log(nKind + ", " + nResult);

                    if (nKind == 1)
                    {
                        GameMng.I.addGold(nResult);
                    }
                    else if (nKind == 2)
                    {
                        GameMng.I.addFood(nResult);
                    }

                    GameMng.I.targetTile._builtObj._hp -= 1;
                }
                if (GameMng.I.targetTile._builtObj._hp <= 0)
                {
                    GameMng.I.targetTile._builtObj.DestroyMyself();
                    GameMng.I.targetTile._builtObj = null;

                    GameMng.I._hextile.TilecodeClear(GameMng.I.targetTile);                 // TODO : �ڵ� �� ���� ������
                }
                EnforceAttack();
            }

            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();

            act = ACTIVITY.NONE;
        }
    }

    /**
     * @brief ����Ʈ�� �ʿ��� ���ֵ鿡�� �´� ����Ʈ ����
     * @param unitcode ������ �ڵ�
     */
    void AttackEffectSetting(int unitcode)
    {
        GameObject EffectParent = GameObject.Find("AttackEffect");
        switch (unitcode)
        {
            case (int)UNIT.FOREST_WITCH_0:
                AttackEffect = EffectParent.transform.Find("Forest").transform.Find("ForestEffect_0").gameObject;
                break;
            case (int)UNIT.FOREST_WITCH_1:
                AttackEffect = EffectParent.transform.Find("Forest").transform.Find("ForestEffect_1").gameObject;
                break;
            case (int)UNIT.SEA_WITCH_0:
                AttackEffect = EffectParent.transform.Find("Sea").transform.Find("SeaEffect_0").gameObject;
                break;
            case (int)UNIT.SEA_WITCH_1:
                AttackEffect = EffectParent.transform.Find("Sea").transform.Find("SeaEffect_2").gameObject;
                break;
            case (int)UNIT.DESERT_WITCH_0:
                AttackEffect = EffectParent.transform.Find("Desert").transform.Find("DesertEffect_0").gameObject;
                break;
            case (int)UNIT.DESERT_WITCH_1:
                AttackEffect = EffectParent.transform.Find("Desert").transform.Find("DesertEffect_2").gameObject;
                break;
            default:
                break;
        }
        AttackEffect.SetActive(true);
        AttackEffect.transform.position = new Vector3(GameMng.I.targetTile.transform.position.x, GameMng.I.targetTile.transform.position.y, -10f);
        StartCoroutine("AttackEffectReset");
    }
    
    /**
     * @brief ����Ʈ ��ƼŬ�� ������ ����Ǵ� �ҽ�
     */
    IEnumerator AttackEffectReset()
    {
        yield return new WaitForSeconds(2.3f);                      // ���� �ִ� ����Ʈ�� ���� ���� �����Ǵ� ����Ʈ�� 2.3����
        AttackEffect.transform.localPosition = new Vector3(0f, 0f, 10f);
        AttackEffect.SetActive(false);
        AttackEffect = null;
    }

    /**
     * @brief ���� ��� (Ŭ��)
     */
    void EnforceAttack()
    {
        reversalUnit(GameMng.I.selectedTile._unitObj.transform, GameMng.I.targetTile.transform);

        GameMng.I.selectedTile._unitObj._anim.SetTrigger("isAttacking");

        NetworkMng.getInstance.SendMsg(string.Format("ATTACK:{0}:{1}:{2}:{3}:{4}",
            GameMng.I.selectedTile.PosX,
            GameMng.I.selectedTile.PosZ,
            GameMng.I.targetTile.PosX,
            GameMng.I.targetTile.PosZ,
            GameMng.I.selectedTile._unitObj._damage));

        NetworkMng.getInstance.SendMsg("TURN");
    }

    public void Move()
    {
        GameMng.I._range.moveRange(GameMng.I.selectedTile._unitObj._basedistance);
        Debug.Log("ĳ���� �̵�");
    }

    public void buildMine()
    {
        GameMng.I._range.moveRange(GameMng.I.selectedTile._unitObj._basedistance);
    }

    public void buildFarm()
    {
        GameMng.I._range.moveRange(GameMng.I.selectedTile._unitObj._basedistance);
    }

    public void buildAttackBuilding()
    {
        GameMng.I._range.moveRange(GameMng.I.selectedTile._unitObj._basedistance);
    }

    public void buildMillitaryBaseBuilding()
    {
        GameMng.I._range.moveRange(GameMng.I.selectedTile._unitObj._basedistance);
    }

    public void buildShieldBuilding()
    {
        GameMng.I._range.moveRange(GameMng.I.selectedTile._unitObj._basedistance);
    }

    public void buildUpgradeBuilding()
    {
        GameMng.I._range.moveRange(GameMng.I.selectedTile._unitObj._basedistance);
    }

    public void unitAttacking()
    {
        GameMng.I._range.attackRange(GameMng.I.selectedTile._unitObj._attackdistance);
    }
}