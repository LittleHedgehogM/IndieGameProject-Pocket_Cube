using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        //await Task.Delay(300);
        SetActive(false);
        Destroy(gameObject);

        if (UIManager.Instance.panelDict.ContainsKey(name))
        {
            //await Task.Delay(300);
            UIManager.Instance.panelDict.Remove(name);
        }
    }

    
}
