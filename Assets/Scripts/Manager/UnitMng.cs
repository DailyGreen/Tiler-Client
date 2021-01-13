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
            GameMng.I._range.AttackrangeTileReset();                                                     //�߰�
            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();
        }
    }

    public void CheckMove()
    {
        GameMng.I.mouseRaycast(true);

        if (GameMng.I.hit.collider != null)
        {
            Debug.Log("ĳ���� �̵� �غ�");
            GameMng.I.cleanActList();
            GameMng.I._range.rangeTileReset();  // ���� Ÿ�� ��ġ �ʱ�ȭ
            GameMng.I.distanceOfTiles = Vector2.Distance(GameMng.I.selectedTile._unitObj.transform.position, GameMng.I.targetTile.transform.position);
            if (GameMng.I.hit.collider.tag.Equals("Tile") && GameMng.I.distanceOfTiles <= 1.5f && Tile.isEmptyTile(GameMng.I.targetTile))
            {
                Debug.Log("ĳ���� �̵� ����");
                act = ACTIVITY.ACTING;
                if (GameMng.I.selectedTile._unitObj.transform.localPosition.x < GameMng.I.targetTile.transform.localPosition.x)                                                                 //���� ���� ȸ�� ������
                {
                    GameMng.I.selectedTile._unitObj.transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
                }
                else if (GameMng.I.selectedTile._unitObj.transform.localPosition.x == GameMng.I.targetTile.transform.localPosition.x)                                                           //���� ���� X
                {

                }
                else
                {
                    GameMng.I.selectedTile._unitObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));                                                                 //���� ���� ȸ�� ����
                }
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
            GameMng.I.selectedTile._unitObj = null;
            GameMng.I.selectedTile._code = (int)TILE.CAN_MOVE - 1;
        }
    }

    public void Building(int cost, int index)
    {
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
                    int nKind = Random.Range(1, 3);            // 1: ��� 2: �ķ�
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