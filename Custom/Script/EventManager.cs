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
}

public class EventManager : MonoBehaviour
{
    #region Event
    public Action<EventType, Vector3Int> SetAnimationTileEvent;
    public Action<Vector2> Set_Width_Height_Event;
    public Action<int, int> Set_Heart_Event;
    public Action<EventType, int> mine_treasure_count_Change_Event;
    public Action<EventType> Set_UI_Filter_Event;

    public Action<bool> Game_Over_Event;

    #endregion

    public static EventManager instance = null;
    public static bool isAnimationPlaying = false;

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
            Game_Over_Event.Invoke(true);
            return;
        }else if(eventType == EventType.Game_Restart)
        {
            Game_Over_Event.Invoke(false);
            return;
        }

        if(param1 is int)
        {
            if(param2 is int)
            {
                Set_Heart_Event.Invoke((int)param1, (int)param2);
            }else
            {
                mine_treasure_count_Change_Event.Invoke(eventType, (int)param1);
                Set_UI_Filter_Event.Invoke(eventType);
            }
            
        }

        if(param1 is Vector3Int)
        {
            SetAnimationTileEvent.Invoke(eventType, (Vector3Int)param1);
        }

        if(param1 is Vector2)
        {
            Set_Width_Height_Event.Invoke( (Vector2)param1);
        }
        
    }


    
}
