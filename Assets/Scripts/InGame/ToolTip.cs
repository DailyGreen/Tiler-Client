using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text tooltipText;
    [SerializeField]
    RectTransform bgRectTransform;
    [SerializeField]
    RectTransform myTransform;

    static ToolTip instance;
    [SerializeField]
    Camera _camera;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    void Update()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myTransform, Input.mousePosition, _camera, out mousePos);
        transform.localPosition = mousePos;
    }

    void ShowToolTip(string msg)
    {
        gameObject.SetActive(true);

        tooltipText.text = msg;
        float txtPdSize = 8f;
        Vector2 bgSize = new Vector2(
            tooltipText.preferredWidth + txtPdSize * 2f + 10,
            tooltipText.preferredHeight + txtPdSize * 2f + 5);
        bgRectTransform.sizeDelta = bgSize;

    }

    void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowToolTip_Static(string msg)
    {
        instance.ShowToolTip(msg);
    }

    public static void HideToolTip_Static()
    {
        instance.HideToolTip();
    }

}
