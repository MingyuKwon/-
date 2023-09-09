using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    PlayerAnimation playerAnimation;
    PlayerMove playerMove;

    public Transform playerTransform;

    public Vector3Int checkPlayerNearFourDirection(Vector3Int checkPosition){
        Vector3Int PlayerCellPosition = TileGrid.CheckCellPosition(transform.position);
        Vector3Int gap = checkPosition - PlayerCellPosition;

        if(gap.magnitude == 1)
        {
            return gap;
        }

        return Vector3Int.zero;
    }

    private void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else
        {
            Destroy(this.gameObject);
        }

        playerAnimation = GetComponent<PlayerAnimation>();
        playerMove = GetComponent<PlayerMove>();
        playerTransform = GetComponent<Transform>();

        playerTransform.position = new Vector3(0.5f, 0.5f, 0);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0))
        {
            
        }
    }

}
