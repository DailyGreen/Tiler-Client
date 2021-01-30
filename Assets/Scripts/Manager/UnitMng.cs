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
            GameMng.I._range.AttackrangeTileReset();                                                     //추가
            GameMng.I._range.SelectTileSetting(true);

            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();
        }
    }

    /**
    * @brief 유닛 이동 검사 (대상이 올바른지)
    */
    public void CheckMove()
    {
        float movedis;
        GameMng.I.mouseRaycast(true);

        if (GameMng.I.hit.collider != null)
        {
            GameMng.I.cleanActList();
            GameMng.I._range.rangeTileReset();  // 범위 타일 위치 초기화

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
            else                                     // 범위가 아닌 다른 곳을 누름
            {
                act = ACTIVITY.NONE;
                //GameMng.I.selectedTile = GameMng.I.targetTile;
                //GameMng.I.targetTile = null;
            }
        }
    }

    /**
     * @brief 유닛 이동 (클라 전용)
     */
    IEnumerator Moving(float movedis)
    {
        if (Vector2.Distance(GameMng.I.selectedTile._unitObj.transform.localPosition, GameMng.I.targetTile.transform.localPosition) >= 0.01f
            && GameMng.I.distanceOfTiles <= movedis) // 캐릭터와 타일간 거리가 1.5 * a 이하일시 움직일수 있음 (거리 한칸당 1.24?정도 되드라)
        {
            GameMng.I.selectedTile._unitObj.transform.localPosition = Vector2.Lerp(GameMng.I.selectedTile._unitObj.transform.localPosition, GameMng.I.targetTile.transform.localPosition, GameMng.I.unitSpeed * Time.deltaTime);        //타일 간 부드러운 이동
            
            yield return null;

            StartCoroutine("Moving", movedis);
        }
        else if (GameMng.I.distanceOfTiles >= 0.1f)     // 남은 거리가 좁아지면 타일 위치로 자동 이동
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
     * @brief 유닛 이동 (상대편의 이동, 서버 전용)
     * @param posX 대상 x 좌표
     * @param posY 대상 y 좌표
     * @param toX 이동할 x 좌표
     * @param toY 이동할 y 좌표
     */
    public IEnumerator MovingUnit(int posX, int posY, int toX, int toY)
    {
        bool isRun = true;

        reversalUnit(GameMng.I._hextile.GetCell(posX, posY)._unitObj.transform, GameMng.I._hextile.GetCell(toX, toY).transform);

        GameMng.I._hextile.GetCell(posX, posY)._unitObj._anim.SetTrigger("isRunning");
        GameMng.I._hextile.GetCell(toX, toY)._code = GameMng.I._hextile.GetCell(posX, posY)._code;

        GameMng.I._hextile.TilecodeClear(posX, posY);

        GameMng.I.addActMessage(string.Format("{0}님의 유닛이 이동했습니다.", GameMng.I.getUserName(GameMng.I._hextile.GetCell(posX, posY)._unitObj._uniqueNumber)), toX, toY);

        while (isRun)
        {
            if (Vector2.Distance(GameMng.I._hextile.GetCell(posX, posY)._unitObj.transform.localPosition, GameMng.I._hextile.GetCell(toX, toY).transform.localPosition) >= 0.01f)
            {
                GameMng.I._hextile.GetCell(posX, posY)._unitObj.transform.localPosition = Vector2.Lerp(GameMng.I._hextile.GetCell(posX, posY)._unitObj.transform.localPosition, GameMng.I._hextile.GetCell(toX, toY).transform.localPosition, GameMng.I.unitSpeed * Time.deltaTime);        //타일 간 부드러운 이동
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
     * @brief 유닛의 이동 방향으로 방향 전환
     * @param selectedObj 대상 유닛 오브젝트
     * @param targetObj 이동할 오브젝트
     */
    public void reversalUnit(Transform selectedObj, Transform targetObj)
    {
        if (selectedObj.localPosition.x < targetObj.localPosition.x)                // 가는 방향 회전 오른쪽
        {
            selectedObj.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
        }
        else if (selectedObj.localPosition.x == targetObj.localPosition.x)          // 방향 변동 X
        {
        }
        else
        {
            selectedObj.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));       // 가는 방향 회전 왼쪽
        }
    }

    /**
     * @brief 건물을 생성함 (클라 전용)
     * @param cost 사용된 코스트
     * @param index 건물 코드
     * @param unitindex 건물을 짓는 유닛 코드
     */
    public void Building(int cost, int index, int unitindex)
    {
        GameMng.I.mouseRaycast(true);                       //캐릭터 정보와 타일 정보를 알아와야해서 false에서 true로 변경
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            if (GameMng.I._gold >= cost)
            {
                GameMng.I.selectedTile._unitObj._bActAccess = false;

                GameObject Child = Instantiate(builtObj[index - 200], GameMng.I.targetTile.transform) as GameObject;
                GameMng.I.targetTile._builtObj = Child.GetComponent<Built>();
                GameMng.I.targetTile._builtObj.SaveX = GameMng.I.selectedTile.PosX;                // 생성하는 건물에게 있는 위치 저장 변수에 해당 유닛 위치값을 저장해줌
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
     * @brief 유닛을 생성함 (서버에서 클라로 정보를 보낼때 호출됨)
     * @param posX 생성할 X 위치
     * @param posY 생성할 Y 위치
     * @param index 유닛 코드
     * @param uniqueNumber 생성자
     * @param byX 생성을 지시한 X 위치
     * @param byY 생성을 지시한 Y 위치
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

        GameMng.I.addActMessage(string.Format("{0}님의 건물이 지어지고 있습니다.", GameMng.I.getUserName(uniqueNumber)), posX, posY);
    }

    /**
     * @brief 유닛 공격 원거리 공격 하려면 수정 필요
     * @param distance 공격 사거리
     */
    public void UnitAttack(int distance)
    {
        GameMng.I.mouseRaycast(true);
        if (GameMng.I.targetTile._unitObj != null || GameMng.I.targetTile._builtObj != null)
        {
            if (GameMng.I.targetTile._unitObj != null && GameMng.I.targetTile._unitObj._uniqueNumber != NetworkMng.getInstance.uniqueNumber && GameMng.I.targetTile.Distance <= distance)
            {
                GameMng.I.targetTile._unitObj._hp -= GameMng.I.selectedTile._unitObj._damage;

                AttackEffectSetting(GameMng.I.selectedTile._unitObj._code); ;                           // 공격하는 유닛에 맞는 이펙트를 적용시킴

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
                
                AttackEffectSetting(GameMng.I.selectedTile._unitObj._code); ;                           // 공격하는 유닛에 맞는 이펙트를 적용시킴

                if (GameMng.I.targetTile._builtObj._code == (int)BUILT.AIRDROP)
                {
                    int nKind = Random.Range(1, 3);            // 1: 골드,  2: 식량
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

                    GameMng.I._hextile.TilecodeClear(GameMng.I.targetTile);                 // TODO : 코드 값 원래 값으로
                }
                EnforceAttack();
            }

            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();

            act = ACTIVITY.NONE;
        }
    }

    /**
     * @brief 이펙트가 필요한 유닛들에게 맞는 이펙트 적용
     * @param unitcode 유닛의 코드
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
     * @brief 이펙트 파티클이 끝난후 적용되는 소스
     */
    IEnumerator AttackEffectReset()
    {
        yield return new WaitForSeconds(2.3f);                      // 현재 있는 이펙트중 제일 오래 유지되는 이펙트가 2.3초임
        AttackEffect.transform.localPosition = new Vector3(0f, 0f, 10f);
        AttackEffect.SetActive(false);
        AttackEffect = null;
    }

    /**
     * @brief 공격 명령 (클라)
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
        Debug.Log("캐릭터 이동");
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