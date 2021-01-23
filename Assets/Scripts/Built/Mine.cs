using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int making;              // 골드 생산량
    public static int cost = 1;     // 건설 비용

    void Awake()
    {
        _name = "광산";
        _max_hp = 10;
        _hp = _max_hp;
        _code = (int)BUILT.MINE;
        making = 5;
        maxCreateCount = 3;
        SaveX = GameMng.I.selectedTile.PosX;
        SaveY = GameMng.I.selectedTile.PosZ;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (maxCreateCount - createCount) + "턴 남음";


        // 2턴 후에 생성됨
        if (createCount > maxCreateCount - 1)
        {
            //GameMng.I._hextile.GetCell(SaveX, SaveY)._unitObj.GetComponent<Worker>()._bActAccess = false;

            _desc = "골드를 캘 수 있다";

            _anim.SetTrigger("isSpawn");

            GameMng.I.RemoveDelegate(this.waitingCreate);

            // 내꺼라면
            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            {
                init();

                GameMng.I.AddDelegate(MakingGold);
            }
        }
    }

    /**
     * @brief 골드 생산
     */
    void MakingGold()
    {
        _anim.SetTrigger("isMaking");
        GameMng.I.addGold(making);
    }

    void OnDestroy()
    {
        if (createCount > maxCreateCount - 1 && NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            GameMng.I.RemoveDelegate(MakingGold);
        else
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
