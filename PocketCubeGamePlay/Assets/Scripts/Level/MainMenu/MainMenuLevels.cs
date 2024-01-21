using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuLevels : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed;

    [SerializeField] private GameObject level0Active;
    [SerializeField] private GameObject level0Deactive;

    [SerializeField] private GameObject level1Active;
    [SerializeField] private GameObject level1Deactive;

    [SerializeField] private GameObject level2Active;
    [SerializeField] private GameObject level2Deactive;

    [SerializeField] private GameObject level3Active;  
    [SerializeField] private GameObject level3Deactive;

    [Header("Level Pose Rotation")]
    [SerializeField] private Vector3 cubeRotation0;
    [SerializeField] private Vector3 cubeRotation1;
    [SerializeField] private Vector3 cubeRotation2;
    [SerializeField] private Vector3 cubeRotation3;



    [Header("Level Status")]
    [SerializeField] private int levelStatus;
    [SerializeField] private GameObject frontPage;
    [SerializeField] private Button pressAnyKey;

    private bool stopAutoRotate = false;
    Quaternion toRotation0;
    Quaternion toRotation1;
    Quaternion toRotation2;
    Quaternion toRotation3;
    [SerializeField]float lerpSpeed;
    Quaternion fromRotation;
    [SerializeField]float rotationSpeed = 1f;
    //mousebutton
    private float pressedTime;
    void Awake()
    {
        gameObject.GetComponent<Outline>().OutlineWidth = 0;
        pressAnyKey.onClick.AddListener(OnPressAnyKey);

        levelStatus = PlayerPrefs.GetInt("Level");

        if (levelStatus >= 0)
        {
            //active
            level0Active.SetActive(true);
            level0Deactive.SetActive(false);
            //de-active
            level1Active.SetActive(false);
            level1Deactive.SetActive(true);
            level2Active.SetActive(false);
            level2Deactive.SetActive(true);
            level3Active.SetActive(false);
            level3Deactive.SetActive(true);

            rotateTutorial.SetActive(false);
            if (levelStatus >= 1)
            {
                level1Active.SetActive(true);
                level1Deactive.SetActive(false);

                if (levelStatus >= 2)
                {
                    level2Active.SetActive(true);
                    level2Deactive.SetActive(false);

                    if (levelStatus >= 3)
                    {
                        level3Active.SetActive(true);
                        level3Deactive.SetActive(false);
                        if (levelStatus == 4)
                        {
                            stopAutoRotate = true;
                            gameObject.GetComponent<Outline>().OutlineWidth = 10;
                            frontPage.SetActive(false);
                            UIManager.Instance.OpenPanel(UIConst.MainMenuPanel);
                            rotateTutorial.SetActive(true);
                        }
                    }
                }
            }
        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
        toRotation0 = Quaternion.Euler(cubeRotation0);
        toRotation1 = Quaternion.Euler(cubeRotation1);
        toRotation2 = Quaternion.Euler(cubeRotation2);
        toRotation3 = Quaternion.Euler(cubeRotation3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            
            pressedTime = 0;
        }

        if (!stopAutoRotate)
        {
            StartMenuCubeAutoRotate();
        }
        

        if (stopAutoRotate)
        {
            fromRotation = transform.rotation;
            switch (levelStatus)
            {
                case 0:
                    
                    transform.rotation = Quaternion.Lerp(fromRotation, toRotation0, Time.deltaTime * lerpSpeed);

                    break;

                case 1:
                    //Debug.Log("case 1" + fromRotation + toRotation1);
                    transform.rotation = Quaternion.Lerp(fromRotation, toRotation1, Time.deltaTime * lerpSpeed);

                    break;

                case 2:

                    transform.rotation = Quaternion.Lerp(fromRotation, toRotation2, Time.deltaTime * lerpSpeed);

                    break;

                case 3:

                    transform.rotation = Quaternion.Lerp(fromRotation, toRotation3, Time.deltaTime * lerpSpeed);

                    break;
            }
        }

        
    }

    private void OnEnable()
    {
        LevelManager.SceneChangeInprogress += RotateTutorialOutOnSceneChange;
    }
    private void OnDisable()
    {
        LevelManager.SceneChangeInprogress -= RotateTutorialOutOnSceneChange;
    }

    void OnPressAnyKey()
    {
        
            stopAutoRotate = true;
       


    }

    public void StartMenuCubeAutoRotate()
    {
        transform.Rotate(_rotation * _speed * Time.deltaTime);
    }

    //rotate cube toturial
    [SerializeField] private GameObject rotateTutorial;
    private bool tutorialExist = true;
    //private bool pressedKey;

    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !(levelStatus == 4))//如果有UI阻挡着就返回不响应了
            return;
        
        if (tutorialExist)
        {
            pressedTime += Time.deltaTime;
            print(pressedTime);        
            if (pressedTime > 1f)
            {
                
                tutorialExist = false;
                StartCoroutine(RotateTutorialOut());
            }
        }     
        float XRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        //float YRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
        transform.Rotate(Vector3.down, XRotation);
        //transform.Rotate(Vector3.right, YRotation);

    }

    private IEnumerator RotateTutorialOut()
    {
        print("tutorial disable");
        rotateTutorial.SetActive(false);
        yield return null;
    }
    private void RotateTutorialOutOnSceneChange()
    {
        rotateTutorial.SetActive(false);
    }
}
