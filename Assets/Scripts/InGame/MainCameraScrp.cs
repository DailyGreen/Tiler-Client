using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScrp : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;

    // ī�޶� �ܿ� ���̴� ������
    private float zoomSize;
    private const float zoomScale = 10f;
    private const float zoomLerpSpeed = 10f;
    [SerializeField]
    private float fMinzoom = 8f;
    [SerializeField]
    private float fMaxzoom = 4.5f;

    // ----
    // �巡�� ���ۿ� ����
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
      * @brief ���콺 ��ũ�ѷ� ī�޶� ��
      */
    void MouseScrollzoom()
    {
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        zoomSize -= scrollData * zoomScale;
        zoomSize = Mathf.Clamp(zoomSize, fMaxzoom, fMinzoom);
        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, zoomSize, Time.deltaTime * zoomLerpSpeed);
    }

    /**
     * @brief ���콺 �巡�׷� ī�޶� ������
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
