using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType {
    MineAppear = 0,
    MineDisappear = 1,
    TreasureAppear = 2,
    TreasureDisappear = 3,
}

public class EventManager : MonoBehaviour
{
    #region Event
    public Action<EventType, Vector3Int> SetAnimationTileEvent;

    #endregion

    public static EventManager instance = null;

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
        switch(eventType)
        {
            case EventType.MineAppear :
            case EventType.MineDisappear :
            case EventType.TreasureAppear :
            case EventType.TreasureDisappear :
                if(param1 is Vector3Int)
                {
                    SetAnimationTileEvent.Invoke(eventType, (Vector3Int)param1);
                }else
                {
                    Debug.LogError("param1 is not a Vector3Int Type!");
                }
                
                break;
        }
    }


    
}
