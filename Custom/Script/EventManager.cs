using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType {
    MineAppear = 0,
    MineDisappear = 1,
    TreasureAppear = 2,
    TreasureDisappear = 3,
    Set_Width_Height = 4,
    Set_Heart = 5,
    Game_Over = 6,
    Game_Restart = 7,
    Item_Use = 8,
    Item_Obtain = 9,
}

public enum GameOver_Reason {
    None = 0,
    Heart0 = 1,
    TreasureCrash = 2,
    TimeOver = 3,
}

public enum Item {
    None = 0,
    Potion = 1,
    Mag_Glass = 2,
    Holy_Water = 3,
}

public enum ItemUseType {
    Shovel = 4,
    Potion = 3,
    Mag_Glass = 2,
    Holy_Water = 0,
    Crash = 1,
}

public class EventManager : MonoBehaviour
{
    #region Event

    public Action<EventType, Vector3Int> SetAnimationTileEvent;
    public Action<Vector2> Set_Width_Height_Event;

    public Action<EventType, int> mine_treasure_count_Change_Event;
    public Action<EventType> Set_UI_Filter_Event;

    public Action<EventType,Item, int> Item_Count_Change_Event;

    public Action<bool, GameOver_Reason> Game_Over_Event;

    public Action<int, int> Reduce_Heart_Event;
    public void Reduce_HeartInvokeEvent(int currentHeart, int maxHeart)
    {
        Reduce_Heart_Event.Invoke(currentHeart, maxHeart);
    }
    public Action<int, int> Heal_Heart_Event;
    public void Heal_HeartInvokeEvent(int currentHeart, int maxHeart)
    {
        Heal_Heart_Event.Invoke(currentHeart, maxHeart);
    }


    public Action<int, int> timerEvent;
    public void TimerInvokeEvent(int timeElapsed, int timeLeft)
    {
        timerEvent.Invoke(timeElapsed, timeLeft);
    }

    public Action<Vector3Int,bool, bool, bool, bool, bool> ItemPanelShow_Event;
    public void ItemPanelShow_Invoke_Event(Vector3Int position, bool isShow, bool isHolyEnable = false, bool isCrachEnable = false, bool isMagEnable = false, bool isPotionEnable = false)
    {
        ItemPanelShow_Event.Invoke(position, isShow, isHolyEnable , isCrachEnable , isMagEnable , isPotionEnable);
    }

    public Action<ItemUseType, Vector3Int> ItemUseEvent;
    public void ItemUse_Invoke_Event(ItemUseType itemUseType, Vector3Int itemUseDirection)
    {
        ItemUseEvent.Invoke(itemUseType, itemUseDirection);
    }

    #endregion




    public static EventManager instance = null;
    public static bool isAnimationPlaying{
        get{
            return _AnimationPlaying;
        }

        set{
            _AnimationPlaying = value;
        }
    }

    static bool _AnimationPlaying = false;

    private void Awake() {
        if(instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }else
        {
            instance = this;
        }

    }


    public void InvokeEvent(EventType eventType, System.Object param1 = null,System.Object param2 = null)
    {
        if(eventType == EventType.Game_Over)
        {
            if(param1 is GameOver_Reason)
            {
                Game_Over_Event.Invoke(true, (GameOver_Reason)param1);
            }
            
            return;
        }else if(eventType == EventType.Game_Restart)
        {
            Game_Over_Event.Invoke(false, GameOver_Reason.None);
            return;
        }

        if(param1 is int)
        {
            mine_treasure_count_Change_Event.Invoke(eventType, (int)param1);
            Set_UI_Filter_Event.Invoke(eventType);
        }

        if(param1 is Vector3Int)
        {
            SetAnimationTileEvent.Invoke(eventType, (Vector3Int)param1);
        }

        if(param1 is Vector2)
        {
            Set_Width_Height_Event.Invoke( (Vector2)param1);
        }
        

        if(param1 is Item)
        {
            Item_Count_Change_Event.Invoke(eventType, (Item)param1, (int)param2);
        }
    }


    
}
