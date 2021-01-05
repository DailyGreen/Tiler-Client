using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Object
{
    /*
     * ���� ���� ����
     * �ݵ�� ��ӹ޴� �ڽ��� Start ���� ���� ������ �ֽñ� �ٶ��ϴ�.
     */
    public int _hp = 0;
    public int _damage = 0;
    public int _cost = 0;

    // ���� �����϶� �ʿ��� ������
    Tile TileCs = null;
    public Tile NowTile = null;                                                                   //���� Ÿ�� ������ �˾ƿ��� ����
    GameObject TileGams = null;

    Vector2 pos;

    Ray2D ray;

    RaycastHit2D hit;

    [SerializeField]
    private float fCharSpeed = 0.0f;
    [SerializeField]
    private float TileDistance = 0.0f;
    [SerializeField]
    public bool bCharMove = false;

    int resultx;
    int resulty;
    int result;

    int Checkroad()
    {
        if (NowTile != null)
        {
            resultx = TileCs.PosX + NowTile.PosX;
            resulty = TileCs.PosY + NowTile.PosY;
        }
        result = resultx + resulty;
        if (result <= 2 || result >= -2)
        {
            if (TileCs.PosX.Equals(NowTile.PosX))
            {
                return 1;   // ���� �ö�
            }
            else if(TileCs.PosY.Equals(NowTile.PosY))
            {
                return 2;   // x �����
            }
        }

        return -1;       // 100% �Ұ���
    }

    /**
     * @brief RaycastHit2D�� Ÿ�� ���� �˾ƿͼ� ĳ���� �̵� ���
     * @param ĳ���� GameObject
     */
    public void CharClickMove(GameObject CharGame)
    {
        if (Input.GetMouseButtonDown(1))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            ray = new Ray2D(pos, Vector2.zero);

            hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider.tag.Equals("Tile"))
            {
                if (NowTile != null)
                {
                    NowTile._unitObj = null;        // �������� ������
                }

                TileCs = hit.collider.gameObject.GetComponent<Tile>();
                //NowTile = TileCs;

                TileGams = hit.collider.gameObject;
                TileDistance = Vector2.Distance(CharGame.transform.localPosition, TileGams.transform.localPosition);                  // Ÿ���� �������� ĳ���Ϳ� Ŭ���� Ÿ�ϰ� �Ÿ� ���

                if (!bCharMove)
                {
                    bCharMove = true;
                }
            }
        }

        if (bCharMove)
        {
            TileCs._unitObj = this;    // ����
            if (Vector2.Distance(CharGame.transform.localPosition, TileGams.transform.localPosition) >= 0.01f && TileDistance <= 6f)      // ĳ���Ϳ� Ÿ�ϰ� �Ÿ��� 1.5 * a �����Ͻ� �����ϼ� ���� (�Ÿ� ��ĭ�� 1.24?���� �ǵ��)
            {
                CharGame.transform.localPosition = Vector2.Lerp(CharGame.transform.localPosition, TileCs.GetTileVec2, fCharSpeed * Time.deltaTime);        //Ÿ�� �� �ε巯�� �̵�
                if (CharGame.transform.localPosition.x < TileGams.transform.localPosition.x)                                                                 //���� ���� ȸ�� ������
                {
                    CharGame.transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
                }
                else if (CharGame.transform.localPosition.x == TileGams.transform.localPosition.x)                                                           //���� ���� X
                {

                }
                else
                {
                    CharGame.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));                                                                 //���� ���� ȸ�� ����
                }
            }
            else if (TileDistance <= 6f)                                                                                                            //���� �Ÿ��� �������� Ÿ�� ��ġ�� �ڵ� �̵�
            {
                CharGame.transform.localPosition = TileCs.GetTileVec2;
                bCharMove = false;
            }
        }
    }
}