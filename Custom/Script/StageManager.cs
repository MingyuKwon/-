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

public enum NumMode{
    Total = 0,
    Mine = 1,
    Treasure = 2,
}

public class StageManager : MonoBehaviour, IStageManager
{   
    [SerializeField] private TileGrid grid;

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

    int[,] totalNumArray = null;
    bool[,] totalNumMask = null;

    int[,] mineNumArray = null;
    int[,] treasureNumArray = null;


    public delegate bool ConditionDelegate(int x);
    List<ConditionDelegate> NumModeConditions = new List<ConditionDelegate>
        {
            (x) => x < 0 ,  // 토탈로 보면 0보다 작은 경우는 전부 센다
            (x) => x == -1,  // 지뢰인 경우를 찾는다
            (x) => x == -2  // 보물인 경우를 찾는다
        };
    private int[] aroundX = {-1,0,1 };
    private int[] aroundY = {-1,0,1 };

    [Button]
    public void StageInitialize(int width, int height, Difficulty difficulty)
    {
        totalNumArray = null;
        totalNumMask = null;

        mineNumArray = null;
        treasureNumArray = null;
        
        MakeMineTreasureArray(width, height, difficulty);

        UpdateArrayNum(NumMode.Total);

        grid.ShowEnvironment(width, height);
        grid.ShowTotalNum(totalNumArray, totalNumMask);
        grid.ShowMineTreasure(mineTreasureArray);
    }

    [Button]
    void UpdateArrayNum(NumMode numMode)
    {
        int height = mineTreasureArray.GetLength(0);
        int width = mineTreasureArray.GetLength(1);

        int[,] targetNumArray = null;
        switch(numMode)
        {
            case NumMode.Total :
                targetNumArray = totalNumArray;
                break;
            case NumMode.Mine :
                targetNumArray = mineNumArray;
                break;
            case NumMode.Treasure :
                targetNumArray = treasureNumArray;
                break;
        }

        if(targetNumArray == null)
        {
            switch(numMode)
            {
            case NumMode.Total :
                totalNumArray = new int[height, width];
                totalNumMask = new bool[height, width];
                targetNumArray = totalNumArray;
                break;
            case NumMode.Mine :
                mineNumArray = new int[height, width];
                targetNumArray = mineNumArray;
                break;
            case NumMode.Treasure :
                treasureNumArray = new int[height, width];
                targetNumArray = treasureNumArray;
                break;
            }

        }else
        {
            if(height != targetNumArray.GetLength(0) || 
                width != targetNumArray.GetLength(1))
            {
                Debug.LogError(" mineTreasureArray size and targetNumArray size dont match! \n height : " + height + " width : " + width + " \n targetNumArray.GetLength(0) : " + targetNumArray.GetLength(0) + " targetNumArray.GetLength(1) :" + targetNumArray.GetLength(1));
            }
        }

        for(int i=0; i<height; i++)
        {
            for(int j=0; j<width; j++)
            {
                if(NumModeConditions[(int)numMode](mineTreasureArray[i,j])) // 모드에 따라 어떻게 판단 해야 할지 다르다
                {
                    for(int aroundI =0; aroundI < aroundY.Length; aroundI++)
                    {
                        for(int aroundJ =0; aroundJ < aroundX.Length; aroundJ++)
                        {
                            if(aroundX[aroundJ] == 0 && aroundY[aroundI] == 0) continue;

                            int x = j+ aroundX[aroundJ];
                            int y = i+ aroundY[aroundI];

                            if(x > -1 && x < width 
                            && y > -1 && y < height
                            && mineTreasureArray[y,x] != -1) // 이거 지뢰인 경우를 제외하고는 다 계산을 해줘야 한다
                            {
                                targetNumArray[y,x]++;
                            }
                        }
                    }
                }
            }
        }

        String str = "";
        for(int i=0; i< height; i++)
        {
            for(int j=0; j< width; j++)
            {
                str += targetNumArray[i, j].ToString();
                str += "  ";
            }

            str += "\n";
        }

        Debug.Log(str);

    }


    [Button]
    public void MakeMineTreasureArray(int width = 10, int height = 10, Difficulty difficulty = Difficulty.Easy)
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
