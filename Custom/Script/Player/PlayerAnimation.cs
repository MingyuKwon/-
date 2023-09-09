using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationXY(float x, float y)
    {
        animator.SetFloat("X", x);
        animator.SetFloat("Y", y);
        animator.SetFloat("LastX", x);
        animator.SetFloat("LastY", y);
    }
}
