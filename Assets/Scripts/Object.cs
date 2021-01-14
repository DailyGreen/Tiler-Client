using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    /*
     * 오브젝트 고유 코드
     * 해당 코드로 어떤 오브젝트인지 구별이 가능해집니다.
     */
    public int _code = 0;

    /*
     * 오브젝트 설명
     * 오브젝트를 클릭했을때 설명해주는 변수입니다. 상속받는 최종 자식의 Start 에서 변경해 주시길 바랍니다.
     */
    public string _name = "";       // 오브젝트 고유 명칭
    public string _desc = "";       // 오브젝트 설명

    //public int uniqueNumber;      // 플레이어 구별 코드

    public List<ACTIVITY> _activity = new List<ACTIVITY>();

}