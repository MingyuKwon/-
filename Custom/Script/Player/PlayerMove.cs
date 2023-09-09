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
            
        }
        
    }

    IEnumerator MoveDirectly(Vector3Int moveVector)
    {
        transform.position += moveVector;

        yield return null;

        StageManager.stageInputBlock--;
    }
}
