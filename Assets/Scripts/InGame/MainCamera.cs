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

    // 세이브 포인트
    Object[] savePoints = new Object[5];
    [SerializeField]
    UnityEngine.UI.Button[] savePointBT;

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

            GameMng.I._range.AttackrangeTileReset();    // 클릭시 터렛 공격 범위 초기화

            if (GameMng.I.selectedTile)
            {
                GameMng.I.clickTile(GameMng.I.selectedTile);
                if (GameMng.I.selectedTile._builtObj != null)
                {
                    if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING && NetworkMng.getInstance.uniqueNumber.Equals(GameMng.I.selectedTile._builtObj._uniqueNumber))
                    {
                        GameMng.I._range.attackRange(GameMng.I.selectedTile._builtObj.GetComponent<Turret>()._attackdistance);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !GameMng.I.isWriting)
        {
            GoToMyCastle();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            UserListPanel.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            UserListPanel.SetActive(false);
        }
        // 세이브 포인트로 이동하기
        else if (Input.GetKeyDown(KeyCode.Alpha1) && savePoints[0] != null) GoToSavePoint(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && savePoints[1] != null) GoToSavePoint(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3) && savePoints[2] != null) GoToSavePoint(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4) && savePoints[3] != null) GoToSavePoint(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5) && savePoints[4] != null) GoToSavePoint(4);

        // 세이브 포인트 저장하기
        if (Input.GetKey(KeyCode.LeftControl) && GameMng.I.selectedTile)
        {
            Object obj = null;
            if (GameMng.I.selectedTile._unitObj) obj = GameMng.I.selectedTile._unitObj;
            else if (GameMng.I.selectedTile._builtObj) obj = GameMng.I.selectedTile._builtObj;
            else obj = GameMng.I.selectedTile;

            if (Input.GetKeyDown(KeyCode.Alpha1))       SavePoint(obj, 0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))  SavePoint(obj, 1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))  SavePoint(obj, 2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))  SavePoint(obj, 3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))  SavePoint(obj, 4);
        }

    }

    /**
     * @brief 이모트 컨트롤
     */

    public void EmoteControl()
    {
        onoffemote = !onoffemote;

        if (onoffemote)
        {
            _camera.cullingMask = ~(1 << 3);
        }
        else
        {
            _camera.cullingMask = -1;
        }
    }

    /**
     * @brief 내 성 위치로 이동
     */
    public void GoToMyCastle()
    {
        Vector3 pos = GameMng.I.CastlePos;
        pos.z = -10;
        Camera.main.transform.position = pos;
    }

    /**
     * @brief 내 세이브 포인트를 지울떄
     * @param checkingPoint 저장된 오브젝트가 아니면 지움이 허락되지 않음
     */
    public void removeMySavePoints(Object checkingPoint)
    {
        for (int i = 0; i < savePoints.Length; i++)
        {
            if (savePoints[i] == checkingPoint)
            {
                savePoints[i] = null;
                savePointBT[i].interactable = false;
                break;
            }
        }
    }

    /**
     * @brief 내 세이브 포인트 위치로 이동할때
     * @param i 인덱스 값
     */
    public void GoToSavePoint(int i)
    {
        Vector3 pos = savePoints[i].transform.position;
        pos.z = -10;
        Camera.main.transform.position = pos;
    }

    /**
     * @brief 내 세이브 포인트 저장
     * @param i 인덱스 값
     */
    public void SavePoint(Object obj, int i)
    {
        savePoints[i] = obj;
        savePointBT[i].interactable = true;
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
        if ((Input.GetKey("w") || Input.mousePosition.y >= Screen.height - borderThickness) && !GameMng.I.isWriting)
        {
            pos.y += fMoveSpeed * Time.deltaTime;
        }
        if ((Input.GetKey("s") || Input.mousePosition.y <= borderThickness) && !GameMng.I.isWriting)
        {
            pos.y -= fMoveSpeed * Time.deltaTime;
        }
        if ((Input.GetKey("d") || Input.mousePosition.x >= Screen.width - borderThickness) && !GameMng.I.isWriting)
        {
            pos.x += fMoveSpeed * Time.deltaTime;
        }
        if ((Input.GetKey("a") || Input.mousePosition.x <= borderThickness) && !GameMng.I.isWriting)
        {
            pos.x -= fMoveSpeed * Time.deltaTime;
        }
        pos.z = -20;

        pos.x = Mathf.Clamp(pos.x, -limitPos.x, limitPos.x);
        pos.y = Mathf.Clamp(pos.y, -limitPos.y, limitPos.y);
        this.transform.position = pos;
    }
}
