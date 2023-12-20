using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyPanel : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Toggle[] buttons;
    public TextMeshProUGUI[] startItem;
    public TextMeshProUGUI[] defaultPlusItem;
    public TextMeshProUGUI[] ItemPlusItem;
    public TextMeshProUGUI[] ItemUpPercent;
    public TextMeshProUGUI noItem;
    public TextMeshProUGUI trapDamage;

    public void UpdateDifficultyPanel()
    {
        Debug.Log("UpdateDifficultyPanel " + StageInformationManager.difficulty.ToString());

        int difficulty = (int)StageInformationManager.difficulty;        

        startItem[0].text = (StageInformationManager.Potion_Default + StageInformationManager.plusPotion_Default_perStage[difficulty]).ToString();
        startItem[1].text = (StageInformationManager.Mag_Default + StageInformationManager.plusMag_Default_perStage[difficulty]).ToString();
        startItem[2].text = (StageInformationManager.Holy_Default + StageInformationManager.plusHoly_Default_perStage[difficulty]).ToString();
        startItem[3].text = (StageInformationManager.DefaultTimeperStage[difficulty] + StageInformationManager.plusTime_byItem_perStage[difficulty]).ToString();
    
        defaultPlusItem[0].text = (StageInformationManager.plusPotion_Default_perStage[difficulty]).ToString();
        defaultPlusItem[1].text = (StageInformationManager.plusMag_Default_perStage[difficulty]).ToString();
        defaultPlusItem[2].text = (StageInformationManager.plusHoly_Default_perStage[difficulty]).ToString();
        defaultPlusItem[3].text = (StageInformationManager.plusTime_byItem_perStage[difficulty]).ToString();

        ItemPlusItem[0].text = (StageInformationManager.plusPotion_byItem_perStage[difficulty]).ToString();
        ItemPlusItem[1].text = (StageInformationManager.plusMag_byItem_perStage[difficulty]).ToString();
        ItemPlusItem[2].text = (StageInformationManager.plusHoly_byItem_perStage[difficulty]).ToString();
        ItemPlusItem[3].text = (StageInformationManager.plusTime_byItem_perStage[difficulty]).ToString();

        ItemUpPercent[0].text = (StageInformationManager.item_obtain_Up_Percentage[difficulty] * 100).ToString() + "%";
        ItemUpPercent[1].text = (StageInformationManager.item_obtain_Up_Percentage[difficulty] * 100).ToString() + "%";
        ItemUpPercent[2].text = (StageInformationManager.item_obtain_Up_Percentage[difficulty] * 100).ToString() + "%";
        ItemUpPercent[3].text = (StageInformationManager.item_obtain_Up_Percentage[difficulty] * 100 / 2).ToString() + "%";

        noItem.text = (StageInformationManager.noItemRatio[difficulty] * 100).ToString() + "%";
        trapDamage.text = StageInformationManager.DefaultTrapDamage[difficulty].ToString();
    }

    private void OnEnable() {
        Debug.Log("OnEnable " + StageInformationManager.difficulty.ToString());
        UpdateDifficulty();
    }

    

    public void UpdateDifficulty()
    {
        
        int difficulty = (int)StageInformationManager.difficulty;
        Toggle selectedToggle = buttons[difficulty];
        Debug.Log("UpdateDifficulty " + StageInformationManager.difficulty.ToString() + "\n" + selectedToggle.transform.parent.name);
        
        // Toggle Group을 사용하여 선택한 Toggle 활성화
        toggleGroup.SetAllTogglesOff(); // 모든 토글을 끄고
        selectedToggle.isOn = true; // 선택한 토글만 켭니다.
    }

    public void ChangeDifficulty(int difficulty)
    {
        StageInformationManager.difficulty = (Difficulty)difficulty;
        Debug.Log("ChangeDifficulty " + StageInformationManager.difficulty.ToString());
        UpdateDifficultyPanel();
    }
}
