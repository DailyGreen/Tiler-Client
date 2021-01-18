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
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";
        _hp = 10;
        _code = (int)BUILT.MINE;
        making = 5;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.DESTROY_BUILT);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";

        // 2턴 후에 생성됨
        if (createCount > 2)
        {
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
     * @brief  골드 약탈 공격력 퍼센트 (밸런스 조정 필요)
     * @param attactdmg 공격유닛 대미지
     * @param attacker 공격한 사람의 고유 번호
     * @param attected 공격 당한 사람 고유 번호
     */
    public void GoldPlunder(int attactdmg, int attacker, int attected)
    {
        if (GameMng.I.selectedTile._builtObj._uniqueNumber.Equals(attected))
            NetworkMng.getInstance.SendMsg(string.Format("GOLD_PLUNDER:{0}:{1}:{2}", attacker, attected, GameMng.I._gold * attactdmg / 100));
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
        if (createCount > 2 && NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
            GameMng.I.RemoveDelegate(MakingGold);
        else
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
