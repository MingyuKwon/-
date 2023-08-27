using UnityEngine;

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
    public void UpdateSeperateNum(int[,] bombNumArray, int[,] treasureNumArray, Vector2Int position);
}
