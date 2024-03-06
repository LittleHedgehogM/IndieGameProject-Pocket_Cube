using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils : MonoBehaviour
{
   public static bool isMouseOverUI()
   {
        
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> resultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, resultList);

        for (int i = 0; i < resultList.Count; i++) 
        {
            if (resultList[i].gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                Debug.Log("hit name : " + resultList[i].gameObject.name);
                return true;
            }
        }

        return false;
   }

}
