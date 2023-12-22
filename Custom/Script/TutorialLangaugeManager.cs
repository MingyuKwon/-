using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialLangaugeManager : MonoBehaviour
{
    public TextMeshProUGUI[] tutorial1Title;
    public TextMeshProUGUI[] tutorial1Text;

    public TextMeshProUGUI[] tutorial2Title;
    public TextMeshProUGUI[] tutorial2Text;

    public TextMeshProUGUI[] tutorial3Title;
    public TextMeshProUGUI[] tutorial3Text;

    public TextMeshProUGUI[] tutorial4Title;
    public TextMeshProUGUI[] tutorial4Text;


    private string[] tutorial1TitleStringEnglish = {
    "Basics of the Game",
    "Game Clear",
    "Numbers on the Floor",
    "Game Over",
    };
    private string[] tutorial1TextStringEnglish = {
    "The goal is to find all the treasures on each stage to proceed to the next level, reaching the final floor.",
    "Underneath the rocks, there might be treasures to find or traps that can cause damage. We must try to avoid traps and collect all treasures.",
    "This means that the sum of traps + treasures in the 8 surrounding tiles is 3. The exact number of each is unknown.",
    "The left white number indicates the number of traps among the 8 surrounding tiles, while the right yellow number indicates the number of treasures.",
    "There are three conditions for game over:",
    "If you fall into a trap, your health decreases, and the game is over when health reaches 0. Be careful, as trap damage increases with difficulty and levels.",
    "If the time limit is exceeded, it's game over.",
    "Removing a treasure while trying to clear a trap results in game over.",
    };
    private string[] tutorial2TitleStringEnglish;
    private string[] tutorial2TextStringEnglish;
    private string[] tutorial3TitleStringEnglish;
    private string[] tutorial3TextStringEnglish;
    private string[] tutorial4TitleStringEnglish;
    private string[] tutorial4TextStringEnglish;

    private string[] tutorial1TitleStringKorean = {
        "게임의 기초",
        "게임 클리어",
        "바닥의 숫자",
        "게임 오버",
    };
    private string[] tutorial1TextStringKorean = {
        "각 스테이지에 있는 모든 보물을 찾아내 다음 층으로 넘어가, 최종 층에 도달하는 것이 목표입니다",
        "바위 아래에는 찾아야 하는 보물이나, 피해를 입을 수 있는 함정이 있을 수도 있습니다. 우리는 최대한 함정을 피해서 보물을 전부 얻어야 합니다.",
        "바닥 주변 8개의 칸에 함정 + 보물의 개수가 3개가 된다는 것을 의미합니다. 각각 몇개가 있는지는 알 수 없습니다 ",
        "왼쪽 휜색 숫자가 주변 8칸 중 함정의 개수,  오른쪽 노란색 숫자가 보물의 개수입니다.",
        "게임 오버가 되는 조건은 3가지가 있습니다 ",
        "함정에 걸리면 체력이 줄어듭고, 그러다 체력이 0이 되면 게임 오버가 됩니다. 난이도와 층이  올라감에 따라 함정의 데미지가 올라가니 주의합시다.",
        "시간제한을 넘기게 되면 게임 오버가 됩니다.",
        "함정을 제거하려다 보물을 제거하면 게임 오버입니다.",
    };
    private string[] tutorial2TitleStringKorean;
    private string[] tutorial2TextStringKorean;
    private string[] tutorial3TitleStringKorean;
    private string[] tutorial3TextStringKorean;
    private string[] tutorial4TitleStringKorean;
    private string[] tutorial4TextStringKorean;

    private void OnEnable() {
        UpdatePanel("");
    }

    private void UpdatePanel(string s)
    {
        string[] tutorial1TitleString = tutorial1TitleStringEnglish;
        string[] tutorial1TextString  = tutorial1TextStringEnglish;
        string[] tutorial2TitleString = tutorial2TitleStringEnglish;
        string[] tutorial2TextString  = tutorial2TextStringEnglish;
        string[] tutorial3TitleString = tutorial3TitleStringEnglish;
        string[] tutorial3TextString  = tutorial3TextStringEnglish;
        string[] tutorial4TitleString = tutorial4TitleStringEnglish;
        string[] tutorial4TextString  = tutorial4TextStringEnglish;
        if(LanguageManager.currentLanguage == "English")
        {
            tutorial1TitleString = tutorial1TitleStringEnglish;
            tutorial1TextString  = tutorial1TextStringEnglish;
            tutorial2TitleString = tutorial2TitleStringEnglish;
            tutorial2TextString  = tutorial2TextStringEnglish;
            tutorial3TitleString = tutorial3TitleStringEnglish;
            tutorial3TextString  = tutorial3TextStringEnglish;
            tutorial4TitleString = tutorial4TitleStringEnglish;
            tutorial4TextString  = tutorial4TextStringEnglish;
        }else
        {
            tutorial1TitleString = tutorial1TitleStringKorean;
            tutorial1TextString  = tutorial1TextStringKorean;
            tutorial2TitleString = tutorial2TitleStringKorean;
            tutorial2TextString  = tutorial2TextStringKorean;
            tutorial3TitleString = tutorial3TitleStringKorean;
            tutorial3TextString  = tutorial3TextStringKorean;
            tutorial4TitleString = tutorial4TitleStringKorean;
            tutorial4TextString  = tutorial4TextStringKorean;
        }

        for(int i=0; i<tutorial1Title.Length; i++)
        {
            tutorial1Title[i].text = tutorial1TitleString[i];
        }
        for(int i=0; i<tutorial1Text.Length; i++)
        {
            tutorial1Text[i].text = tutorial1TextString[i];
        }
        for(int i=0; i<tutorial2Title.Length; i++)
        {
            tutorial2Title[i].text = tutorial2TitleString[i];
        }
        for(int i=0; i<tutorial2Text.Length; i++)
        {
            tutorial2Text[i].text = tutorial2TextString[i];
        }
        for(int i=0; i<tutorial3Title.Length; i++)
        {
            tutorial3Title[i].text = tutorial3TitleString[i];
        }
        for(int i=0; i<tutorial3Text.Length; i++)
        {
            tutorial3Text[i].text = tutorial3TextString[i];
        }
        for(int i=0; i<tutorial4Title.Length; i++)
        {
            tutorial4Title[i].text = tutorial4TitleString[i];
        }
        for(int i=0; i<tutorial4Text.Length; i++)
        {
            tutorial4Text[i].text = tutorial4TextString[i];
        }

    }

}
