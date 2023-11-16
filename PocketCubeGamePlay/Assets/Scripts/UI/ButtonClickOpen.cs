using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickOpen : BasePanel
{
    public void ClickOpenTutotialWindowPanel()
    {
        UIManager.Instance.OpenPanel(UIConst.TutotialWindowPanel);
    }
}
