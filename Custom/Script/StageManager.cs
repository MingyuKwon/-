using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class StageManager : MonoBehaviour, IStageManager
{   
    const int DefaultX = 30;
    const int DefaultY = 18;

    static public bool isNowInitializing = false;

    /// <summary>
    /// 스테이지에 입력을 받을지 말지 정한다. 이게 0이면 스테이지 인풋을 받고, 아니면 차단
    /// </summary>
    static public int stageInputBlock{
        get{
            return _stageInputBlock;
        }

        set{
            _stageInputBlock =  value;
            if(_stageInputBlock < 0) _stageInputBlock = 0;
        }
    }

    static public bool isStageInputBlocked{
        get{
            return (stageInputBlock > 0 || EventManager.isAnimationPlaying);
        }

    }

    static private int _stageInputBlock = 0; 

    [SerializeField] private TileGrid grid;

    [Space]
    [Header("For Debug")]
    [SerializeField] private TextMeshProUGUI tmp;

    private float easyMineRatio = 0.15f;
    private float normalMineRatio = 0.18f;
    private float hardMineRatio = 0.23f;
    private float professionalMineRatio = 0.28f;
    private float mineToTreasureRatio = 0.4f;

    int startX = -1;
    int startY = -1;

    int width = -1;
    int height = -1;

    public int maxHeart{
        get{
            return _maxHeart;
        }

        set{
            _maxHeart = value;
        }
    }

    public int currentHeart{
        get{
            return _currentHeart;
        }

        set{
            _currentHeart = value;
        }
    }

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

    private int _maxHeart = 0;
    private int _currentHeart = 0;
    private int _mineCount = 0;
    private int _treasureCount = 0;

    int[,] mineTreasureArray; // -2 : treausre, -1 : mine, 1 : Start Safe Area

    int[,] totalNumArray = null;
    bool[,] totalNumMask = null;

    bool[,] treasureSearchMask = null;

    int[,] mineNumArray = null;
    int[,] treasureNumArray = null;

    int[,] flagArray = null;

    bool[,] isObstacleRemoved = null;


    #region ITEM_Field
    int potionCount = 0;
    int magGlassCount = 0;
    int holyWaterCount = 0;

    bool potionEnable{
        get{
            return potionCount > 0;
        }
    }

    bool magGlassEnable{
        get{
            return magGlassCount > 0;
        }
    }

    bool holyWaterEnable{
        get{
            return holyWaterCount > 0;
        }
    }
    #endregion

    private Vector3Int currentFocusPosition = Vector3Int.one;

    private int totalTime = 0;
    private int timeElapsed = 0;
    private int timeLeft {
        get{
            return totalTime - timeElapsed;
        }
    }
    private Coroutine timerCoroutine = null;

    public delegate bool ConditionDelegate(int x);
    List<ConditionDelegate> NumModeConditions = new List<ConditionDelegate>
        {
            (x) => x < 0 ,  // 토탈로 보면 0보다 작은 경우는 전부 센다
            (x) => x == -1,  // 지뢰인 경우를 찾는다
            (x) => x == -2  // 보물인 경우를 찾는다
        };
    private int[] aroundX = {-1,0,1 };
    private int[] aroundY = {-1,0,1 };

    private void Start() {
        StageInitialize();
    }

    private void Update() {
        
        if(isStageInputBlocked) 
        {
            tmp.text = "NO";
            tmp.color = Color.red;
            return;
        }
        
        tmp.text = "OK";
        tmp.color = Color.green;

        if(EventSystem.current.IsPointerOverGameObject()) return;

        SetFocus();
        SetPlayer_Overlay();
        SetInteract_Ok();

        Vector3Int gap = PlayerManager.instance.checkPlayerNearFourDirection(currentFocusPosition);
        bool isNearFlag = (gap.magnitude == 1 && gap != Vector3Int.forward) ? true : false;

        if(Input.GetMouseButtonDown(0))
        {
            if(CheckHasObstacle(currentFocusPosition))
            {
                if(!isNearFlag) return;
                RemoveObstacle(currentFocusPosition);
            }else
            {
                InputManager.InputEvent.Invoke_Move(currentFocusPosition);
            }
            
        }
        
        if(Input.GetMouseButtonDown(1))
        {
            SetFlag(currentFocusPosition);
        }else if(Input.GetMouseButtonDown(2) && magGlassEnable)
        {
            if(gap != Vector3Int.zero) return;
            ChangeTotalToSeperate(currentFocusPosition);
        }else if(Input.GetMouseButton(3)) { 
            if(!isNearFlag) return;
            BombObstacle(currentFocusPosition);
        }else if(Input.GetMouseButton(4) && holyWaterEnable) { 
            if(!isNearFlag) return;
            SetTreasureSearch(currentFocusPosition);
        }
    }

    private void OnEnable() {
        EventManager.instance.Game_Over_Event += GameOver;
    }

    private void OnDisable() {
        EventManager.instance.Game_Over_Event -= GameOver;
    }

    private Vector3Int[] InteractPosition1 = new Vector3Int[4]{Vector3Int.forward, Vector3Int.forward,Vector3Int.forward,Vector3Int.forward};
    private Vector3Int[] InteractPosition2 = new Vector3Int[4]{Vector3Int.forward, Vector3Int.forward,Vector3Int.forward,Vector3Int.forward};
    private bool is1Next = true;
    private Vector3Int[] iterateMap = new Vector3Int[4]{Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left};
    
    private void SetInteract_Ok()
    {
       Vector3Int playerPosition = PlayerManager.instance.PlayerCellPosition;

       if(is1Next)
       {
            for(int i=0; i<4; i++)
            {
                if(grid.obstacleTilemap.HasTile(playerPosition + iterateMap[i]))
                {
                    InteractPosition1[i] = playerPosition + iterateMap[i];
                }else
                {
                    InteractPosition1[i] = Vector3Int.forward;
                }
            }

            grid.SetInteract_Ok(InteractPosition2,InteractPosition1);
       }else
       {
            for(int i=0; i<4; i++)
            {
                if(grid.obstacleTilemap.HasTile(playerPosition + iterateMap[i]))
                {
                    InteractPosition2[i] = playerPosition + iterateMap[i];
                }else
                {
                    InteractPosition2[i] = Vector3Int.forward;
                }
            }

            grid.SetInteract_Ok(InteractPosition1,InteractPosition2);
       }
       

       is1Next = !is1Next;

    }

    private Vector3Int currentPlayerPosition = Vector3Int.zero;
    private void SetPlayer_Overlay(bool isForce = false)
    {
        Vector3Int playerPosition = PlayerManager.instance.PlayerCellPosition;
        Vector3Int arrayPos = ChangeCellPosToArrayPos(playerPosition);

        //if(totalNumArray[arrayPos.y, arrayPos.x] == 0) grid.ShowOverlayNum(playerPosition,false, true);
        if(currentPlayerPosition == playerPosition && !isForce) return; // 만약 플레이어 위치가 변하지 않았다면 그냥 아무것도 안함

        grid.ShowOverlayNum(playerPosition,false, true);
        grid.ShowOverlayNum(playerPosition,false, false);

        currentPlayerPosition = playerPosition;

        if(totalNumMask[arrayPos.y, arrayPos.x]) // 만약 돋보기를 쓴 경우
        {
            grid.ShowOverlayNum(playerPosition,true,false,mineNumArray[arrayPos.y, arrayPos.x], treasureNumArray[arrayPos.y, arrayPos.x]);
            return;
        }

        if(totalNumArray[arrayPos.y, arrayPos.x] != 0) // 사용자 위치에 숫자가 떠야 하는 경우
        {
            grid.ShowOverlayNum(playerPosition,true,true,totalNumArray[arrayPos.y, arrayPos.x]);

        }
        

    }

    private void SetFocus()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = TileGrid.CheckCellPosition(worldPos);

        if(cellPos == currentFocusPosition) return; // 만약 포커스가 아직 바뀌지 않았다면 요청 무시
        if(grid.boundTilemap.HasTile(cellPos))  return; // 해당 위치가 필드 바깥이면 무시

        if(CheckHasObstacle(cellPos))  // 해당 위치에 타일이 있는지 확인
        { // 만약 타일이 있다면 상호작용이 가능한 놈만 포커스를 줘야 한다. 
            // 그니까, 해당 타일의 상하좌우 4공간 상에 비어있는 곳이 하나라도 있다면 가능, 아니면 불가능
            if( (grid.boundTilemap.HasTile(cellPos +  Vector3Int.up) || CheckHasObstacle(cellPos +  Vector3Int.up)) &&
                (grid.boundTilemap.HasTile(cellPos +  Vector3Int.down) || CheckHasObstacle(cellPos +  Vector3Int.down)) &&
                (grid.boundTilemap.HasTile(cellPos +  Vector3Int.right) || CheckHasObstacle(cellPos +  Vector3Int.right)) &&
                (grid.boundTilemap.HasTile(cellPos +  Vector3Int.left) || CheckHasObstacle(cellPos +  Vector3Int.left)) 
            ) return;
            
        }
         
        grid.SetFocus(currentFocusPosition, cellPos);
        currentFocusPosition = cellPos;
    }

    private void RemoveObstacle(Vector3Int cellPos, bool special = false) // Special은 보물을 찾거나 지뢰를 없애서 갱신되고 처음 도는 재귀를 의미. 
                                                                            //이 경우에는 타일이 이미 지워져 있어도 다시 돌아야 한다
    {
        Vector3Int arrayPos = ChangeCellPosToArrayPos(cellPos);
        if (special || CheckHasObstacle(cellPos))  // 해당 위치에 타일이 있는지 확인
        {
            SetFlag(cellPos, true);
            SetTreasureSearch(cellPos, true);

            RemoveObstacleTile(cellPos); 

            if(mineTreasureArray[arrayPos.y, arrayPos.x] == -1) // 지뢰
            {
                EventManager.instance.InvokeEvent(EventType.MineAppear, mineCount);
                HeartChange(-1); 
                return;
            }else{ // 지뢰가 아닌 타일 
                
                if(mineTreasureArray[arrayPos.y, arrayPos.x] == -2) //보물인 경우에는 추가 작업 해줘야 함
                {
                    mineTreasureArray[arrayPos.y, arrayPos.x] = 0; // 배열에서 보물을 지운다
                    treasureCount--;
                    UpdateArrayNum(Total_Mine_Treasure.Total); // 갱신
                    UpdateArrayNum(Total_Mine_Treasure.Treasure); // 갱신
                    GetItem(true);
                    EventManager.instance.InvokeEvent(EventType.TreasureAppear, cellPos);
                    EventManager.instance.InvokeEvent(EventType.TreasureAppear, treasureCount);
                    grid.ShowTotalNum(totalNumArray, totalNumMask);
                    SetPlayer_Overlay(true);
                    grid.ShowSeperateNum(mineNumArray, treasureNumArray, totalNumMask);

                    for(int aroundI =0; aroundI < aroundY.Length; aroundI++)
                        {
                            for(int aroundJ =0; aroundJ < aroundX.Length; aroundJ++)
                            {
                                if(aroundX[aroundJ] == 0 && aroundY[aroundI] == 0) continue;

                                int x = arrayPos.x + aroundX[aroundJ];
                                int y = arrayPos.y + aroundY[aroundI];

                                if(x > -1 && x < width 
                                && y > -1 && y < height
                                && (totalNumArray[y,x] == 0)
                                && (mineTreasureArray[y,x] >= 0)
                                ) 
                                {
                                    BombObstacle(new Vector3Int(x - startX, startY - y), true);
                                }
                            }
                        }
                }
                
                if(totalNumArray[arrayPos.y, arrayPos.x] == 0){ // 완전 빈 공간인 경우 사방 8개를 자동으로 다 연다
                    for(int aroundI =0; aroundI < aroundY.Length; aroundI++)
                        {
                            for(int aroundJ =0; aroundJ < aroundX.Length; aroundJ++)
                            {
                                if(aroundX[aroundJ] == 0 && aroundY[aroundI] == 0) continue;

                                int x = arrayPos.x + aroundX[aroundJ];
                                int y = arrayPos.y + aroundY[aroundI];

                                if(x > -1 && x < width 
                                && y > -1 && y < height) 
                                {
                                    RemoveObstacle(new Vector3Int(x - startX, startY - y));
                                }
                            }
                        }
                }
            }
            
        }
    }

    private void BombObstacle(Vector3Int cellPos, bool special = false) // Special은 보물을 찾거나 지뢰를 없애서 갱신되고 처음 도는 재귀를 의미. 
                                                                        //이 경우에는 타일이 이미 지워져 있어도 다시 돌아야 한다
    {
        Vector3Int arrayPos = ChangeCellPosToArrayPos(cellPos);
        if (special || CheckHasObstacle(cellPos))  // 해당 위치에 타일이 있는지 확인
        {
            SetFlag(cellPos, true);
            SetTreasureSearch(cellPos, true);

            RemoveObstacleTile(cellPos, true);

            if(mineTreasureArray[arrayPos.y, arrayPos.x] == -2) // 보물
            {
                EventManager.instance.InvokeEvent(EventType.TreasureDisappear, treasureCount);
                EventManager.instance.InvokeEvent(EventType.Game_Over, GameOver_Reason.TreasureCrash);
                return;
            }else{ // 보물이 아님
                
                if(mineTreasureArray[arrayPos.y, arrayPos.x] == -1) // 지뢰
                {
                    mineTreasureArray[arrayPos.y, arrayPos.x] = 0; // 배열에서 지뢰를 지운다
                    mineCount--;
                    UpdateArrayNum(Total_Mine_Treasure.Total); // 갱신
                    UpdateArrayNum(Total_Mine_Treasure.Mine); // 갱신
                    EventManager.instance.InvokeEvent(EventType.MineDisappear, cellPos);
                    EventManager.instance.InvokeEvent(EventType.MineDisappear, mineCount);
                    grid.ShowTotalNum(totalNumArray, totalNumMask);
                    SetPlayer_Overlay(true);
                    grid.ShowSeperateNum(mineNumArray, treasureNumArray, totalNumMask);

                    // 새로 갱신 후에는 , 갱신으로 인해 자기 주변에서 새로 0이 된 것이 없나 따로 확인 절차가 필요하다
                    for(int aroundI =0; aroundI < aroundY.Length; aroundI++)
                        {
                            for(int aroundJ =0; aroundJ < aroundX.Length; aroundJ++)
                            {
                                if(aroundX[aroundJ] == 0 && aroundY[aroundI] == 0) continue;

                                int x = arrayPos.x + aroundX[aroundJ];
                                int y = arrayPos.y + aroundY[aroundI];

                                if(x > -1 && x < width 
                                && y > -1 && y < height
                                && (totalNumArray[y,x] == 0)
                                && (mineTreasureArray[y,x] >= 0)
                                ) 
                                {
                                    BombObstacle(new Vector3Int(x - startX, startY - y), true);
                                }
                            }
                        }

                }

                if(totalNumArray[arrayPos.y, arrayPos.x] == 0){ // 완전 빈 공간인 경우 사방 8개를 자동으로 다 연다
                    for(int aroundI =0; aroundI < aroundY.Length; aroundI++)
                        {
                            for(int aroundJ =0; aroundJ < aroundX.Length; aroundJ++)
                            {
                                if(aroundX[aroundJ] == 0 && aroundY[aroundI] == 0) continue;

                                int x = arrayPos.x + aroundX[aroundJ];
                                int y = arrayPos.y + aroundY[aroundI];

                                if(x > -1 && x < width 
                                && y > -1 && y < height) 
                                {
                                    BombObstacle(new Vector3Int(x - startX, startY - y));
                                }
                            }
                        }
                }
            }
            
        }
    }
    private Vector3Int ChangeCellPosToArrayPos(Vector3Int cellPos)
    {   
        return new Vector3Int(cellPos.x + startX , startY - cellPos.y, cellPos.z);
    }
    private bool CheckHasObstacle(Vector3Int cellPos)
    {
        Vector3Int arrayPos = ChangeCellPosToArrayPos(cellPos);
        if(arrayPos.x <0 || arrayPos.y < 0 || arrayPos.x >= width || arrayPos.y >= height) return false;

        return !isObstacleRemoved[arrayPos.y, arrayPos.x];
    }

    private void RemoveObstacleTile(Vector3Int cellPos, bool isBomb = false)
    {
        Vector3Int arrayPos = ChangeCellPosToArrayPos(cellPos);
        isObstacleRemoved[arrayPos.y, arrayPos.x] = true;
        grid.RemoveObstacleTile(cellPos, isBomb);
    }

    private void ChangeTotalToSeperate(Vector3Int cellPos)
    {
        Vector3Int arrayPos = ChangeCellPosToArrayPos(cellPos);
        if(CheckHasObstacle(cellPos)) return; // 해당 위치에 장애물 타일이 있으면 그 자리에서 반환
        if(totalNumArray[arrayPos.y, arrayPos.x] == 0) return; // 만약 해당 위치가 0이어도 반환 (써도 의미가 없음)
        if(totalNumMask[arrayPos.y, arrayPos.x]) return;

        magGlassCount--;
        EventManager.instance.InvokeEvent(EventType.Item_Use, Item.Mag_Glass, magGlassCount);

        totalNumMask[arrayPos.y, arrayPos.x] = true;
        grid.UpdateSeperateNum(mineNumArray, treasureNumArray, cellPos);
    }

    private void SetFlag(Vector3Int cellPos, bool forceful = false)
    {
        Vector3Int arrayPos = ChangeCellPosToArrayPos(cellPos);
        if(!(CheckHasObstacle(cellPos))) return; // 해당 위치에 장애물 타일이 없으면 무시

        if(forceful)
        {
            flagArray[arrayPos.y, arrayPos.x] = 0;
            grid.SetFlag(cellPos, Flag.None);
        }else
        {
            Flag[] flagEnumArray = (Flag[]) Enum.GetValues(typeof(Flag));
            flagArray[arrayPos.y, arrayPos.x] = (flagArray[arrayPos.y, arrayPos.x] + 1) % flagEnumArray.Length;
            grid.SetFlag(cellPos, flagEnumArray[flagArray[arrayPos.y, arrayPos.x]]);
        }
    }

    private void SetTreasureSearch(Vector3Int cellPos, bool forceful = false)
    {
        Vector3Int arrayPos = ChangeCellPosToArrayPos(cellPos);
        if(!(CheckHasObstacle(cellPos))) return; // 해당 위치에 장애물 타일이 없으면 무시
        if(treasureSearchMask[arrayPos.y, arrayPos.x] && !forceful) return;

        if(forceful)
        {
            grid.SetTreasureSearch(cellPos, TreasureSearch.None);
        }else
        {
            holyWaterCount--;
            EventManager.instance.InvokeEvent(EventType.Item_Use, Item.Holy_Water, holyWaterCount);

            if(mineTreasureArray[arrayPos.y, arrayPos.x] == -2) // 보물
            {
                // 보물이 맞다고 해당 장애물 위에 띄움
                grid.SetTreasureSearch(cellPos, TreasureSearch.Yes);
            }else
            {
                // 보물이 아니라고 해당 장애물 위에 띄움
                grid.SetTreasureSearch(cellPos, TreasureSearch.No);
            }

            treasureSearchMask[arrayPos.y, arrayPos.x] = true;
        }
    }

    private void GetItem(bool isUsable)
    {
        if(isUsable)
        {
            // 적은 개수를 가지는 아이템이 나오도록 바꿔야 한다
            Item randUsableItem = (Item)UnityEngine.Random.Range(1, 4);

            switch(randUsableItem)
            {
                case Item.Potion :
                    potionCount += 1;
                    EventManager.instance.InvokeEvent(EventType.Item_Obtain, randUsableItem, potionCount);
                    break;
                case Item.Mag_Glass :
                    magGlassCount += 1;
                    EventManager.instance.InvokeEvent(EventType.Item_Obtain, randUsableItem, magGlassCount);
                    break;
                case Item.Holy_Water :
                    holyWaterCount += 1;
                    EventManager.instance.InvokeEvent(EventType.Item_Obtain, randUsableItem, holyWaterCount);
                    break;
            }

        }else
        {

        }
    }


    [Button]
    public void StageInitialize(int width = DefaultX ,  int height = DefaultY, Difficulty difficulty = Difficulty.Hard, int maxHeart = 9,  int currentHeart = 1, int potionCount = 5, int magGlassCount = 5, int holyWaterCount = 5, int totalTime = 60)
    {
        isNowInitializing = true;

        totalNumArray = null;
        totalNumMask = null;
        treasureSearchMask = null;

        mineNumArray = null;
        treasureNumArray = null;

        this.maxHeart = maxHeart;
        this.currentHeart = currentHeart;

        flagArray = new int[height, width];
        isObstacleRemoved = new bool[height, width];

        startX = -1;
        startY = -1;

        this.width = width;
        this.height = height;

        this.potionCount = potionCount;
        this.magGlassCount = magGlassCount;
        this.holyWaterCount = holyWaterCount;

        if(Application.isPlaying)
        {
            EventManager.instance.InvokeEvent(EventType.Set_Heart, currentHeart, maxHeart);

            EventManager.instance.InvokeEvent(EventType.Item_Obtain, Item.Potion, potionCount);
            EventManager.instance.InvokeEvent(EventType.Item_Obtain, Item.Mag_Glass, magGlassCount);
            EventManager.instance.InvokeEvent(EventType.Item_Obtain, Item.Holy_Water, holyWaterCount);
        }
        
        MakeMineTreasureArray(width, height, difficulty);

        UpdateArrayNum(Total_Mine_Treasure.Total);
        UpdateArrayNum(Total_Mine_Treasure.Mine);
        UpdateArrayNum(Total_Mine_Treasure.Treasure);

        grid.ShowEnvironment(width, height);
        grid.ShowTotalNum(totalNumArray, totalNumMask);
        grid.ShowMineTreasure(mineTreasureArray);


        RemoveObstacle(new Vector3Int(0,0,0));

        CameraSize_Change.ChangeCameraSizeFit();

        timerCoroutine = StartCoroutine(StartTimer(totalTime)); 

        PlayerManager.instance.SetPlayerPositionStart();

        isNowInitializing = false;
    }

    IEnumerator StartTimer(int totalTime)
    {
        this.totalTime = totalTime;
        timeElapsed = 0;
        EventManager.instance.TimerInvokeEvent(timeElapsed, timeLeft);

        while(timeLeft > 0)
        {
            yield return new WaitForSeconds(1);

            if(!isStageInputBlocked)
            {
                timeElapsed++;
                EventManager.instance.TimerInvokeEvent(timeElapsed, timeLeft);
            }
            
        }

        EventManager.instance.InvokeEvent(EventType.Game_Over, GameOver_Reason.TimeOver);
    }

    [Button]
    void UpdateArrayNum(Total_Mine_Treasure numMode)
    {
        int height = mineTreasureArray.GetLength(0);
        int width = mineTreasureArray.GetLength(1);

        int[,] targetNumArray = null;
        switch(numMode)
        {
            case Total_Mine_Treasure.Total :
                targetNumArray = totalNumArray;
                break;
            case Total_Mine_Treasure.Mine :
                targetNumArray = mineNumArray;
                break;
            case Total_Mine_Treasure.Treasure :
                targetNumArray = treasureNumArray;
                break;
        }

        if(targetNumArray == null)
        {
            switch(numMode)
            {
            case Total_Mine_Treasure.Total :
                totalNumArray = new int[height, width];
                totalNumMask = new bool[height, width];
                treasureSearchMask = new bool[height, width];
                targetNumArray = totalNumArray;
                break;
            case Total_Mine_Treasure.Mine :
                mineNumArray = new int[height, width];
                targetNumArray = mineNumArray;
                break;
            case Total_Mine_Treasure.Treasure :
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

            for(int i=0; i<height; i++)
            {
                for(int j=0; j<width; j++)
                {
                    targetNumArray[i,j] = 0;
                }
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

        // String str = "";
        // for(int i=0; i< height; i++)
        // {
        //     for(int j=0; j< width; j++)
        //     {
        //         str += targetNumArray[i, j].ToString();
        //         str += "  ";
        //     }

        //     str += "\n";
        // }

        // Debug.Log(str);

    }


    [Button]
    public void MakeMineTreasureArray(int width = 10, int height = 10, Difficulty difficulty = Difficulty.Easy)
    {
        mineTreasureArray = new int[height, width];
        int totalBockNum = height * width;

        EventManager.instance.InvokeEvent(EventType.Set_Width_Height, new Vector2(width, height));
        
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

        EventManager.instance.InvokeEvent(EventType.MineAppear, mineCount);
        EventManager.instance.InvokeEvent(EventType.TreasureAppear, treasureCount);

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

        // String str = "";
        // for(int i=0; i< height; i++)
        // {
        //     for(int j=0; j< width; j++)
        //     {
        //         str += mineTreasureArray[i, j].ToString();
        //         str += "  ";
        //     }

        //     str += "\n";
        // }

        // str += "\nmineCount : " + mineCount;
        // str += "\ntreasureCount : " + treasureCount;


        // Debug.Log(str);

    }

    private void GameOver(bool isGameOver, GameOver_Reason reason)
    {
        if(isGameOver)
        {
            stageInputBlock++;
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }else
        {
            StageInitialize();
            stageInputBlock =0;
        }
        
    }

    public void RestartGame()
    {
        EventManager.instance.InvokeEvent(EventType.Game_Restart);
    }

    private void HeartChange(int changeValue)
    {
        currentHeart += changeValue;
        if(currentHeart < 0) currentHeart = 0;

        EventManager.instance.InvokeEvent(EventType.Set_Heart, currentHeart, maxHeart);

        if(currentHeart == 0)
        {
            EventManager.instance.InvokeEvent(EventType.Game_Over, GameOver_Reason.Heart0);
        }
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
