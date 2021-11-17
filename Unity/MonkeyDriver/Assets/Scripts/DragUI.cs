using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerClickHandler, IDropHandler
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
        popVec = new Vector2(5,5);
    }

    public void Update()
    {
        if (BusControls.atBoundUp && ctrlNum == 0) { GetComponent<Image>().color = new Color32(255, 255, 225, 100); }
        if (!BusControls.atBoundUp && ctrlNum == 0) { GetComponent<Image>().color = new Color32(255, 255, 225, 255); }
        if (BusControls.atBoundDown && ctrlNum == 1) { GetComponent<Image>().color = new Color32(255, 255, 225, 100); }
        if (!BusControls.atBoundDown && ctrlNum == 1) { GetComponent<Image>().color = new Color32(255, 255, 225, 255); }
        if (BusControls.atBoundLeft && ctrlNum == 2) { GetComponent<Image>().color = new Color32(255, 255, 225, 100); }
        if (!BusControls.atBoundLeft && ctrlNum == 2) { GetComponent<Image>().color = new Color32(255, 255, 225, 255); }
        if (BusControls.atBoundRight && ctrlNum == 3) { GetComponent<Image>().color = new Color32(255, 255, 225, 100); }
        if (!BusControls.atBoundRight && ctrlNum == 3) { GetComponent<Image>().color = new Color32(255, 255, 225, 255); }
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

    public void OnPointerClick(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition -= popVec;
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

            eventData.pointerDrag.GetComponent<DragUI>().isEnabled = !eventData.pointerDrag.GetComponent<DragUI>().isEnabled;
            isEnabled = !isEnabled;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        dragRectTransform.anchoredPosition = defaultPos;
    }

}