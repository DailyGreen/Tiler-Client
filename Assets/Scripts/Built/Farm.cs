using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Built
{
    public int making;              // �ķ� ���귮

    void Awake()
    {
        _name = "����";
        _code = (int)BUILT.FARM;
        maxCreateCount = 3;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void Start()
    {
        _name = string.Format("{0} ���� ����  (������ : {1})", GameMng.I.getUserTribe(_uniqueNumber), GameMng.I.getUserName(_uniqueNumber));
        _emoteSide.color = GetUserColor(_uniqueNumber);

        switch ((int)NetworkMng.getInstance.myTribe)
        {
            case 0:     // �� ����
                _max_hp = 7;
                _hp = _max_hp;
                making = 5;
                break;
            case 1:     // �� ����
                _max_hp = 7;
                _hp = _max_hp;
                making = 5;
                break;
            case 2:     // �縷 ����
                _max_hp = 7;
                _hp = _max_hp;
                making = 5;
                break;
        }
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }
    public void waitingCreate()
    {
        createCount++;
        _desc = "�������� " + (maxCreateCount - createCount) + "�� ����";

        // 2�� �Ŀ� ������
        if (createCount > maxCreateCount - 1)
        {
            _desc = "�ķ��� �����Ѵ�";

            _anim.SetTrigger("isSpawn");

            GameMng.I.RemoveDelegate(this.waitingCreate);

            GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._bActAccess = true;
            GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._anim.SetBool("isWorking", false);

            // �������
            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();

                GameMng.I.AddDelegate(MakingFood);
            }
        }
    }

    /**
     * @brief �ķ� ����
     */
    void MakingFood()
    {
        _anim.SetTrigger("isMaking");
        GameMng.I.addFood(making);
    }

    void OnDestroy()
    {
        if (createCount > maxCreateCount - 1 && NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            GameMng.I.RemoveDelegate(MakingFood);
        else
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
