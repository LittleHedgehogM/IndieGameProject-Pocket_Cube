using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CoinAnimationController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Animator animController;

    bool isPlayingSprite = false;
    bool isPlayingSwallow = false;

    private void Start()
    {
        animController.enabled = false;
    }
    public void SpriteAnimFinished()
    {
        isPlayingSprite = false;
        animController.enabled = false;

    }
    public void SwallowAnimFinished()
    {
        isPlayingSwallow = false;
        animController.enabled = false;
    }

    public void PlaySwallowAnim(bool isLeft)
    {
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = true;
        animController.enabled = true;
        if (isLeft)
        {
            animController.Play("AM_CoinLeftSwallow");
        }
        else
        {
            animController.Play("AM_CoinRightSwallow");
        }
        isPlayingSwallow = true;
    }

    public void PlaySpriteAnim(bool isLeft)
    {
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = true;
        animController.enabled = true;
        if (isLeft)
        {
            animController.Play("AM_CoinLeftSprite");
        }
        else
        {
            animController.Play("AM_CoinRightSprite");
        }

        isPlayingSprite = true;
    }

}
