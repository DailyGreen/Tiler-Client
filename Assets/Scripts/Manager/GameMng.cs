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
}