using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    static public InGameUI instance;
    #region Serialize
    [Header("Texts")]
    public TextMeshProUGUI width;
    public TextMeshProUGUI height;

    [Space]
    public TextMeshProUGUI[] usableItemExplain;

    [Space]
    public TextMeshProUGUI leftTimeText;
    public TextMeshProUGUI elapsedTimeText;

    [Space]
    public TextMeshProUGUI mineCount;
    public TextMeshProUGUI treasureCount;

    [Space]
    public TextMeshProUGUI potionCount;
    public TextMeshProUGUI magGlassCount;
    public TextMeshProUGUI holyWaterCount;

    [Header("Heart Panel")]
    public Sprite heartFill;
    public Sprite heartEmpty;
    public Sprite heartNone;
    public Sprite[] heartReducingAnimationSprite;
    public RectTransform[] heartPanels; 

    [Space]
    [Header("Transforms")]
    float blackBoxMaxSize = 86f;
    public RectTransform SandClockTrans;
    public RectTransform upClockBlackBox;
    public RectTransform downClockBlackBox;

    [Space]
    public RectTransform totalItemPanel;
    public RectTransform[] ItemUses; // 0 : right, 1 : down, 2 : left, 3 : up
    private Button[] itemButtons; // 0 : right, 1 : down, 2 : left, 3 : up
    private Image[] itemimages; // 0 : right, 1 : down, 2 : left, 3 : up
    [Space]
    public Transform potionImage;
    public Transform magGlassImage;
    public Transform HolyWaterImage;

    #endregion

    private Image[] heartImages = new Image[9];
    private InGameUIAniimation inGameUIAniimation;

    public void ItemUse(int numtype)
    {
        EventManager.instance.ItemUse_Invoke_Event((ItemUseType)numtype, StageManager.instance.gapBetweenPlayerFocus);
        EventManager.instance.ItemPanelShow_Invoke_Event(Vector3Int.zero, false);
        StageManager.isNowInputtingItem = false;
    }

    private void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else
        {
            Destroy(this.gameObject);
        }
        
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                heartImages[i*3 + j] = heartPanels[i].GetChild(j).GetComponent<Image>();
                heartImages[i*3 + j].sprite = heartNone;
            }
        }

        inGameUIAniimation = GetComponent<InGameUIAniimation>();

        itemButtons = totalItemPanel.GetComponentsInChildren<Button>();
        itemimages = totalItemPanel.GetComponentsInChildren<Image>();

        usableItemExplain[0].text = "Restores 1 unit of health";
        usableItemExplain[1].text = "Displays numbers on the ground to distinguish between traps and treasures";
        usableItemExplain[2].text = "When sprinkled on an obstacle, it reveals whether there's a treasure underneath or not";
    }


    private void OnEnable() {
        EventManager.instance.mine_treasure_count_Change_Event += Change_Mine_Treasure_Count;
        EventManager.instance.Set_Width_Height_Event += Set_Width_Height;
        EventManager.instance.Reduce_Heart_Event += Reduce_Heart;
        EventManager.instance.Heal_Heart_Event += Heal_Heart;

        EventManager.instance.Item_Count_Change_Event += Change_Item_Count;
        EventManager.instance.ItemPanelShow_Event += ShowItemUsePanel;

        EventManager.instance.timerEvent += SetTimeTexts;
    }

    private void OnDisable() {
        EventManager.instance.mine_treasure_count_Change_Event -= Change_Mine_Treasure_Count;
        EventManager.instance.Set_Width_Height_Event -= Set_Width_Height;
        EventManager.instance.Reduce_Heart_Event -= Reduce_Heart;
        EventManager.instance.Heal_Heart_Event -= Heal_Heart;

        EventManager.instance.Item_Count_Change_Event -= Change_Item_Count;
        EventManager.instance.ItemPanelShow_Event -= ShowItemUsePanel;

        EventManager.instance.timerEvent -= SetTimeTexts;
    }

    public Vector2 WorldToCanvasPosition(Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition -= new Vector3(Screen.width /2 , Screen.height/2 , 0);

        return screenPosition;
    }


    private void ShowItemUsePanel(Vector3Int position, bool isShow, bool isHolyEnable = false, bool isCrachEnable = false, bool isMagEnable = false, bool isPotionEnable = false)
    {
        Vector2 screenPoint = WorldToCanvasPosition(TileGrid.CheckWorldPosition(position));

        if(isShow)
        {
            if(isHolyEnable)
            {
                itemButtons[0].interactable = true;
            }else
            {
                itemButtons[0].interactable = false;
            }

            if(isCrachEnable)
            {
                itemButtons[1].interactable = true;
            }else
            {
                itemButtons[1].interactable = false;
            }

            if(isMagEnable)
            {
                itemButtons[2].interactable = true;
            }else
            {
                itemButtons[2].interactable = false;
            }

            if(isPotionEnable)
            {
                itemButtons[3].interactable = true;
            }else
            {
                itemButtons[3].interactable = false;
            }

            totalItemPanel.anchoredPosition = screenPoint;

            inGameUIAniimation.Set_Item_Use(true);
            

        }else
        {
            inGameUIAniimation.Set_Item_Use(false);
        }
    }

    private void SetTimeTexts(int elapsedTime, int leftTime)
    {
        elapsedTimeText.text = elapsedTime.ToString();
        leftTimeText.text = leftTime.ToString();
        
        float percentageToChangeBlackBox = (float)elapsedTime / (elapsedTime + leftTime);
        upClockBlackBox.sizeDelta = new Vector2(upClockBlackBox.sizeDelta.x, percentageToChangeBlackBox * blackBoxMaxSize);
        downClockBlackBox.sizeDelta = new Vector2(downClockBlackBox.sizeDelta.x, (1 - percentageToChangeBlackBox) * blackBoxMaxSize);

        float changeDelta = percentageToChangeBlackBox * percentageToChangeBlackBox / 20;

        StartCoroutine(changeClockSizeShortly(changeDelta * 0.8f + 0.005f));
    }

    IEnumerator changeClockSizeShortly(float P)
    {
        float deltaP = P / 3;

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x - deltaP, SandClockTrans.localScale.y - deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x - deltaP, SandClockTrans.localScale.y - deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x - deltaP, SandClockTrans.localScale.y - deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x + deltaP, SandClockTrans.localScale.y + deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x + deltaP, SandClockTrans.localScale.y + deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x + deltaP, SandClockTrans.localScale.y + deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x + deltaP, SandClockTrans.localScale.y + deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x + deltaP, SandClockTrans.localScale.y + deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x + deltaP, SandClockTrans.localScale.y + deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x - deltaP, SandClockTrans.localScale.y - deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x - deltaP, SandClockTrans.localScale.y - deltaP,0);

        yield return new WaitForSeconds(0.04f);
        SandClockTrans.localScale = new Vector3(SandClockTrans.localScale.x - deltaP, SandClockTrans.localScale.y - deltaP,0);
    }

    [Button]
    private void Change_Item_Count(EventType eventType, Item usableItem , int count)
    {
        bool flag = false;

        inGameUIAniimation.SetItem_Use_Obtain_Flag(usableItem);
        
        if(eventType == EventType.Item_Use)
        {
            flag = false;
        }else if(eventType == EventType.Item_Obtain)
        {
            flag = true;
        }

        Debug.Log(eventType + " " +  usableItem + " " + count);

        switch(usableItem)
        {
            case Item.Potion :
                potionCount.text = ": " + count.ToString();
                if(StageManager.isNowInitializing) return;
                StartCoroutine(changeItemSize(potionImage ,potionCount , flag));
                break;
            case Item.Mag_Glass :
                magGlassCount.text = ": " + count.ToString();
                if(StageManager.isNowInitializing) return;
                StartCoroutine(changeItemSize(magGlassImage , magGlassCount , flag));
                break;
            case Item.Holy_Water :
                holyWaterCount.text = ": " + count.ToString();
                if(StageManager.isNowInitializing) return;
                StartCoroutine(changeItemSize(HolyWaterImage , holyWaterCount , flag));
                break;
        }
    }

    IEnumerator changeItemSize(Transform imageTransform, TextMeshProUGUI count, bool goBigger)
    {
        float changeSizeUnit = 0.03f;
        float C = (goBigger) ? 1 : -1;

        if(goBigger)
        {
            count.color = Color.green;
        }else
        {
            count.color = Color.red;
        }

        imageTransform.localScale = new Vector3(imageTransform.localScale.x + C * changeSizeUnit, imageTransform.localScale.y +  C *changeSizeUnit,0);
        yield return new WaitForSeconds(0.02f);

        imageTransform.localScale = new Vector3(imageTransform.localScale.x + C * changeSizeUnit, imageTransform.localScale.y +  C *changeSizeUnit,0);
        yield return new WaitForSeconds(0.02f);

        imageTransform.localScale = new Vector3(imageTransform.localScale.x + C * changeSizeUnit, imageTransform.localScale.y +  C *changeSizeUnit,0);
        yield return new WaitForSeconds(0.02f);

        imageTransform.localScale = new Vector3(imageTransform.localScale.x + C * changeSizeUnit, imageTransform.localScale.y +  C *changeSizeUnit,0);
        yield return new WaitForSeconds(0.02f);

        imageTransform.localScale = new Vector3(imageTransform.localScale.x + C * changeSizeUnit, imageTransform.localScale.y +  C *changeSizeUnit,0);
        yield return new WaitForSeconds(0.02f);

        imageTransform.localScale = new Vector3(imageTransform.localScale.x - C * changeSizeUnit, imageTransform.localScale.y -  C *changeSizeUnit,0);
        yield return new WaitForSeconds(0.02f);

        imageTransform.localScale = new Vector3(imageTransform.localScale.x - C * changeSizeUnit, imageTransform.localScale.y -  C *changeSizeUnit,0);
        yield return new WaitForSeconds(0.02f);

        imageTransform.localScale = new Vector3(imageTransform.localScale.x - C * changeSizeUnit, imageTransform.localScale.y -  C *changeSizeUnit,0);
        yield return new WaitForSeconds(0.02f);

        imageTransform.localScale = new Vector3(imageTransform.localScale.x - C * changeSizeUnit, imageTransform.localScale.y -  C *changeSizeUnit,0);
        yield return new WaitForSeconds(0.02f);

        imageTransform.localScale = new Vector3(imageTransform.localScale.x - C * changeSizeUnit, imageTransform.localScale.y -  C *changeSizeUnit,0);


        count.color = Color.white;
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
        int changeSizeUnit = 5;

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

    private void Reduce_Heart(int currentHeart, int maxHeart)
    {
        for(int i=0; i<maxHeart; i++)
        {
            heartImages[i].sprite = heartEmpty;
        }

        for(int i=0; i<currentHeart; i++)
        {
            heartImages[i].sprite = heartFill;
        }

        if(StageManager.isNowInitializing) return;

        StartCoroutine(heartReducing(currentHeart));
    }

    private void Heal_Heart(int currentHeart, int maxHeart)
    {
        for(int i=0; i<maxHeart; i++)
        {
            heartImages[i].sprite = heartEmpty;
        }

        for(int i=0; i<currentHeart-1; i++)
        {
            heartImages[i].sprite = heartFill;
        }

        if(StageManager.isNowInitializing) return;

        StartCoroutine(heartHealing(currentHeart-1));
    }

    IEnumerator heartReducing(int index)
    {
        float changeUnit = 0.1f;
        for(int i=0; i< 5; i++)
        {
            heartImages[index].gameObject.transform.localScale = new Vector3(1 + i * changeUnit,1 + i * changeUnit,1);
            yield return new WaitForSeconds(0.02f);
        }
        
        foreach(Sprite sprite in heartReducingAnimationSprite)
        {
            heartImages[index].sprite = sprite;
            yield return new WaitForSeconds(0.05f);
        }

        for(int i=0; i< 5; i++)
        {
            heartImages[index].gameObject.transform.localScale = new Vector3(1 + (4 - i)* changeUnit,1 + (4 - i) * changeUnit,1);
            yield return new WaitForSeconds(0.02f);
        }

        heartImages[index].gameObject.transform.localScale = Vector3.one;
    }

    IEnumerator heartHealing(int index)
    {
        float changeUnit = 0.1f;
        for(int i=0; i< 5; i++)
        {
            heartImages[index].gameObject.transform.localScale = new Vector3(1 + i * changeUnit,1 + i * changeUnit,1);
            yield return new WaitForSeconds(0.02f);
        }
        
        foreach(Sprite sprite in heartReducingAnimationSprite.Reverse().ToArray())
        {
            heartImages[index].sprite = sprite;
            yield return new WaitForSeconds(0.05f);
        }

        for(int i=0; i< 5; i++)
        {
            heartImages[index].gameObject.transform.localScale = new Vector3(1 + (4 - i)* changeUnit,1 + (4 - i) * changeUnit,1);
            yield return new WaitForSeconds(0.02f);
        }

        heartImages[index].gameObject.transform.localScale = Vector3.one;
    }

    private void Set_Width_Height(Vector2 vector2)
    {
        width.text = "Width : " + vector2.x.ToString();
        height.text = "Height : " + vector2.y.ToString();
    }

}
