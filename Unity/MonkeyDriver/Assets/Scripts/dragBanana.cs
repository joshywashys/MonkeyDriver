using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class dragBanana : MonoBehaviour, IDragHandler
{
    private Canvas canvas;
    RectTransform dragRectTransform;

    Vector2 popVec;
    public Vector2 defaultPos;

    public void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        dragRectTransform = GetComponent<RectTransform>();
        defaultPos = dragRectTransform.anchoredPosition;
        popVec = new Vector2(10, 10);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

}
