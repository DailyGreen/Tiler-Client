using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetMouseButtonDown(0) && act != ACTIVITY.ACTING)
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
            }
        }
        else if (Input.GetMouseButtonUp(1) && act != ACTIVITY.ACTING)
        {
            act = ACTIVITY.NONE;
            GameMng.I._range.rangeTileReset();
            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();
        }
    }

    public void CheckMove()
    {
        GameMng.I.mouseRaycast(true);

        if (GameMng.I.hit.collider != null)
        {
            GameMng.I.cleanActList();
            GameMng.I._range.rangeTileReset();  // ���� Ÿ�� ��ġ �ʱ�ȭ
            GameMng.I.distanceOfTiles = Vector2.Distance(GameMng.I.selectedTile._unitObj.transform.localPosition, GameMng.I.targetTile.transform.localPosition);
            if (GameMng.I.hit.collider.tag.Equals("Tile") && GameMng.I.distanceOfTiles <= 1.5f && Tile.isEmptyTile(GameMng.I.targetTile))
            {
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
            else
            {
                // ������ �ƴ� �ٸ� ���� ����
                act = ACTIVITY.NONE;
                GameMng.I.selectedTile = GameMng.I.targetTile;
                GameMng.I.targetTile = null;
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
        }
    }

    public void Building(int cost, int index)
    {
        GameMng.I.mouseRaycast();
        if (GameMng.I.selectedTile._builtObj == null)
        {
            if (GameMng.I._gold > cost)
            {
                GameObject Child = Instantiate(builtObj[index - 200], GameMng.I.selectedTile.transform) as GameObject;
                GameMng.I.selectedTile._builtObj = Child.GetComponent<Built>();
                GameMng.I.selectedTile._code = index;
                GameMng.I.minGold(cost);
                GameMng.I._range.rangeTileReset();
            }
        }
    }
}