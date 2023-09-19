using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    public int NextLevel;

    public Animator transition;
    public float transitionTime = 1.0f;
    // Update is called once per frame


    public void RestartLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(NextLevel));
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        // play animation
        transition.SetTrigger("StartSceneTransition");       
        
        // wait
        yield return new WaitForSeconds(transitionTime);

        //load scene
        SceneManager.LoadScene(levelIndex);

    }
}

