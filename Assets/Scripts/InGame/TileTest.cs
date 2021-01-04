using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTest : MonoBehaviour
{
    public GameObject grassgams;
    public GameObject[][] TileArr;
    public Transform parenttr;
    float fXPos = -8.0f;
    float fYPos = 4.0f;
    float fXAddPos = 1.24f;
    float fYAddPos = 1.09f;

    // Start is called before the first frame update
    void Start()
    {
        instanti();
    }

    // Update is called once per frame
    void Update()
    {
        //Tileinfo();
    }

    //void Tileinfo()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //        Ray2D ray = new Ray2D(pos, Vector2.zero);

    //        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

    //        if (hit.collider != null)
    //        {
    //            Debug.Log(hit.collider.gameObject.transform.localPosition);
    //        }

    //    }
    //}

    void instanti()
    {
        for (int i = 0; i < 16; i++)
        {
            fYPos -= fYAddPos;
            for (int j = 0; j < 30; j++)
            {
                if (i % 2 == 0)
                {
                    if (j == 0)
                        fXPos = -7.37f;
                }
                else
                {
                    if (j == 0)
                        fXPos = -8.0f;
                }
                Instantiate(grassgams, new Vector3(fXPos, fYPos, 0f), Quaternion.identity, parenttr);
                fXPos += fXAddPos;

            }
        }
    }
    
}
