using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScrp : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;

    // 카메라 줌에 쓰이는 변수들
    private float zoomSize;
    private const float zoomScale = 10f;
    private const float zoomLerpSpeed = 10f;
    [SerializeField]
    private float fMinzoom = 8f;
    [SerializeField]
    private float fMaxzoom = 4.5f;
    private float scrollData;

    // ----
    // 카메라 움직임 쓰임
    [SerializeField]
    private Vector3 lLmitPos;
    public float fMoveSpeed = 10f;
    private const float fBoderthickness = 10f;      // 마우스가 스크린 밖에 닿는 범위( 두께 )
    void Start()
    {
        MainCamera = Camera.main;
        zoomSize = MainCamera.orthographicSize;
    }

    void LateUpdate()
    {
        CameraMove();
        MouseScrollzoom();
    }

    /**
      * @brief 마우스 스크롤로 카메라 줌
      */
    void MouseScrollzoom()
    {
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        zoomSize -= scrollData * zoomScale;
        zoomSize = Mathf.Clamp(zoomSize, fMaxzoom, fMinzoom);
        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, zoomSize, Time.deltaTime * zoomLerpSpeed);
    }

    /**
     * @brief 마우스 드래그로 카메라 움직임
     */
    void CameraMove()
    {
        Vector3 pos = this.transform.position;
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - fBoderthickness)
        {
            pos.y += fMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= fBoderthickness)
        {
            pos.y -= fMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - fBoderthickness)
        {
            pos.x += fMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= fBoderthickness)
        {
            pos.x -= fMoveSpeed * Time.deltaTime;
        }
        pos.z = -20;

        pos.x = Mathf.Clamp(pos.x, -lLmitPos.x, lLmitPos.x);
        pos.y = Mathf.Clamp(pos.y, -lLmitPos.y, lLmitPos.y);
        this.transform.position = pos;
    }
}
