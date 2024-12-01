using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas {
        get {
            return GetComponentInParent<Canvas>();
        }
    }

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Text count
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).GetComponent<Text>();
            }
            return null;
        }
    }

    public static GameObject itemBeingDragged;
    public static Transform startParent;

    private void Start()
    {
        transform.localScale = Vector3.one;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        startParent = transform.parent;
        transform.SetParent(canvas.transform);
        itemBeingDragged = gameObject;
        if (eventData.button == PointerEventData.InputButton.Right && Int16.Parse(count.text) != 1)
        {
            GameObject tmp = (GameObject)Instantiate(Resources.Load<GameObject>(name), startParent.transform.position, startParent.transform.rotation, startParent.transform);
            tmp.name = name;
            tmp.transform.GetChild(0).GetComponent<Text>().text = Mathf.Floor(Int16.Parse(count.text)/2f).ToString();
            count.text = (Int16.Parse(count.text) - Int16.Parse(tmp.transform.GetChild(0).GetComponent<Text>().text)).ToString();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, null, out localPoint);
        rectTransform.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;

        if (transform.parent == canvas.transform)
        {
            if (startParent.childCount > 0)
            {
                count.text = (Int16.Parse(count.text) + Int16.Parse(startParent.GetChild(0).GetComponent<DragDrop>().count.text)).ToString();
                Destroy(startParent.GetChild(0).gameObject);
            }

            transform.SetParent(startParent);
            rectTransform.localPosition = Vector2.zero;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}