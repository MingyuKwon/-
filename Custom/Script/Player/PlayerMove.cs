using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private void OnEnable() {
        InputManager.InputEvent.MovePressEvent += MovePlayer;
    }

    private void OnDisable() {
        InputManager.InputEvent.MovePressEvent -= MovePlayer;
    }

    private void MovePlayer(Vector3Int cellPosition)
    {
        Vector3Int gap = PlayerManager.instance.checkPlayerNearFourDirection(cellPosition);

        if(gap == Vector3Int.zero) return;
        
        if(gap != Vector3Int.forward)
        {
            if(gap.magnitude == 1) 
            {
                StartCoroutine(MoveOneDirectly(gap));
            }else
            {
                StartCoroutine(MoveTwoDirectly(gap));
            }

            
        }else
        {
            StartCoroutine(MoveTeleport(cellPosition));
        }
    }

    private Vector3Int[] twoTileVectorPositons = new Vector3Int[8] 
    {
        new Vector3Int(2,0,0),
        new Vector3Int(0,2,0) ,
        new Vector3Int(0,-2,0) , 
        new Vector3Int(-2,0,0) ,
        new Vector3Int(1,1,0) ,
        new Vector3Int(1,-1,0), 
        new Vector3Int(-1,1,0),
        new Vector3Int(-1,-1,0)
    };

    IEnumerator MoveTwoDirectly(Vector3Int moveVector)
    {
        StageManager.stageInputBlock++;

        if(moveVector == twoTileVectorPositons[0])
        {
            yield return StartCoroutine(MoveOneDirectly(Vector3Int.right));
            yield return StartCoroutine(MoveOneDirectly(Vector3Int.right));
        }else if(moveVector == twoTileVectorPositons[1])
        {
            yield return StartCoroutine(MoveOneDirectly(Vector3Int.up));
            yield return StartCoroutine(MoveOneDirectly(Vector3Int.up));
        }
        else if(moveVector == twoTileVectorPositons[2])
        {
            yield return StartCoroutine(MoveOneDirectly(Vector3Int.down));
            yield return StartCoroutine(MoveOneDirectly(Vector3Int.down));
        }
        else if(moveVector == twoTileVectorPositons[3])
        {
            yield return StartCoroutine(MoveOneDirectly(Vector3Int.left));
            yield return StartCoroutine(MoveOneDirectly(Vector3Int.left));
        }
        else if(moveVector == twoTileVectorPositons[4])
        {
            if(!TileGrid.CheckObstaclePosition(PlayerManager.instance.PlayerCellPosition + Vector3Int.right))
            {
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.right));
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.up));
            }else if(!TileGrid.CheckObstaclePosition(PlayerManager.instance.PlayerCellPosition + Vector3Int.up))
            {
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.up));
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.right));
            }
        }
        else if(moveVector == twoTileVectorPositons[5])
        {
            if(!TileGrid.CheckObstaclePosition(PlayerManager.instance.PlayerCellPosition + Vector3Int.right))
            {
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.right));
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.down));
            }else if(!TileGrid.CheckObstaclePosition(PlayerManager.instance.PlayerCellPosition + Vector3Int.down))
            {
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.down));
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.right));
            }
        }
        else if(moveVector == twoTileVectorPositons[6])
        {
            if(!TileGrid.CheckObstaclePosition(PlayerManager.instance.PlayerCellPosition + Vector3Int.left))
            {
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.left));
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.up));
            }else if(!TileGrid.CheckObstaclePosition(PlayerManager.instance.PlayerCellPosition + Vector3Int.up))
            {
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.up));
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.left));
            }
        }
        else if(moveVector == twoTileVectorPositons[7])
        {
            if(!TileGrid.CheckObstaclePosition(PlayerManager.instance.PlayerCellPosition + Vector3Int.left))
            {
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.left));
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.down));
            }else if(!TileGrid.CheckObstaclePosition(PlayerManager.instance.PlayerCellPosition + Vector3Int.down))
            {
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.down));
                yield return StartCoroutine(MoveOneDirectly(Vector3Int.left));
            }
        }

        StageManager.stageInputBlock--;
    }

    IEnumerator MoveOneDirectly(Vector3Int moveVector)
    {
        StageManager.stageInputBlock++;

        PlayerManager.instance.playerAnimation.MoveAnimation(moveVector);

        Vector3 moveUnitVec = (Vector3)moveVector / 5f;
        
        yield return new WaitForSeconds(0.02f);
        transform.position += moveUnitVec;

        yield return new WaitForSeconds(0.02f);
        transform.position += moveUnitVec;

        yield return new WaitForSeconds(0.02f);
        transform.position += moveUnitVec;

        yield return new WaitForSeconds(0.02f);
        transform.position += moveUnitVec;

        yield return new WaitForSeconds(0.02f);
        transform.position += moveUnitVec;

    
        StageManager.stageInputBlock--;
    }

    IEnumerator MoveTeleport(Vector3Int movePosition)
    {
        PlayerManager.instance.playerAnimation.MoveAnimation(Vector3Int.forward);

        yield return new WaitForSeconds(0.2f);
        transform.position =  TileGrid.CheckWorldPosition(movePosition);
        yield return new WaitForSeconds(0.2f);
    
        StageManager.stageInputBlock--;
    }
}
