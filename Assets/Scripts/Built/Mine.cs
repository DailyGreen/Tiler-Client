using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int stack;   // 보유 골드량
    public int making;  // 생산 골드량

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    void Init ()
    {
        _name = "광산";
        _desc = "골드를 캘 수 있다";
        _hp = 10;
        _cost = 0;
        stack = 0;
        making = 5;
    }

    // Update is called once per frame
    void Update()
    {
        MakingGold();
    }

    /// <summary>
    /// 골드를 제조함
    /// </summary>
    void MakingGold()
    {
        if (Input.GetKey("g"))
            stack += making;
    }
}
