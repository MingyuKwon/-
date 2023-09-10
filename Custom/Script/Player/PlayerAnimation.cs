using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;

    float animationX = 0;
    float animationY = 0;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        CheckRotate();
    }

    public void SetAnimationXY(float x, float y)
    {
        animationX = x;
        animationY = y;

        animator.SetFloat("X", animationX);
        animator.SetFloat("Y", animationY);
        animator.SetFloat("LastX", animationX);
        animator.SetFloat("LastY", animationY);
    }

    private void CheckRotate() // 현재 보고 있는 방향에 따라 콜라이더 등을 전체 돌려줌
    {
        if(animationX == 0)
        {
            if(animationY == 1)
            {
                transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3 (0, 0, 180));
            }else if(animationY == -1)
            {
                transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(new Vector3 (0, 0, 0));
            }
        }else if(animationY == 0)
        {
            if(animationX == 1)
            {
                transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(new Vector3 (0, 0, 90));
            }else if(animationX == -1)
            {
                transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(new Vector3 (0, 0, 270));
            }
        }
    }

    public void SetAnimationPlayingFlag(int flag)
    {
        if(flag == 0)
        {
            EventManager.isAnimationPlaying = false;
        }else
        {
            EventManager.isAnimationPlaying = true;
        }
        
    }

    public void MoveAnimation(Vector3Int vector3Int)
    {        
        if(vector3Int == Vector3Int.forward)
        { // 텔레포트
            animator.SetFloat("isNear", 1);
        }else if(vector3Int == Vector3Int.up)
        { 
            animator.SetFloat("isNear", 0);
            SetAnimationXY(0,1);
        }else if(vector3Int == Vector3Int.down)
        { 
            animator.SetFloat("isNear", 0);
            SetAnimationXY(0,-1);
        }else if(vector3Int == Vector3Int.right)
        { 
            animator.SetFloat("isNear", 0);
            SetAnimationXY(1,0);
        }else if(vector3Int == Vector3Int.left)
        { 
            animator.SetFloat("isNear", 0);
            SetAnimationXY(-1,0);
        }

        animator.SetTrigger("Move");
    }
}
