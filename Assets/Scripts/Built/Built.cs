using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Built : Object
{
    /*
     * �ǹ� ����  ����
     * �ݵ�� ��ӹ޴� �ڽ��� Start ���� ���� ������ �ֽñ� �ٶ��ϴ�.
     */
    public int _hp = 0;

    public RaycastHit2D hit;

    /**
     * @brief �ǹ��� ����
     * @param ������ child ���ӿ�����Ʈ
     */
    public void Building(GameObject built)
    {
        hit = GameMng.I.MouseLaycast();
        if (GameMng.I.GetTileCs._builtObj == null)
        {
           
            {
                Instantiate(built, GameMng.I.GetTileCs.transform);
                GameMng.I.GetTileCs._builtObj = this;
            }
        }
    }
}