using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : Object
{
    public int _hp;
    public int _max_hp;
    //public int _cost;

    public int _uniqueNumber;      // 플레이어 구별 코드
    public SpriteRenderer _emoteSide;      // 말풍선 이모지 색

    /**
     * 유닛 애니메이션 관리
     */
    public Animator _anim;
    public AnimationClip dyingClip;

    /**
     * 유닛 턴제 관리
     */
    public int createCount = 0;             // 생성시 걸리는 턴 수 
    public int maxCreateCount = 0;          // 생성되는 턴 수 (한번 정해지면 수정되선 안됨)

    /**
     * 유닛 거리 관리
     */
    public int _basedistance = 0;           // 이동 거리 또는 생성 거리
    public int _attackdistance = 0;         // 공격 거리

    public int SaveX, SaveY;                // 명령을 내린 오브젝트의 위치를 저장할 변수

    public bool _bActAccess = true;         // 오브젝트들의 행동 제어

    public void DestroyMyself()
    {
        _anim.SetTrigger("isDying");

        Destroy(this.gameObject, dyingClip.length - .2f);
    }

    public Color GetUserColor()
    {
        Color color;
        ColorUtility.TryParseHtmlString(CustomColor.TransColor(NetworkMng.getInstance.myColor), out color);
        return color;
    }
}