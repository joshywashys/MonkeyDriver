using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IDropHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    RectTransform dragRectTransform;
    Vector2 popVec;
    Vector2 defaultPos;

    public int ctrlNum;
    public bool isEnabled;
    private bool isAvailable;

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();

        dragRectTransform = GetComponent<RectTransform>();
        defaultPos = dragRectTransform.anchoredPosition;
        popVec = new Vector2(10,10);
    }
        
    //atBoundUp, atBoundLeft, atBoundRight, atBoundDown

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += popVec;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Vector2 temp = eventData.pointerDrag.GetComponent<DragUI>().defaultPos;

        if (isEnabled != eventData.pointerDrag.GetComponent<DragUI>().isEnabled && eventData.pointerDrag.tag == "Controls")
        {
            eventData.pointerDrag.GetComponent<DragUI>().defaultPos = defaultPos;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = defaultPos;
            defaultPos = temp;
            GetComponent<RectTransform>().anchoredPosition = temp;

            isEnabled = !isEnabled;
            eventData.pointerDrag.GetComponent<DragUI>().isEnabled = !eventData.pointerDrag.GetComponent<DragUI>().isEnabled;
        }
        if (eventData.pointerDrag.tag == "Banana")
        {
            eventData.pointerDrag.GetComponent<dragBanana>().defaultPos = defaultPos;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        dragRectTransform.anchoredPosition = defaultPos;
    }

}