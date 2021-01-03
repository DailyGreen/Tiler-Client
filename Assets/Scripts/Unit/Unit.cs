using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /*
     * 유닛 고유 스탯
     * 반드시 상속받는 자식의 Start 에서 값을 변경해 주시길 바랍니다.
     */
    public int _hp = 0;
    public int _damage = 0;
    public int _cost = 0;

    /*
     * 유닛 설명
     * 유닛을 클릭했을때 설명해주는 변수입니다. 상속받는 최종 자식의 Start 에서 변경해 주시길 바랍니다.
     */
    public string _name = "";       // 오브젝트 고유 명칭
    public string _desc = "";       // 오브젝트 설명

}
