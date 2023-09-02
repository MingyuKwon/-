using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public TextMeshProUGUI width;
    public TextMeshProUGUI height;


    [Space]
    public TextMeshProUGUI mineCount;
    public TextMeshProUGUI treasureCount;

    private void OnEnable() {
        EventManager.instance.mine_treasure_count_Change_Event += Change_Mine_Treasure_Count;
        EventManager.instance.Set_Width_Height_Event += Set_Width_Height;
    }

    private void OnDisable() {
        EventManager.instance.mine_treasure_count_Change_Event -= Change_Mine_Treasure_Count;
        EventManager.instance.Set_Width_Height_Event -= Set_Width_Height;
    }

    private void Change_Mine_Treasure_Count(EventType eventType, int count)
    {
        switch(eventType)
        {
            case EventType.MineAppear :
                mineCount.text = count.ToString();
                StartCoroutine(changeTextColorShortly(mineCount, Color.red, Color.white));
                break;
            case EventType.MineDisappear :
                mineCount.text = count.ToString();
                StartCoroutine(changeTextColorShortly(mineCount, Color.red, Color.white));
                break;
            case EventType.TreasureAppear :
                treasureCount.text = count.ToString();
                StartCoroutine(changeTextColorShortly(treasureCount, Color.yellow, Color.white));
                break;
            case EventType.TreasureDisappear :
                treasureCount.text = count.ToString();
                StartCoroutine(changeTextColorShortly(treasureCount, Color.yellow, Color.white));
                break;
        }

        
    }

    IEnumerator changeTextColorShortly(TextMeshProUGUI textMeshProUGUI, Color standardColor, Color changeColor)
    {
        textMeshProUGUI.color = changeColor;
        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize - 2;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize - 2;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize - 2;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize - 2;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize + 2;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize + 2;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize + 2;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize + 2;
        textMeshProUGUI.color = standardColor;
    }

    private void Set_Width_Height(Vector2 vector2)
    {
        width.text = "Width : " + vector2.x.ToString();
        height.text = "Height : " + vector2.y.ToString();
    }
}
