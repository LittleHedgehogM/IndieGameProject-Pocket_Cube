using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAnim : MonoBehaviour
{
    [SerializeField]private GameObject roundOne;
    [SerializeField]private GameObject roundTwo;
    [SerializeField] private GameObject roundThr;
    [SerializeField] private GameObject roundFour;
    [SerializeField] private GameObject roundFive;
    [SerializeField] private GameObject roundSix;
    private Animator animatorOne;
    private Animator animatorTwo;
    private Animator animatorThr;
    private Animator animatorFour;
    private Animator animatorFive;
    private Animator animatorSix;

    private int beatCount = 0;
    // Start is called before the first frame update
    private void Start()
    {
        animatorOne = roundOne.GetComponent<Animator>();
        animatorTwo = roundTwo.GetComponent<Animator>();
        animatorThr = roundThr.GetComponent<Animator>();
        animatorFour = roundFour.GetComponent<Animator>();
        animatorFive = roundFive.GetComponent<Animator>();
        animatorSix = roundSix.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PushTransitions()
    {
        //print(beatCount);     

        if (beatCount == 0)
        {
            animatorOne.Play("Pawn_0_Up");
        }
        else if (beatCount == 1)
        {
            animatorTwo.Play("Pawn_4_Up");
        }
        else if (beatCount == 2)
        {
            animatorThr.Play("Pawn_1_Up");
        }
        else if (beatCount == 3)
        {
            animatorFour.Play("Pawn_2_Up");
        }
        else if (beatCount == 4)
        {
            animatorFive.Play("Pawn_5_Up");
        }
        else if (beatCount == 5)
        {
            animatorSix.Play("Pawn_3_Up");
        }


        else if (beatCount == 6)
        {
            animatorOne.Play("Pawn_0_Down");
        }
        else if (beatCount == 7)
        {
            animatorTwo.Play("Pawn_4_Down");
        }
        else if (beatCount == 8)
        {
            animatorThr.Play("Pawn_1_Down");
        }
        else if (beatCount == 9)
        {
            animatorFour.Play("Pawn_2_Down");
        }
        else if (beatCount == 10)
        {
            animatorFive.Play("Pawn_5_Down");
        }
        else if (beatCount == 11)
        {
            animatorSix.Play("Pawn_3_Down");
        }


        beatCount++;
        if (beatCount == 12)
        {
            beatCount = 0;
        }
        //print("transition On");
    }
}
