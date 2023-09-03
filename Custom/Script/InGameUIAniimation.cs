using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIAniimation : MonoBehaviour
{
    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        EventManager.instance.Set_UI_Filter_Event += Set_UI_Filter;
    }

    private void OnDisable() {
        EventManager.instance.Set_UI_Filter_Event -= Set_UI_Filter;
    }

    bool isShowingMinimap = false;
    public void ShowMinimap()
    {
        if(StageManager.isNowInitializing) return;
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

    private void Set_UI_Filter(EventType eventType)
    {
        if(StageManager.isNowInitializing) return;

        switch(eventType)
        {
            case EventType.MineAppear :
                animator.SetTrigger("Mine Find");
                break;
            case EventType.TreasureAppear :
                animator.SetTrigger("Trea Find");
                break;
        }
    }
}
