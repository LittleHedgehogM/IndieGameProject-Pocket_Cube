using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isRemoved = false;
    protected new string name;


    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    public virtual void OpenPanel(string name)
    {
        this.name = name;
        SetActive(true);
    }

    public virtual void ClosePanel()
    {
        isRemoved = true;
        SetActive(false);
        Destroy(gameObject);

        if (UIManager.Instance.panelDict.ContainsKey(name))
        {
            UIManager.Instance.panelDict.Remove(name);
        }
    }

    
}
