using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonAnimationCtr : MonoBehaviour
{
    [Header("Animations")]
    private Animator _animator;
    private string _CurrentAnimation;


    void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator != null)
        {
            SetAnimation("BoyDrop");
            SetAnimation("GirlDrop");
        }
    }

    public void SetAnimation(string animationName)
    {
        float transitionDuration = 0.2f;
        if (_animator == null)
        {
            Debug.LogError("Animator is not assigned or found in the children.");
            return;
        }

        if (_CurrentAnimation != animationName)
        {
            _CurrentAnimation = animationName;
            _animator.CrossFade(animationName, transitionDuration);
        }
    }
}
