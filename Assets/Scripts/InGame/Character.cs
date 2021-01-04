using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    Tile TileCs = null;

    GameObject TileGams = null;

    Vector2 pos;

    Ray2D ray;

    RaycastHit2D hit;

    [SerializeField]
    private float fCharSpeed = 0.0f;

    private float TileDistance = 0.0f;

    private bool bCharMove = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Tileinfo();
        CharMove();
    }

    void Tileinfo()
    {


        if (Input.GetMouseButtonDown(1))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            ray = new Ray2D(pos, Vector2.zero);

            hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider.tag.Equals("Tile"))
            {
                TileCs = hit.collider.gameObject.GetComponent<Tile>();
                TileGams = hit.collider.gameObject;
                TileDistance = Vector2.Distance(gameObject.transform.localPosition, TileGams.transform.localPosition);                  //타일이 눌렸을때 캐릭터와 클릭한 타일간 거리 계산
                if (!bCharMove)
                {
                    bCharMove = true;

                }
                else
                {
                    if (TileDistance <= 6f)                                                                                            //캐릭터와 타일간 거리가 1.5Xa 이하일시 움직일수 있음 (거리 한칸당 1.24?정도 되드라)
                    {
                        //transform.localPosition = TileCs.GetTileVec2;
                        //transform.localPosition = Vector2.Lerp(gameObject.transform.localPosition, TileCs.GetTileVec2, 0.1f);

                        bCharMove = false;
                    }
                }
                Debug.Log(TileCs.Code);
            }
        }

    }

    void CharMove()
    {
        if ((Vector2.Distance(gameObject.transform.localPosition, TileGams.transform.localPosition) >= 0.01f) && TileDistance <= 6f) 
        {
            transform.localPosition = Vector2.Lerp(gameObject.transform.localPosition, TileCs.GetTileVec2, fCharSpeed * Time.deltaTime);
        }
        else if((Vector2.Distance(gameObject.transform.localPosition, TileGams.transform.localPosition) <= 0.01f) && TileDistance <= 6f)
        {
            transform.localPosition = TileCs.GetTileVec2;
        }
    }
}


