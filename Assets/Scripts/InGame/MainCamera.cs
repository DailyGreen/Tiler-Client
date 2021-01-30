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

    // ���̺� ����Ʈ
    Object[] savePoints = new Object[5];
    [SerializeField]
    UnityEngine.UI.Button[] savePointBT;

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

            GameMng.I._range.AttackrangeTileReset();    // Ŭ���� �ͷ� ���� ���� �ʱ�ȭ

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
        // ���̺� ����Ʈ�� �̵��ϱ�
        else if (Input.GetKeyDown(KeyCode.Alpha1) && savePoints[0] != null) GoToSavePoint(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && savePoints[1] != null) GoToSavePoint(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3) && savePoints[2] != null) GoToSavePoint(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4) && savePoints[3] != null) GoToSavePoint(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5) && savePoints[4] != null) GoToSavePoint(4);

        // ���̺� ����Ʈ �����ϱ�
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
     * @brief �̸�Ʈ ��Ʈ��
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
     * @brief �� �� ��ġ�� �̵�
     */
    public void GoToMyCastle()
    {
        Vector3 pos = GameMng.I.CastlePos;
        pos.z = -10;
        Camera.main.transform.position = pos;
    }

    /**
     * @brief �� ���̺� ����Ʈ�� ���
     * @param checkingPoint ����� ������Ʈ�� �ƴϸ� ������ ������� ����
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
     * @brief �� ���̺� ����Ʈ ��ġ�� �̵��Ҷ�
     * @param i �ε��� ��
     */
    public void GoToSavePoint(int i)
    {
        Vector3 pos = savePoints[i].transform.position;
        pos.z = -10;
        Camera.main.transform.position = pos;
    }

    /**
     * @brief �� ���̺� ����Ʈ ����
     * @param i �ε��� ��
     */
    public void SavePoint(Object obj, int i)
    {
        savePoints[i] = obj;
        savePointBT[i].interactable = true;
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
