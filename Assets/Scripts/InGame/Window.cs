using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour
{
    public string hoverMessage;

    public void MouseEnter()
    {
        ToolTip.ShowToolTip_Static(hoverMessage);
    }

    public void MouseExit()
    {
        ToolTip.HideToolTip_Static();
    }
}
