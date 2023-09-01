using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehindCamera : MonoBehaviour
{
    public Camera uiCamera; // 대상 카메라. 인스펙터에서 지정해주세요.
    public float referenceOrthographicSize = 5f; // 참조 카메라의 orthographic 크기
    public Vector2 referenceCanvasSize = new Vector2(1920, 1080); // 원하는 Canvas 크기

    private Canvas canvas;
    private RectTransform rectTransform;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();

        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogError("Canvas RenderMode is not World Space!");
            return;
        }

        AdjustCanvasSize();
    }

    void Update()
    {
        AdjustCanvasSize();
    }

    void AdjustCanvasSize()
    {
        if(uiCamera.orthographic)
        {
            float scaleRatio = uiCamera.orthographicSize / referenceOrthographicSize;
            rectTransform.sizeDelta = referenceCanvasSize * scaleRatio;
        }

    }
}
