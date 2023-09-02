using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType {
    MineAppear = 0,
    MineDisappear = 1,
    TreasureAppear = 2,
    TreasureDisappear = 3,
    Set_Width_Height = 3,
}

public class EventManager : MonoBehaviour
{
    #region Event
    public Action<EventType, Vector3Int> SetAnimationTileEvent;
    public Action<Vector2> Set_Width_Height_Event;
    public Action<EventType, int> mine_treasure_count_Change_Event;

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

    public void InvokeEvent(EventType eventType, System.Object param1 = null)
    {
        if(param1 is int)
        {

            mine_treasure_count_Change_Event.Invoke(eventType, (int)param1);
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
