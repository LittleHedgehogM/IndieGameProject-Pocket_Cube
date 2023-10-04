using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPuzzleController : MonoBehaviour
{
    [SerializeField] Electro_Switch leftSwitch;
    [SerializeField] Electro_Switch rightSwitch;
    [SerializeField] Electro_Circuit leftCircuit;
    [SerializeField] Electro_Circuit rightCircuit;
    [SerializeField] Electro_Circuit centerCircuit;

    bool isLeftSwitchOn = false;
    bool isRightSwitchOn = false;

    private void OnEnable()
    {
        Electro_Switch.SwitchColorTranslationFinished += OnAnySwitchUpdated;
        Electro_Circuit.CircuitChange += onAnyCircuitUpdated;
    }


    private void OnDisable()
    {
        Electro_Switch.SwitchColorTranslationFinished -= OnAnySwitchUpdated; 
        Electro_Circuit.CircuitChange -= onAnyCircuitUpdated;

    }

    private void OnAnySwitchUpdated()
    {
        if (isLeftSwitchOn != leftSwitch.isElectroSwitchOn())
        {
            isLeftSwitchOn = leftSwitch.isElectroSwitchOn();
            leftCircuit.changeCircuitColor(isLeftSwitchOn);
        }
        else if (isRightSwitchOn != rightSwitch.isElectroSwitchOn())
        {
            isRightSwitchOn = rightSwitch.isElectroSwitchOn();
            rightCircuit.changeCircuitColor(isRightSwitchOn);
        }
        
    }

    private IEnumerator Wait()
    {
        
        yield return new WaitForSeconds(1);
        centerCircuit.changeCircuitColor(true);
    }

    private void onAnyCircuitUpdated()
    {
        if (leftSwitch.isElectroSwitchOn() && rightSwitch.isElectroSwitchOn())
        {
            centerCircuit.changeCircuitColor(true);
        }
        else if (centerCircuit.getIsCircuitValid())
        {
            centerCircuit.changeCircuitColor(false);
        }
        
        
    }

    public bool isStarPuzzleSolved()
    {
        return isLeftSwitchOn && isRightSwitchOn;
    }

    public void setInteractionEnabled(bool isEnabled)
    {
        leftSwitch.setInteractionEnabled(isEnabled);
        rightSwitch.setInteractionEnabled(isEnabled);

    }

    // Update is called once per frame
    //void Update()
    //{
    //    isLeftSwitchOn = leftSwitch.isElectroSwitchOn();
    //    isRightSwitchOn = rightSwitch.isElectroSwitchOn();
    //    leftCircuit.changeCircuitColor(isLeftSwitchOn);
    //    rightCircuit.changeCircuitColor(isRightSwitchOn);
    //    centerCircuit.changeCircuitColor(isLeftSwitchOn && isRightSwitchOn);

    //}
}
