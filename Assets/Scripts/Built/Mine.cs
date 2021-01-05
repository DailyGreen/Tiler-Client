using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Built
{
    public int making;  // °ñµå »ý»ê·®
    public static int cost = 1;   // °Ç¼³ ºñ¿ë
    // Start is called before the first frame update
    void Start()
    {
        _name = "±¤»ê";
        _desc = "°ñµå¸¦ Ä¶ ¼ö ÀÖ´Ù";
        _hp = 10;
        making = 5;
    }

    // Update is called once per frame
    void Update()
    {
        MakingGold();
        if (Input.GetMouseButtonDown(0))
            Building(this.gameObject);
    }

    /**
     * @brief °ñµå »ý»ê
     * 
     */
    void MakingGold()
    {
        if (Input.GetKeyDown("g"))
            GameMng.I.addGold(making);
    }
}
