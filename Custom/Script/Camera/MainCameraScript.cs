using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    Transform thisTransform;
    Transform player;
    private void Awake() {
        thisTransform = GetComponent<Transform>();
    }

    private void Start() {
        player = PlayerManager.instance.playerTransform;
    }
    void Update()
    {
        thisTransform.position = new Vector3(player.position.x, player.position.y,thisTransform.position.z); 
    }
}
