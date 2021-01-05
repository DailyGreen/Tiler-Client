using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Object
{
    /*
     * 유닛 고유 스탯
     * 반드시 상속받는 자식의 Start 에서 값을 변경해 주시길 바랍니다.
     */
    public int _hp = 0;
    public int _damage = 0;
    public int _cost = 0;

    // 유닛 움직일때 필요한 변수들
    Tile TileCs = null;
    public Tile NowTile = null;                                                                   //현재 타일 정보를 알아오는 변수
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
                return 1;   // 위로 올라감
            }
            else if(TileCs.PosY.Equals(NowTile.PosY))
            {
                return 2;   // x 축따라감
            }
        }

        return -1;       // 100% 불가능
    }

    /**
     * @brief RaycastHit2D로 타일 정보 알아와서 캐릭터 이동 계산
     * @param 캐릭터 GameObject
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
                    NowTile._unitObj = null;        // 떠났을때 지워짐
                }

                TileCs = hit.collider.gameObject.GetComponent<Tile>();
                //NowTile = TileCs;

                TileGams = hit.collider.gameObject;
                TileDistance = Vector2.Distance(CharGame.transform.localPosition, TileGams.transform.localPosition);                  // 타일이 눌렸을때 캐릭터와 클릭한 타일간 거리 계산

                if (!bCharMove)
                {
                    bCharMove = true;
                }
            }
        }

        if (bCharMove)
        {
            TileCs._unitObj = this;    // 세팅
            if (Vector2.Distance(CharGame.transform.localPosition, TileGams.transform.localPosition) >= 0.01f && TileDistance <= 6f)      // 캐릭터와 타일간 거리가 1.5 * a 이하일시 움직일수 있음 (거리 한칸당 1.24?정도 되드라)
            {
                CharGame.transform.localPosition = Vector2.Lerp(CharGame.transform.localPosition, TileCs.GetTileVec2, fCharSpeed * Time.deltaTime);        //타일 간 부드러운 이동
                if (CharGame.transform.localPosition.x < TileGams.transform.localPosition.x)                                                                 //가는 방향 회전 오른쪽
                {
                    CharGame.transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
                }
                else if (CharGame.transform.localPosition.x == TileGams.transform.localPosition.x)                                                           //방향 변동 X
                {

                }
                else
                {
                    CharGame.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));                                                                 //가는 방향 회전 왼쪽
                }
            }
            else if (TileDistance <= 6f)                                                                                                            //남은 거리가 좁아지면 타일 위치로 자동 이동
            {
                CharGame.transform.localPosition = TileCs.GetTileVec2;
                bCharMove = false;
            }
        }
    }
}