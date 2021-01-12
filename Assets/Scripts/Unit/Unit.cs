using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : DynamicObject
{
    /*
     * 유닛 고유 스탯
     * 반드시 상속받는 자식의 Start 에서 값을 변경해 주시길 바랍니다.
     */
    public int _damage = 0;
}