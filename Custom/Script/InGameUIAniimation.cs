using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIAniimation : MonoBehaviour
{
    public TextMeshProUGUI gameOverReason;
    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        EventManager.instance.Set_UI_Filter_Event += Set_UI_Filter;
        EventManager.instance.Game_Over_Event += GameOverAnimation;
    }

    private void OnDisable() {
        EventManager.instance.Set_UI_Filter_Event -= Set_UI_Filter;
        EventManager.instance.Game_Over_Event -= GameOverAnimation;
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

    bool isShowingMenu = false;
    public void ShowMenu()
    {
        if(StageManager.isNowInitializing) return;
        if(EventManager.isAnimationPlaying) return;

        if(!isShowingMenu)
        {
            StageManager.stageInputBlock++;
            isShowingMenu = true;
            animator.SetTrigger("Menu Show");
        }else
        {
            StageManager.stageInputBlock--;
            isShowingMenu = false;
            animator.SetTrigger("Menu Close");
        }

    }

    public void GameOverAnimation(bool isGameOver, GameOver_Reason reason){
        if(isGameOver)
        {
            switch(reason)
            {
                case GameOver_Reason.Heart0 :
                    gameOverReason.text = "HP ZERO";
                    gameOverReason.color = Color.red;
                    break;
                case GameOver_Reason.TreasureCrash :
                    gameOverReason.text = "Treasure Crash";
                    gameOverReason.color = Color.yellow;
                    break;
                case GameOver_Reason.TimeOver :
                    gameOverReason.text = "Time Over";
                    gameOverReason.color = Color.green;
                    break;
            }
            animator.SetTrigger("Game Over");
        }else
        {
            animator.SetTrigger("Restart");
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
            case EventType.MineDisappear :
                animator.SetTrigger("Mine Destroy");
                break;
            case EventType.TreasureAppear :
                animator.SetTrigger("Trea Find");
                break;
            case EventType.TreasureDisappear :
                animator.SetTrigger("Trea Destroy");
                break;
        }
    }
}
