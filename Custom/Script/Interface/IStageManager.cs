using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define in IStageManager
/// </summary>
public enum Difficulty{
    Easy = 0,
    Normal = 1,
    Hard = 2,
    Professional = 3,
}

/// <summary>
/// Define in IStageManager
/// </summary>
public enum Total_Mine_Treasure{
    Total = 0,
    Mine = 1,
    Treasure = 2,
}

public interface IStageManager
{
    public void StageInitialize(int width, int height, Difficulty difficulty);
}
