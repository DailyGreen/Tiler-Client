using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : Object
{
    public int _hp;
    public int _max_hp;
    //public int _cost;

    public int _uniqueNumber;      // 플레이어 구별 코드

    /*
     * 유닛 애니메이션 관리
     */
    public Animator _anim;
    public AnimationClip dyingClip;

    public int createCount = 0;            // 생성시 걸리는 턴 수 
    public int maxCreateCount = 0;         // 생성되는 턴 수 (한번 정해지면 수정되선 안됨)

    public void DestroyMyself()
    {
        _anim.SetTrigger("isDying");

        Destroy(this.gameObject, dyingClip.length - .2f);
    }
}