using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInformationManager
{
    public static float easyMineRatio = 0.15f;
    public static float normalMineRatio = 0.18f;
    public static float hardMineRatio = 0.23f;
    public static float professionalMineRatio = 0.28f;
    public static float mineToTreasureRatio = 0.4f;

    public static float[] noItemRatio = {0.1f,0.2f,0.3f};

    public static int[] tutorialWidth = {5,5,7,9};
    public static int[] tutorialHeight = {5,5,7,9};

    public static int[,] tutorial1Stageinform = {
                        {0,0,0,-1,-2},
                        {0,1,1,1,0},
                        {0,1,1,1,0},
                        {-1,1,1,1,0},
                        {-2,0,0,-2,0}, 
                    };

    public static int[,] tutorial2Stageinform = {
                        {-2,-1,0,-1,-1},
                        {-2,1,1,1,-1},
                        {-1,1,1,1,-1},
                        {-1,1,1,1,-1},
                        {-2,0,0,-2,-2}, 
                    };
    public static int[,] tutorial3Stageinform = {
                        {-1,-2,-1,0,0,0,-1},
                        {-2,0,0,0,-2,-2,0},
                        {-2,-1,1,1,1,-2,0},
                        {0,0,1,1,1,0,0},
                        {-1,0,1,1,1,0,-1},
                        {-1,0,0,0,-2,0,0},
                        {-1,-2,0,0,0,-2,0},
                    };

    public static int[] tutorialmineCount = {2,8,8};
    public static int[] tutorialtreasureCount = {3,5,9};

    public static int[,] stageWidthMin = 
    {
        {8 , 9, 10, 11, 12,13},
        {15, 16, 17, 18, 19,20},
        {28 , 29, 30, 31, 32,33},
    };
    public static int[,] stageWidthMax = 
    {
        {10 , 11, 12, 13, 14,15},
        {17 , 18, 19, 20, 21,22},
        {30 , 31, 32, 33, 34,35},
    };
    
    public static int[,] stageHeightMin = {
        {8 , 8, 8, 8, 8,8},
        {14 , 14, 15, 15, 16,16},
        {16 , 16, 16, 16, 17,17},
    };
    public static int[,] stageHeightMax = {
        {9 , 9, 9, 10, 10,10},
        {16 , 16, 16, 17, 17,18},
        {18 , 19, 20, 21, 22,23},
    };

    public static int DefaultTimeforEntireGame = 100;
    public static int[] DefaultTimeperStage = {150, 200, 250};
    public static int[] DefaultTrapDamage = {1, 1, 2};

    public static int plusPotion_byItem_perStage = 3;
    public static int plusMag_byItem_perStage = 3;
    public static int plusHoly_byItem_perStage = 3;
    public static int plusTime_byItem_perStage = 30;

    public static int plusPotion_Default_perStage = 3;
    public static int plusMag_Default_perStage = 5;
    public static int plusHoly_Default_perStage = 3;

    public static GameModeType gameMode = GameModeType.None;
    public static int currentStagetype = 0;
    public static int currentStageIndex = 0;
    public static bool isnextStageDungeon = true;
    public static Difficulty difficulty = Difficulty.Easy;
    public static Vector3Int treasurePosition = new Vector3Int(-4,-1,0);

    public static int NextWidth = -1;  
    public static int NextHeight = -1;  

    public static int NextmaxHeart = -1;  
    public static int NextcurrentHeart = -1; 
    public static int NextpotionCount = -1; 
    public static int NextmagGlassCount = -1; 
    public static int NextholyWaterCount = -1;
    public static int NexttotalTime = -1;
}
