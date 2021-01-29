using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillitaryBase : Built
{
    public GameObject CreatingUnitobj = null;

    public static int cost = 10;   // �Ǽ� ���

    public int maintenanceCost = 0;   // ���� ���

    public int CreatingUnitX = 0;
    public int CreatingUnitY = 0;

    void Awake()
    {
        _name = "���� ����";
        _max_hp = 10;
        _hp = _max_hp;
        _code = (int)BUILT.MILLITARY_BASE;
        maxCreateCount = 3;
        maintenanceCost = 3;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void Start()
    {
        _name = string.Format("{0} ���� ���� ����  (������ : {1})", GameMng.I.getUserTribe(_uniqueNumber), GameMng.I.getUserName(_uniqueNumber));
        _emoteSide.color = GetUserColor(_uniqueNumber);
    }

    void init()
    {
        _activity.Add(ACTIVITY.SOLDIER_0_UNIT_CREATE);
        _activity.Add(ACTIVITY.SOLDIER_1_UNIT_CREATE);
        _activity.Add(ACTIVITY.SOLDIER_2_UNIT_CREATE);
        _activity.Add(ACTIVITY.WITCH_0_UNIT_CREATE);
        _activity.Add(ACTIVITY.WITCH_1_UNIT_CREATE);
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";
        // 2�� �Ŀ� ������
        if (createCount > maxCreateCount - 1)
        {
            _desc = "���µ��� �����Ѵ�";

            _anim.SetTrigger("isSpawn");

            GameMng.I.RemoveDelegate(this.waitingCreate);

            GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._bActAccess = true;
            GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._anim.SetBool("isWorking", false);

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();
                GameMng.I.AddDelegate(maintenance);
            }
        }
    }

    public void maintenance()
    {
        GameMng.I.minGold(maintenanceCost);
    }


    public static void CreateAttackFirstUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.SOLDIER_0_UNIT_CREATE;
            GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        }

    }

    public static void CreateAttackSecondUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.SOLDIER_1_UNIT_CREATE;
            GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        }
    }

    public static void CreateAttackThirdUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.SOLDIER_2_UNIT_CREATE;
            GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        }
    }

    public static void CreateAttackFourthUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.WITCH_0_UNIT_CREATE;
            GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        }
    }

    public static void CreateAttackFifthUnitBtn()
    {
        if (GameMng.I.selectedTile._code == (int)BUILT.MILLITARY_BASE)
        {
            GameMng.I._BuiltGM.act = ACTIVITY.WITCH_1_UNIT_CREATE;
            GameMng.I._range.moveRange((int)UNIQEDISTANCE.DISTANCE);
        }
    }

    void OnDestroy()
    {
        if (createCount < maxCreateCount - 1)
            GameMng.I.RemoveDelegate(waitingCreate);
        else
            GameMng.I.RemoveDelegate(maintenance);

        if (CreatingUnitobj != null)
        {
            Destroy(CreatingUnitobj);
            GameMng.I._hextile.TilecodeClear(CreatingUnitX, CreatingUnitY);
        }
    }
}
