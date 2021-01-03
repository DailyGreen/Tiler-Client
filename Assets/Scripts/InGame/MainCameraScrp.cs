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

    // ----
    // 드래그 동작에 쓰임
    private Vector3 touchStart;
    private float scrollData;

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
       if(Input.GetMouseButtonDown(0))
        {
            touchStart = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

       if(Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - MainCamera.ScreenToWorldPoint(Input.mousePosition);
            MainCamera.transform.position += direction; 
        }
    }
}
