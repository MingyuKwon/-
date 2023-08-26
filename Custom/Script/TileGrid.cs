using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;

public class TileGrid : MonoBehaviour, IGridInterface
{
    [Header("Environment")]
    [SerializeField] private TileBase BaseTile;
    [SerializeField] private TileBase BoundTile;
    [SerializeField] private TileBase ObstacleTile;

    [Space]
    [Header("Num")]
    [SerializeField] private TileBase[] totalNum;
    [SerializeField] private TileBase[] mineNum;
    [SerializeField] private TileBase[] treasureNum;

    [SerializeField] private Tilemap[] tilemaps; 
    //0 : Base , 1: Bound , 2 : Total Num, 3 : Bomb Num, 4 : Treasure Num,5 : Obstacle 

    [Button]
    public void ShowEnvironment(int width = 10, int height = 10)
    {
        tilemaps[0].ClearAllTiles();
        tilemaps[1].ClearAllTiles();

        int groundstartX;
        int groundendX;
        int groundstartY;
        int groundendY;

        CalcBoxStart(width, height, out groundstartX, out groundendX, out groundstartY, out groundendY);

        BoxFillCustom(tilemaps[0], BaseTile, groundstartX-1, groundstartY-1, groundendX+1, groundendY+1);

        int borderstartX = groundstartX - 5;
        int borderendX = groundendX + 5;
        int borderstartY = groundstartY - 5;
        int borderendY = groundendY + 5;

        BoxFillCustom(tilemaps[1], BoundTile, borderstartX, borderstartY, groundstartX-1, borderendY);
        BoxFillCustom(tilemaps[1], BoundTile, groundendX + 1, borderstartY, borderendX, borderendY);
        BoxFillCustom(tilemaps[1], BoundTile, borderstartX, borderstartY, borderendX, groundstartY - 1);
        BoxFillCustom(tilemaps[1], BoundTile, borderstartX, groundendY + 1, borderendX, borderendY);

        //BoxFillCustom(tilemaps[5], ObstacleTile, groundstartX, groundstartY, groundendX, groundendY);
    }

    [Button]
    public void ShowTotalNum(int[,] totalNumArray)
    {
        tilemaps[2].ClearAllTiles();

        totalNumArray =  new int[3, 4]
        {
            { 1, 2, 0, 4 },
            { 5, 0, 7, 8 },
            { 0, 1, 3, 0 }
        };
        int height = totalNumArray.GetLength(0);
        int width = totalNumArray.GetLength(1);
        
        int groundstartX;
        int groundendX;
        int groundstartY;
        int groundendY;

        CalcBoxStart(width, height, out groundstartX, out groundendX, out groundstartY, out groundendY);


        Debug.Log(groundstartX + " " + groundendY);
        for(int i=0; i<height; i++)
        {
            for(int j=0; j<width; j++)
            {
                
                if(totalNumArray[i,j] > 0 && totalNumArray[i,j] < 9)
                {   
                    tilemaps[2].SetTile(new Vector3Int(j + groundstartX,-i + groundendY,0) , totalNum[totalNumArray[i,j]]);
                }
            }
        }
    }

    [Button]
    public void ShowSeperateNum(int[,] bombNumArray, int[,] treasureNumArray)
    {
        int width = bombNumArray.GetLength(0);
        int height = bombNumArray.GetLength(1);
        
        int groundstartX;
        int groundendX;
        int groundstartY;
        int groundendY;

        CalcBoxStart(width, height, out groundstartX, out groundendX, out groundstartY, out groundendY);

        for(int i=0; i<=height; i++)
        {
            for(int j=0; j<=width; j++)
            {
                if(bombNumArray[i,j] >= 0 && bombNumArray[i,j] < 9)
                {   
                    tilemaps[3].SetTile(new Vector3Int(i + groundstartX,j + groundendY,0) , totalNum[bombNumArray[i,j]]);
                }

                if(treasureNumArray[i,j] >= 0 && treasureNumArray[i,j] < 9)
                {   
                    tilemaps[4].SetTile(new Vector3Int(i + groundstartX,j + groundendY,0) , totalNum[treasureNumArray[i,j]]);
                }
            }
        }
    }

    private void BoxFillCustom(Tilemap tilemap, TileBase tile, int startX, int startY, int endX, int endY)
    {
        for(int i=startX; i <= endX; i++)
        {
            for(int j = startY; j <= endY; j++)
            {
                tilemap.SetTile(new Vector3Int(i,j,0), tile);
            }
        }
        
    }

    private void CalcBoxStart(int width, int height, out int groundstartX, out int groundendX,out int groundstartY,out int groundendY)
    {
        if(width % 2 == 0)
        {
            groundstartX = -(width/2);
            groundendX = (width/2 - 1);
        }else
        {
            groundstartX = -(width/2);
            groundendX = (width/2);
        }

        if(height % 2 == 0)
        {
            groundstartY = -(height/2);
            groundendY = (height/2 -1);
        }else
        {
            groundstartY = -(height/2);
            groundendY = (height/2);
        }
    }



}
