using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMng : MonoBehaviour
{
    public delegate void CountTurn();
    public CountTurn countDel;
    private static GameMng _Instance = null;

    //public bool wantToBuilt = false;
    [SerializeField]
    UnityEngine.UI.Text objectNameTxt;          // 선택 오브젝트 이름
    [SerializeField]
    UnityEngine.UI.Text objectDetailTxt;        // 선택 오브젝트 디테일
    [SerializeField]
    UnityEngine.UI.Text hpText;                 // HP 디테일

    public static GameMng I
    {
        get
        {
            if (_Instance.Equals(null))
            {
                Debug.Log("instance is null");
            }
            return _Instance;
        }
    }

    void Awake()
    {
        _Instance = this;
    }

    //public ProduceWorkMan produceworkman;

    // 턴 세기
    public void AddDelegate(CountTurn Method)
    {
        this.countDel += Method;
    }
    public void RemoveDelegate(CountTurn Method)
    {
        this.countDel -= Method;
    }

    /**
     * @brief 오브젝트를 클릭했을때
     * @param 으로 클릭한 오브젝트가 뭔지 알려줘야됨
     */
    public void clickObject(/* 뭘 클릭했는지 넘겨주기 */)
    {
        // 만약 지형이면
        /// 지형 에 대해 알려주기

        // 캐릭터면
        /// 캐릭터 에 대해 알려주기

        objectNameTxt.text = "오브젝트 이름";
        objectDetailTxt.text = "오브젝트 설명";
        hpText.text = "오브젝트 설명";
    }
}