using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveManager : MonoBehaviour
{
    public static PlayerSaveManager instance = null;
    private int Stagetype = -1;
    private int StageIndex = -1;

    private int MaxHeart = -1;  
    private int CurrentHeart = -1; 

    private int PotionCount = -1; 
    private int MagGlassCount = -1; 
    private int HolyWaterCount = -1;

    private int equippedItem1 = 13; // 13 : None
    private int equippedItem2 = 13; // 13 : None
    private int equippedItem3 = 13; // 13 : None
    private int equippedItem4 = 13; // 13 : None
    private int equippedItem5 = 13; // 13 : None

    private int difficulty = -1;

    private void Awake() {
        GetPlayerStageData();
    }

    public void ClearPlayerStageData()
    {
        // 전부 저장 안했을 때 초기값으로 저장을 하는 것이 결국 초기화 작업이나 같다
        SavePlayerStageData(new int[13] {
            -1,
            -1, 
            -1,
            -1, 
            -1,
            -1,
            -1,
            13,
            13,
            13,
            13,
            13,
            -1
        });
    }

    public void SavePlayerStageData(int[] paras)
    {
        Stagetype = paras[0];
        StageIndex = paras[1];
        MaxHeart = paras[2];  
        CurrentHeart = paras[3]; 
        PotionCount = paras[4]; 
        MagGlassCount = paras[5]; 
        HolyWaterCount = paras[6];
        equippedItem1 = paras[7]; 
        equippedItem2 = paras[8]; 
        equippedItem3 = paras[9]; 
        equippedItem4 = paras[10]; 
        equippedItem5 = paras[11];
        difficulty = paras[12];

        PlayerPrefs.SetInt("Stagetype", Stagetype);
        PlayerPrefs.SetInt("StageIndex", StageIndex);
        PlayerPrefs.SetInt("MaxHeart", MaxHeart);
        PlayerPrefs.SetInt("CurrentHeart", CurrentHeart);
        PlayerPrefs.SetInt("PotionCount", PotionCount);
        PlayerPrefs.SetInt("MagGlassCount", MagGlassCount);
        PlayerPrefs.SetInt("HolyWaterCount", HolyWaterCount);
        PlayerPrefs.SetInt("equippedItem1", equippedItem1);
        PlayerPrefs.SetInt("equippedItem2", equippedItem2);
        PlayerPrefs.SetInt("equippedItem3", equippedItem3);
        PlayerPrefs.SetInt("equippedItem4", equippedItem4);
        PlayerPrefs.SetInt("equippedItem5", equippedItem5);
        PlayerPrefs.SetInt("difficulty", difficulty);
        PlayerPrefs.Save();
    }

    public int[] GetPlayerStageData() // 지금 보이는 이 데이터들을 배열로 보내줌
    {
        if(instance == null) // 만약 아직 get을 하지 않았다면 끌어 와야 함
        {
            instance = this;
        }
        
        Stagetype = PlayerPrefs.GetInt("Stagetype", -1);
        // 만약 여기서 -1을 받는다면, 그건 저장된 값이 아예 없다는 이야기 이다
        if(Stagetype == -1)
        {
            return null;
        }

        StageIndex = PlayerPrefs.GetInt("StageIndex", -1);

        MaxHeart = PlayerPrefs.GetInt("MaxHeart", -1);
        CurrentHeart = PlayerPrefs.GetInt("CurrentHeart", -1);

        PotionCount = PlayerPrefs.GetInt("PotionCount", -1);
        MagGlassCount = PlayerPrefs.GetInt("MagGlassCount", -1);
        HolyWaterCount = PlayerPrefs.GetInt("HolyWaterCount", -1);

        equippedItem1 = PlayerPrefs.GetInt("equippedItem1", 13);
        equippedItem2 = PlayerPrefs.GetInt("equippedItem2", 13);
        equippedItem3 = PlayerPrefs.GetInt("equippedItem3", 13);
        equippedItem4 = PlayerPrefs.GetInt("equippedItem4", 13);
        equippedItem5 = PlayerPrefs.GetInt("equippedItem5", 13);

        difficulty = PlayerPrefs.GetInt("difficulty", -1);

        return new int[13] {
            Stagetype,
            StageIndex, 
            MaxHeart,
            CurrentHeart, 
            PotionCount,
            MagGlassCount,
            HolyWaterCount,
            equippedItem1,
            equippedItem2,
            equippedItem3,
            equippedItem4,
            equippedItem5,
            difficulty
        };
    }
}
