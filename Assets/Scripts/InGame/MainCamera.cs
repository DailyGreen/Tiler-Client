using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    // ī�޶� �ܿ� ���̴� ������
    private float zoomSize;
    private const float zoomScale = 10f;
    private const float zoomLerpSpeed = 10f;
    [SerializeField]
    private float minZoom = 8f;
    [SerializeField]
    private float maxZoom = 4.5f;
    private float scrollData;

    // ----
    // ī�޶� ������ ����
    [SerializeField]
    private Vector3 limitPos;
    public float fMoveSpeed = 10f;
    private const float borderThickness = 10f;      // ���콺�� ��ũ�� �ۿ� ��� ����( �β� )
    void Start()
    {
        //MainCamera = Camera.main;
        zoomSize = _camera.orthographicSize;
    }

    void LateUpdate()
    {
        CameraMove();
        MouseScrollzoom();
        // Ŭ���� Ÿ�� �̸� ���� �������°� (�ӽ�)\
        if (Input.GetMouseButtonDown(0) && GameMng.I._UnitGM.act == ACTIVITY.NONE)
        {
            GameMng.I.mouseRaycast();
            GameMng.I.clickTile(GameMng.I.selectedTile);
        }
    }

    /**
      * @brief ���콺 ��ũ�ѷ� ī�޶� ��
      */
    void MouseScrollzoom()
    {
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        zoomSize -= scrollData * zoomScale;
        zoomSize = Mathf.Clamp(zoomSize, maxZoom, minZoom);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, zoomSize, Time.deltaTime * zoomLerpSpeed);
    }

    /**
     * @brief ���콺 �巡�׷� ī�޶� ������
     */
    void CameraMove()
    {
        Vector3 pos = this.transform.position;
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - borderThickness)
        {
            pos.y += fMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= borderThickness)
        {
            pos.y -= fMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - borderThickness)
        {
            pos.x += fMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= borderThickness)
        {
            pos.x -= fMoveSpeed * Time.deltaTime;
        }
        pos.z = -20;

        pos.x = Mathf.Clamp(pos.x, -limitPos.x, limitPos.x);
        pos.y = Mathf.Clamp(pos.y, -limitPos.y, limitPos.y);
        this.transform.position = pos;
    }
}
