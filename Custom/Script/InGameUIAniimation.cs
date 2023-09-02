using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIAniimation : MonoBehaviour
{
    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    bool isShowingMinimap = false;
    public void ShowMinimap()
    {
        if(EventManager.isAnimationPlaying) return;

        if(!isShowingMinimap)
        {
            StageManager.stageInputBlock++;
            isShowingMinimap = true;
            animator.SetTrigger("Minimap Show");
        }else
        {
            StageManager.stageInputBlock--;
            isShowingMinimap = false;
            animator.SetTrigger("Minimap Close");
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
}
