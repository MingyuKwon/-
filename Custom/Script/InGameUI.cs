using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI width;
    public TextMeshProUGUI height;


    [Space]
    public TextMeshProUGUI mineCount;
    public TextMeshProUGUI treasureCount;

    [Header("Heart Panel")]
    public Sprite heartFill;
    public Sprite heartEmpty;
    public Sprite heartNone;
    public RectTransform[] heartPanels; 
    private Image[] heartImages = new Image[9];

    private void Awake() {
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                heartImages[i*3 + j] = heartPanels[i].GetChild(j).GetComponent<Image>();
                heartImages[i*3 + j].sprite = heartNone;
            }
        }
    }


    private void OnEnable() {
        EventManager.instance.mine_treasure_count_Change_Event += Change_Mine_Treasure_Count;
        EventManager.instance.Set_Width_Height_Event += Set_Width_Height;
        EventManager.instance.Set_Heart_Event += Set_Heart;
    }

    private void OnDisable() {
        EventManager.instance.mine_treasure_count_Change_Event -= Change_Mine_Treasure_Count;
        EventManager.instance.Set_Width_Height_Event -= Set_Width_Height;
        EventManager.instance.Set_Heart_Event -= Set_Heart;
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


    int changeSizeUnit = 3;
    IEnumerator changeTextColorShortly(TextMeshProUGUI textMeshProUGUI, Color standardColor, Color changeColor)
    {
        textMeshProUGUI.color = changeColor;
        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize - changeSizeUnit;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize - changeSizeUnit;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize - changeSizeUnit;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize - changeSizeUnit;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize + changeSizeUnit;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize + changeSizeUnit;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize + changeSizeUnit;

        yield return new WaitForSeconds(0.02f);

        textMeshProUGUI.fontSize = textMeshProUGUI.fontSize + changeSizeUnit;
        textMeshProUGUI.color = standardColor;
    }

    private void Set_Heart(int currentHeart, int maxHeart)
    {
        for(int i=0; i<maxHeart; i++)
        {
            heartImages[i].sprite = heartEmpty;
        }

        for(int i=0; i<currentHeart; i++)
        {
            heartImages[i].sprite = heartFill;
        }
    }

    private void Set_Width_Height(Vector2 vector2)
    {
        width.text = "Width : " + vector2.x.ToString();
        height.text = "Height : " + vector2.y.ToString();
    }

}
