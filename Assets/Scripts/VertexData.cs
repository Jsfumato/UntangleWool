using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VertexData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int _vertexId;
    public Canvas parentCanvas;
    public static GameObject itemBeingDragged;

    private RectTransform _rectTrans;
    private Vector3 startPosition;

    public RectTransform RectTransform
    {
        get {
            if (_rectTrans == null)
                _rectTrans = GetComponent<RectTransform>();
            return _rectTrans;
        }
    }

    public void Start()
    {
        _rectTrans = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = this.gameObject;
        startPosition = _rectTrans.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTrans.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;

        if (_rectTrans.position.y > parentCanvas.pixelRect.yMin && _rectTrans.position.y < parentCanvas.pixelRect.yMax
            && _rectTrans.position.x > parentCanvas.pixelRect.xMin && _rectTrans.position.x < parentCanvas.pixelRect.xMax)
            return;
        else
            _rectTrans.position = startPosition;
    }
}
