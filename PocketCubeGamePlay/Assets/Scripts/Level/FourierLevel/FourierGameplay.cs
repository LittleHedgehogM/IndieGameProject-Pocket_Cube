using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourierGameplay : MonoBehaviour
{
    FourierScriptableObject levelData;
    public List<GameObject> levels;
    FourierCameraController cameraController;
    FourierPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = FindObjectOfType<FourierCameraController>();
        player           = FindObjectOfType<FourierPlayer>();

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < levels.Count; i++)
        {

            
        }

        cameraController.onUpdateCameraWithPlayerMovement(player.getMovementDirection());

    }

    public void ColorChanger()
    {
        
    }
}
