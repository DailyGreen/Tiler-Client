using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuiltMng : MonoBehaviour
{
    public ACTIVITY act = ACTIVITY.NONE;


    public GameObject[] unitobj = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && act != ACTIVITY.ACTING && GameMng.I._UnitGM.act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (act)
            {
                case ACTIVITY.WORKER_UNIT_CREATE:
                    CreateUnit((int)UNIT.WORKER);
                    break;
            }
        }


        if (Input.GetMouseButtonDown(0) && GameMng.I._UnitGM.act == ACTIVITY.NONE && act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            GameMng.I._range.AttackrangeTileReset();                                                     //Ŭ���� �ͷ� ���� ���� �ʱ�ȭ
            GameMng.I.mouseRaycast();
            if (GameMng.I.selectedTile._builtObj != null)
            {
                if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING)
                {
                    GameMng.I.selectedTile._builtObj.GetComponent<Turret>().Attack();
                }
            }
        }
    }

    public void CreateUnit(/*int cost,*/ int index)
    {
        GameMng.I.mouseRaycast(true);                       //ĳ���� ������ Ÿ�� ������ �˾ƿ;��ؼ� false���� true�� ����
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            GameObject Child = Instantiate(unitobj[index - 100], GameMng.I.targetTile.transform) as GameObject;                 // enum �� - 100
            Child.transform.parent = transform.parent;
            GameMng.I.targetTile._unitObj = Child.GetComponent<Worker>();
            GameMng.I._range.rangeTileReset();
            act = ACTIVITY.ACTING;
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

}
