using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraSize_Change : MonoBehaviour
{
    private Grid Stage;
    private Tilemap tilemap;

    private Camera camera;

    private void Awake() {
        Stage = FindObjectOfType<Grid>();
        tilemap = Stage.transform.GetChild(0).GetComponent<Tilemap>();
        camera = GetComponent<Camera>();
    }

    private void Start() {
        camera.orthographicSize = tilemap.size.y /2 + 0.2f;
    }
}
