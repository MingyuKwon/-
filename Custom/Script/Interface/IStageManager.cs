using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStageManager
{
    public void StageInitialize(int width, int height, Difficulty difficulty);
}
