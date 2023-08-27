using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Sirenix.OdinInspector;

/// <summary>
/// Define in StageManager
/// </summary>
public enum Difficulty{
    Easy = 0,
    Normal = 1,
    Hard = 2,
    Professional = 3,
}

public class StageManager : MonoBehaviour, IStageManager
{   
    [SerializeField] private IGridInterface grid;

    private float easyMineRatio = 0.15f;
    private float normalMineRatio = 0.2f;
    private float hardMineRatio = 0.3f;
    private float professionalMineRatio = 0.35f;
    private float mineToTreasureRatio = 0.3f;

    public int mineCount{
        get{
            return _mineCount;
        }
        set{
            _mineCount = value;
        }
    }
    public int treasureCount{
        get{
            return _treasureCount;
        }
        set{
            _treasureCount = value;
        }
    }

    private int _mineCount = 0;
    private int _treasureCount = 0;

    int[,] mineTreasureArray; // -2 : treausre, -1 : mine, 1 : Start Safe Area

    int[,] totalNumArray;
    bool[,] totalNumMask;

    int[,] mineNumArray;
    int[,] treasureNumArray;

    public void StageInitialize(int width, int height, Difficulty difficulty)
    {
        MakeMineTreasureArray(width, height, difficulty);
        grid.ShowEnvironment(width, height);
        grid.ShowTotalNum(totalNumArray, totalNumMask);
    }

    [Button]
    void MakeMineTreasureArray(int width = 10, int height = 10, Difficulty difficulty = Difficulty.Easy)
    {
        mineTreasureArray = new int[height, width];
        int totalBockNum = height * width;
        
        float mineRatio = 0;
        switch(difficulty)
        {
            case Difficulty.Easy :
                mineRatio = easyMineRatio;
                break;
            case Difficulty.Normal :
                mineRatio = normalMineRatio;
                break;
            case Difficulty.Hard :
                mineRatio = hardMineRatio;
                break;
            case Difficulty.Professional :
                mineRatio = professionalMineRatio;
                break;
        }

        int totalCount = (int)(totalBockNum * mineRatio);
        mineCount = (int)(totalCount * (1 - mineToTreasureRatio));
        treasureCount = totalCount - mineCount;

        int startX;
        int startY;

        CalcStartArea(width, height, out startX, out startY);

        // 처음 시작하는 곳 0,0 근처 8칸은 폭탄이 없음을 보장한다
        mineTreasureArray[startY-1, startX-1] = 1;
        mineTreasureArray[startY-1, startX] = 1;
        mineTreasureArray[startY-1, startX+1] = 1;
        mineTreasureArray[startY, startX -1] = 1;
        mineTreasureArray[startY, startX] = 1;
        mineTreasureArray[startY, startX +1] = 1;
        mineTreasureArray[startY+1, startX-1] = 1;
        mineTreasureArray[startY+1, startX] = 1;
        mineTreasureArray[startY+1, startX+1] = 1;
        // 처음 시작하는 곳 0,0 근처 8칸은 폭탄이 없음을 보장한다

        System.Random rng = new System.Random();

        List<int> randomNumbers = Enumerable.Range(0, totalBockNum-1)
                                     .OrderBy(_ => rng.Next())
                                     .ToList();
                                     
        List<int> selectedRandomNumbers = new List<int>();

        for(int i=0; selectedRandomNumbers.Count < totalCount && i< randomNumbers.Count; i++)
        {
            int num = randomNumbers[i];

            int row = num / width;
            int column = num % width;

            if(mineTreasureArray[row, column] > 0) // 만약 지뢰 안전 구역이라면 패스
            {
                continue;
            }

            selectedRandomNumbers.Add(num);
        }

        if(selectedRandomNumbers.Count != totalCount) Debug.LogError("sleectedRandomNumbers.Count != mineCount");

        int treasureTemp = treasureCount;
        foreach(int num in selectedRandomNumbers)
        {
            int row = num / width;
            int column = num % width;

            if(treasureTemp < 1)
            {
                mineTreasureArray[row, column] = -1; // 함정
            }else
            {
                mineTreasureArray[row, column] = -2; // 보물
                treasureTemp--;
            }

            
        }


        String str = "";
        for(int i=0; i< height; i++)
        {
            for(int j=0; j< width; j++)
            {
                str += mineTreasureArray[i, j].ToString();
                str += "  ";
            }

            str += "\n";
        }

        str += "\nmineCount : " + mineCount;
        str += "\ntreasureCount : " + treasureCount;


        Debug.Log(str);

    }

    void CalcStartArea(int width, int height, out int groundstartX,out int groundendY)
    {
        groundstartX = (width/2);

        if(height % 2 == 0)
        {
            groundendY = (height/2 -1);
        }else
        {
            groundendY = (height/2);
        }
    }
}
