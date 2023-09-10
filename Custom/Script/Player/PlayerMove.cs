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
        StageManager.stageInputBlock++;
        
        if(gap != Vector3Int.forward)
        {
            StartCoroutine(MoveDirectly(gap));
        }else
        {
            StartCoroutine(MoveTeleport(cellPosition));
        }
        
    }

    IEnumerator MoveDirectly(Vector3Int moveVector)
    {
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
