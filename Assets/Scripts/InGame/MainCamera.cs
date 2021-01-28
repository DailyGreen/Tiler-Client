using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    // 카메라 줌에 쓰이는 변수들
    private float zoomSize;
    private const float zoomScale = 10f;
    private const float zoomLerpSpeed = 10f;
    [SerializeField]
    private float minZoom = 8f;
    [SerializeField]
    private float maxZoom = 4.5f;
    private float scrollData;

    // ----
    // 카메라 움직임 쓰임
    [SerializeField]
    private Vector3 limitPos;
    public float fMoveSpeed = 10f;
    private const float borderThickness = 10f;      // 마우스가 스크린 밖에 닿는 범위( 두께 )
    [SerializeField]
    private GameObject UserListPanel;

    // 이모지 설정
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
        // 클릭시 타일 이름 내용 가져오는곳 (임시)
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
      * @brief 마우스 스크롤로 카메라 줌
      */
    void MouseScrollzoom()
    {
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        zoomSize -= scrollData * zoomScale;
        zoomSize = Mathf.Clamp(zoomSize, maxZoom, minZoom);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, zoomSize, Time.deltaTime * zoomLerpSpeed);
    }

    /**
     * @brief 마우스 드래그로 카메라 움직임
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
