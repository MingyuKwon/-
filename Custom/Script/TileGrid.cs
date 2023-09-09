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
    [SerializeField] private TileBase MineTile;
    [SerializeField] private TileBase MineDisappearTile;
    [SerializeField] private TileBase TreasureTile;
    [SerializeField] private TileBase ObstacleTile;
    [SerializeField] private TileBase FocusTile;
    [SerializeField] private TileBase TreasureYesTile;
    [SerializeField] private TileBase TreasureNoTile;

    [SerializeField] private TileBase CrackTile;
    [SerializeField] private TileBase BombTile;

    [Header("Flag")]
    [SerializeField] private TileBase TrapFlag;
    [SerializeField] private TileBase TreasureFlag;
    [SerializeField] private TileBase QuestionFlag;
    

    [Space]
    [Header("Num")]
    [SerializeField] private TileBase[] totalNum;
    [SerializeField] private TileBase[] mineNum;
    [SerializeField] private TileBase[] treasureNum;

    [SerializeField] private Tilemap[] tilemaps; 
    //0 : Base , 1: Bound , 2 : Total Num, 3 : Bomb Num, 4 : Treasure Num,5 : Mine and Treasure ,6 : Obstacle ,7 : treasure search ,8 : Flag,9 : Crack, 10 : Focus

    public Tilemap obstacleTilemap{
        get{
            return tilemaps[6];
        }
    }

    public Tilemap boundTilemap{
        get{
            return tilemaps[1];
        }
    }

    private void OnEnable() {
        EventManager.instance.SetAnimationTileEvent += ReserveAnimation;
    }

    private void OnDisable() {
        EventManager.instance.SetAnimationTileEvent -= ReserveAnimation;
    }

    [Button]
    public void ShowEnvironment(int width = 10, int height = 10)
    {
        tilemaps[0].ClearAllTiles();
        tilemaps[1].ClearAllTiles();
        tilemaps[3].ClearAllTiles();
        tilemaps[4].ClearAllTiles();
        tilemaps[7].ClearAllTiles();
        tilemaps[8].ClearAllTiles();
        tilemaps[9].ClearAllTiles();

        int groundstartX;
        int groundendX;
        int groundstartY;
        int groundendY;

        CalcBoxStart(width, height, out groundstartX, out groundendX, out groundstartY, out groundendY);

        BoxFillCustom(tilemaps[0], BaseTile, groundstartX-1, groundstartY-1, groundendX+1, groundendY+1);

        int padding = 20;

        int borderstartX = groundstartX - padding;
        int borderendX = groundendX + padding;
        int borderstartY = groundstartY - padding;
        int borderendY = groundendY + padding;

        BoxFillCustom(tilemaps[1], BoundTile, borderstartX, borderstartY, groundstartX-1, borderendY);
        BoxFillCustom(tilemaps[1], BoundTile, groundendX + 1, borderstartY, borderendX, borderendY);
        BoxFillCustom(tilemaps[1], BoundTile, borderstartX, borderstartY, borderendX, groundstartY - 1);
        BoxFillCustom(tilemaps[1], BoundTile, borderstartX, groundendY + 1, borderendX, borderendY);

        BoxFillCustom(tilemaps[6], ObstacleTile, groundstartX, groundstartY, groundendX, groundendY);
    }


    [Button]
    public void ShowTotalNum(int[,] totalNumArray, bool[,] totalNumMask)
    {
        tilemaps[2].ClearAllTiles();

        int height = totalNumArray.GetLength(0);
        int width = totalNumArray.GetLength(1);
        
        int groundstartX;
        int groundendX;
        int groundstartY;
        int groundendY;

        CalcBoxStart(width, height, out groundstartX, out groundendX, out groundstartY, out groundendY);


        for(int i=0; i<height; i++)
        {
            for(int j=0; j<width; j++)
            {
                if(totalNumArray[i,j] > 0 && totalNumArray[i,j] < 9 && !totalNumMask[i,j])
                {   
                    tilemaps[2].SetTile(new Vector3Int(j + groundstartX,-i + groundendY,0) , totalNum[totalNumArray[i,j]]);
                }
            }
        }
    }

    [Button]
    public void ShowMineTreasure(int[,] mineTreasureArray)
    {
        tilemaps[5].ClearAllTiles();

        int height = mineTreasureArray.GetLength(0);
        int width = mineTreasureArray.GetLength(1);
        
        int groundstartX;
        int groundendX;
        int groundstartY;
        int groundendY;

        CalcBoxStart(width, height, out groundstartX, out groundendX, out groundstartY, out groundendY);


        for(int i=0; i<height; i++)
        {
            for(int j=0; j<width; j++)
            {
                if(mineTreasureArray[i,j] < 0)
                {   
                    tilemaps[5].SetTile(new Vector3Int(j + groundstartX,-i + groundendY,0) , (mineTreasureArray[i,j] == -1) ? MineTile : TreasureTile);
                }
            }
        }
    }

    [Button]
    public void UpdateSeperateNum(int[,] mineNumArray, int[,] treasureNumArray, Vector3Int position)
    {
        int height = mineNumArray.GetLength(0);
        int width = mineNumArray.GetLength(1);
        
        int groundstartX;
        int groundendX;
        int groundstartY;
        int groundendY;

        CalcBoxStart(width, height, out groundstartX, out groundendX, out groundstartY, out groundendY);

        int x = position.x - groundstartX;
        int y = -position.y + groundendY;

        tilemaps[2].SetTile(new Vector3Int(position.x,position.y,0) , null);
        tilemaps[3].SetTile(new Vector3Int(position.x,position.y,0) , mineNum[mineNumArray[y,x]]);
        tilemaps[4].SetTile(new Vector3Int(position.x,position.y,0) , treasureNum[treasureNumArray[y,x]]);

        // 만약 전체 필드를 다 보여주고 싶다면
        // for(int i=0; i<height; i++)
        // {
        //     for(int j=0; j<width; j++)
        //     {
        //         if(bombNumArray[i,j] >= 0 && bombNumArray[i,j] < 9)
        //         {   
        //             tilemaps[3].SetTile(new Vector3Int(j + groundstartX,-i + groundendY,0) , mineNum[bombNumArray[i,j]]);
        //         }

        //         if(treasureNumArray[i,j] >= 0 && treasureNumArray[i,j] < 9)
        //         {   
        //             tilemaps[4].SetTile(new Vector3Int(j + groundstartX,-i + groundendY,0) , treasureNum[treasureNumArray[i,j]]);
        //         }
        //     }
        // }
    }

    public void ShowSeperateNum(int[,] mineNumArray, int[,] treasureNumArray, bool[,] totalNumMask)
    {
        int height = mineNumArray.GetLength(0);
        int width = mineNumArray.GetLength(1);
        
        int groundstartX;
        int groundendX;
        int groundstartY;
        int groundendY;

        CalcBoxStart(width, height, out groundstartX, out groundendX, out groundstartY, out groundendY);

        for(int i=0; i<height; i++)
        {
            for(int j=0; j<width; j++)
            {
                if(totalNumMask[i,j]) // 여기가 true로 되어 있으면 아이템으로 토탈 지움 -> seperate 띄워저 있음
                {   
                    tilemaps[3].SetTile(new Vector3Int(j + groundstartX,-i + groundendY,0) , mineNum[mineNumArray[i,j]]);
                    tilemaps[4].SetTile(new Vector3Int(j + groundstartX,-i + groundendY,0) , treasureNum[treasureNumArray[i,j]]);
                }
            }
        }
        
    }

    public void RemoveObstacleTile(Vector3Int cellPos, bool isBomb = false)
    {
        if(!StageManager.isNowInitializing)
        {
            if(obstacleTilemap.HasTile(cellPos)) {
                StageManager.stageInputBlock++;
                StartCoroutine(crackAnimation(cellPos, isBomb));
            }
        }else
        {
            obstacleTilemap.SetTile(cellPos, null);
            tilemaps[9].SetTile(cellPos, null);
        }
        
    }

    IEnumerator crackAnimation(Vector3Int cellPos, bool isBomb = false)
    {
        if(isBomb)
        {
            tilemaps[9].SetTile(cellPos, BombTile);
        }else
        {
            tilemaps[9].SetTile(cellPos, CrackTile);
        }

        yield return new WaitForSeconds(0.2f);


        obstacleTilemap.SetTile(cellPos, null);
        tilemaps[9].SetTile(cellPos, null);
        StageManager.stageInputBlock--;
    }

    public void ReserveAnimation(EventType tileType, Vector3Int cellPos )
    {
        StartCoroutine(SetAnimationTile(cellPos, tileType));
    }

    IEnumerator SetAnimationTile(Vector3Int cellPos, EventType tileType)
    {
        while(StageManager.isStageInputBlocked){
            yield return new WaitForEndOfFrame();
        }
        StageManager.stageInputBlock++;

        yield return null;

        switch(tileType)
        {
            case EventType.MineDisappear :
                tilemaps[5].SetTile(cellPos , MineDisappearTile);
                break;
            case EventType.TreasureAppear :
                tilemaps[5].SetTile(cellPos , TreasureTile);
                break;
        }

        yield return new WaitForSeconds(0.5f);
        tilemaps[5].SetTile(cellPos , null);

        StageManager.stageInputBlock--;
    }

    public void SetFlag(Vector3Int position , Flag flag)
    {
        switch(flag)
        {
            case Flag.None :
                tilemaps[8].SetTile(position, null);
                break;
            case Flag.Question :
                tilemaps[8].SetTile(position, QuestionFlag);
                break;
            case Flag.Mine :
                tilemaps[8].SetTile(position, TrapFlag);
                break;
            case Flag.Treasure :
                tilemaps[8].SetTile(position, TreasureFlag);
                break;
            
        }
    }

    public void SetTreasureSearch(Vector3Int position , TreasureSearch flag)
    {
        switch(flag)
        {
            case TreasureSearch.None :
                tilemaps[7].SetTile(position, null);
                break;
            case TreasureSearch.Yes :
                tilemaps[7].SetTile(position, TreasureYesTile);
                break;
            case TreasureSearch.No :
                tilemaps[7].SetTile(position, TreasureNoTile);
                break;
        }
    }

    public void SetFocus(Vector3Int previousPosition , Vector3Int newPosition)
    {
        tilemaps[10].SetTile(previousPosition, null);
        tilemaps[10].SetTile(newPosition, FocusTile);
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
