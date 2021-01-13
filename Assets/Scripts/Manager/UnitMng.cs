using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitMng : MonoBehaviour
{
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
                case ACTIVITY.ATTACK:
                    UnitAttack();
                    break;
            }
        }
        else if (Input.GetMouseButtonUp(1) && act != ACTIVITY.ACTING)
        {
            act = ACTIVITY.NONE;
            GameMng.I._range.rangeTileReset();
            GameMng.I._range.AttackrangeTileReset();                                                     //추가
            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();
        }
    }

    public void CheckMove()
    {
        GameMng.I.mouseRaycast(true);

        if (GameMng.I.hit.collider != null)
        {
            Debug.Log("캐릭터 이동 준비");
            GameMng.I.cleanActList();
            GameMng.I._range.rangeTileReset();  // 범위 타일 위치 초기화
            GameMng.I.distanceOfTiles = Vector2.Distance(GameMng.I.selectedTile._unitObj.transform.position, GameMng.I.targetTile.transform.position);
            if (GameMng.I.hit.collider.tag.Equals("Tile") && GameMng.I.distanceOfTiles <= 1.5f && Tile.isEmptyTile(GameMng.I.targetTile))
            {
                Debug.Log("캐릭터 이동 시작");
                act = ACTIVITY.ACTING;
                if (GameMng.I.selectedTile._unitObj.transform.localPosition.x < GameMng.I.targetTile.transform.localPosition.x)                                                                 //가는 방향 회전 오른쪽
                {
                    GameMng.I.selectedTile._unitObj.transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
                }
                else if (GameMng.I.selectedTile._unitObj.transform.localPosition.x == GameMng.I.targetTile.transform.localPosition.x)                                                           //방향 변동 X
                {

                }
                else
                {
                    GameMng.I.selectedTile._unitObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));                                                                 //가는 방향 회전 왼쪽
                }
                StartCoroutine("Moving");
            }
            else                                     // 범위가 아닌 다른 곳을 누름
            {
                Debug.Log("else로 빠짐");
                act = ACTIVITY.NONE;
                //GameMng.I.selectedTile = GameMng.I.targetTile;
                //GameMng.I.targetTile = null;
            }
        }
    }

    IEnumerator Moving()
    {
        if (Vector2.Distance(GameMng.I.selectedTile._unitObj.transform.localPosition, GameMng.I.targetTile.transform.localPosition) >= 0.01f
            && GameMng.I.distanceOfTiles <= 1.5f) // 캐릭터와 타일간 거리가 1.5 * a 이하일시 움직일수 있음 (거리 한칸당 1.24?정도 되드라)
        {
            GameMng.I.selectedTile._unitObj.transform.localPosition = Vector2.Lerp(GameMng.I.selectedTile._unitObj.transform.localPosition, GameMng.I.targetTile.transform.localPosition, GameMng.I.unitSpeed * Time.deltaTime);        //타일 간 부드러운 이동
            yield return null;
            StartCoroutine("Moving");
        }
        else if (GameMng.I.distanceOfTiles >= 0.1f)     // 남은 거리가 좁아지면 타일 위치로 자동 이동
        {
            act = ACTIVITY.NONE;

            GameMng.I.selectedTile._unitObj.transform.localPosition = GameMng.I.targetTile.transform.localPosition;
            GameMng.I.targetTile._unitObj = GameMng.I.selectedTile._unitObj;
            GameMng.I.selectedTile._unitObj = null;
            GameMng.I.selectedTile._code = (int)TILE.CAN_MOVE - 1;
        }
    }

    public void Building(int cost, int index)
    {
        GameMng.I.mouseRaycast(true);                       //캐릭터 정보와 타일 정보를 알아와야해서 false에서 true로 변경
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            if (GameMng.I._gold >= cost)
            {
                GameObject Child = Instantiate(builtObj[index - 200], GameMng.I.targetTile.transform) as GameObject;
                GameMng.I.targetTile._builtObj = Child.GetComponent<Built>();
                GameMng.I.targetTile._code = index;
                GameMng.I.minGold(cost);
                GameMng.I._range.rangeTileReset();
                GameMng.I.targetTile._builtObj._uniqueNumber = NetworkMng.getInstance.uniqueNumber;
                act = ACTIVITY.NONE;
            }
        }
    }

    public void UnitAttack()
    {
        GameMng.I.mouseRaycast(true);
        if (GameMng.I.targetTile._unitObj != null || GameMng.I.targetTile._builtObj != null)
        {
            if (GameMng.I.targetTile._unitObj != null)
            {
                GameMng.I.targetTile._unitObj._hp -= GameMng.I.selectedTile._unitObj._damage;
                if (GameMng.I.targetTile._unitObj._hp <= 0)
                {
                    Destroy(GameMng.I.targetTile._unitObj.gameObject);
                }
            }
            else if (GameMng.I.targetTile._builtObj != null)
            {
                GameMng.I.targetTile._builtObj._hp -= GameMng.I.selectedTile._unitObj._damage;
                if (GameMng.I.targetTile._builtObj._code == (int)BUILT.AIRDROP)
                {
                    Debug.Log("asdf");
                    int nKind = Random.Range(1, 3);            // 1: 골드 2: 식량
                    int nResult = Random.Range(20, 60);
                    Debug.Log(nKind + ", " + nResult);
                    if (nKind == 1)
                    {
                        GameMng.I._gold += nResult;
                    }
                    else if (nKind == 2)
                    {
                        Debug.Log("식량 + " + nKind);
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