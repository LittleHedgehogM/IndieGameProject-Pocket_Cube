using System;
using UnityEngine;

public class Origin_RotationTarget : MonoBehaviour
{
    GameObject go;
    Material material_phase_one;
    Material material_phase_two;
    Material material_phase_three;
    
    [SerializeField] private Color colorWhite;
    [SerializeField] private Color colorGreen;
    [SerializeField] private Animator animator;

    Color reflect01;
    Color reflect02;
    Color diffuse01;
    Color diffuse02;

    public static Action TargetVanished;

    // Start is called before the first frame update
    void Awake()
    {
        go = this.gameObject;
        material_phase_one = go.GetComponent<MeshRenderer>().materials[8];
        material_phase_two = go.GetComponent<MeshRenderer>().materials[10];
        material_phase_three = go.GetComponent<MeshRenderer>().materials[1];
        reflect01 = material_phase_one.GetColor("_ReflectOff01");
        reflect02 = material_phase_one.GetColor("_Reflect02");
        diffuse01 = material_phase_one.GetColor("_diffusegradient01");
        diffuse02 = material_phase_one.GetColor("_diffusegradient02");

    }

    public void InitPhaseOne(){
        material_phase_one.SetColor("_ReflectOff01", colorGreen);
        material_phase_one.SetColor("_Reflect02", colorGreen);
        material_phase_one.SetColor("_diffusegradient01", colorGreen);
        material_phase_one.SetColor("_diffusegradient02", colorGreen);
    }

    public void FinishPhaseOne()
    {
        material_phase_one.SetColor("_ReflectOff01", reflect01);
        material_phase_one.SetColor("_Reflect02", reflect02);
        material_phase_one.SetColor("_diffusegradient01", diffuse01);
        material_phase_one.SetColor("_diffusegradient02", diffuse02);
    }

    public void InitPhaseTwo()
    {
        material_phase_two.SetColor("_ReflectOff01", colorGreen);
        material_phase_two.SetColor("_Reflect02", colorGreen);
        material_phase_two.SetColor("_diffusegradient01", colorGreen);
        material_phase_two.SetColor("_diffusegradient02", colorGreen);
    }


    public void FinishPhaseTwo()
    {
        material_phase_two.SetColor("_ReflectOff01", reflect01);
        material_phase_two.SetColor("_Reflect02", reflect02);
        material_phase_two.SetColor("_diffusegradient01", diffuse01);
        material_phase_two.SetColor("_diffusegradient02", diffuse02);
    }

    public void InitPhaseThree()
    {
        material_phase_three.SetColor("_ReflectOff01", colorGreen);
        material_phase_three.SetColor("_Reflect02", colorGreen);
        material_phase_three.SetColor("_diffusegradient01", colorGreen);
        material_phase_three.SetColor("_diffusegradient02", colorGreen);
    }


    public void FinishPhaseThree()
    {
        material_phase_three.SetColor("_ReflectOff01", reflect01);
        material_phase_three.SetColor("_Reflect02", reflect02);
        material_phase_three.SetColor("_diffusegradient01", diffuse01);
        material_phase_three.SetColor("_diffusegradient02", diffuse02);
    }

    public void minimizeTargetAndshowCube()
    {
        animator.Play("FirstSceneTargetAnimation");
    }

    public void targetVanish()
    {
        // Sphere Animation Finish
        TargetVanished?.Invoke();
    }

}
