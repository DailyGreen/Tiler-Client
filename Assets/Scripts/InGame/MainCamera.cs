using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField]
    private GameObject UserListPanel;

    // �̸��� ����
    bool onoffemote = false;

    void Start()
    {
        //MainCamera = Camera.main;
        zoomSize = _camera.orthographicSize;
    }

    void LateUpdate()
    {
        //CameraMove();
        MouseScrollzoom();
        // Ŭ���� Ÿ�� �̸� ���� �������°� (�ӽ�)
        if (Input.GetMouseButtonDown(0) && GameMng.I._UnitGM.act == ACTIVITY.NONE && GameMng.I._BuiltGM.act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            GameMng.I.mouseRaycast();
            if (GameMng.I.selectedTile)
                GameMng.I.clickTile(GameMng.I.selectedTile);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.transform.position = GameMng.I.CastlePos;
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            UserListPanel.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            UserListPanel.SetActive(false);
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

        //pos.x = Mathf.Clamp(pos.x, -limitPos.x, limitPos.x);
        //pos.y = Mathf.Clamp(pos.y, -limitPos.y, limitPos.y);
        this.transform.position = pos;
    }
    
    void EmoteControl()
    {
        if (onoffemote)
        {
            _camera.cullingMask = ~(1 << 3);
        }
        else
        {
            _camera.cullingMask = -1;
        }
    }
}
