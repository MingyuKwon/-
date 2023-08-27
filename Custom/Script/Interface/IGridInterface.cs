using UnityEngine;

/// <summary>
/// Define in IGridInterface
/// </summary>
public enum Flag{
    None = 0,
    Question = 1,
    Mine = 2,
    Treasure = 3,
    
}

public interface IGridInterface
{
    /// <summary>
    /// 주변 환경을 그려주는 함수
    /// </summary>
    public void ShowEnvironment(int width, int height);

    /// <summary>
    /// 숫자를 그려주는 함수
    /// </summary>
    public void ShowTotalNum(int[,] totalNumArray, bool[,] totalNumMask);

    /// <summary>
    /// 전체에 지뢰하고 보물을 보여주는 함수
    /// </summary>
    public void ShowMineTreasure(int[,] mineTreasureArray);

    /// <summary>
    /// 지정된 위치에 total을 seperate로 바꿔주는 함수
    /// </summary>
    public void UpdateSeperateNum(int[,] bombNumArray, int[,] treasureNumArray, Vector3Int position);

    /// <summary>
    /// 지정된 위치에 flag를 세우는 함수
    /// </summary>
    public void SetFlag(Vector3Int position, Flag flag);

    
}
