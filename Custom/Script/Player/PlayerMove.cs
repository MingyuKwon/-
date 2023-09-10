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
        if(gap != Vector3Int.zero)
        {
            StageManager.stageInputBlock++;
            StartCoroutine(MoveDirectly(gap));
        }else
        {
            StageManager.stageInputBlock++;
            StartCoroutine(MoveTeleport(cellPosition));
        }
        
    }

    IEnumerator MoveDirectly(Vector3Int moveVector)
    {
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
        yield return new WaitForSeconds(0.05f);
        transform.position =  TileGrid.CheckWorldPosition(movePosition);
        yield return new WaitForSeconds(0.05f);
    
        StageManager.stageInputBlock--;
    }
}
