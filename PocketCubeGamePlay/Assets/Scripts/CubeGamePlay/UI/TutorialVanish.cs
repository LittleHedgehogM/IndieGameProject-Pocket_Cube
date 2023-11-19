using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVanish : MonoBehaviour
{
    [SerializeField] [Range (3, 8)]private int sleepAfterSeconds;
    private IEnumerator VanishAfterSeconds()
    {
        yield return new WaitForSeconds (sleepAfterSeconds);
        
        this.gameObject.SetActive (false);
    }

    public void SetTutorialInvisible() 
    {      
        StartCoroutine (VanishAfterSeconds ());
    }


}
