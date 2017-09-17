using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VertexData : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int _vertexId;
    public Canvas parentCanvas;
    public static GameObject itemBeingDragged;

    private RectTransform _rectTrans;
    private Vector3 startPosition;

    public UntangleMapMaker uManager;

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
        startPosition = _rectTrans.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(Input.mousePosition);
        _rectTrans.localPosition = Input.mousePosition - new Vector3(parentCanvas.pixelRect.width * 0.5f, parentCanvas.pixelRect.height * 0.5f, 0.0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;

        //if (_rectTrans.position.y > parentCanvas.pixelRect.yMin && _rectTrans.position.y < parentCanvas.pixelRect.yMax
        //    && _rectTrans.position.x > parentCanvas.pixelRect.xMin && _rectTrans.position.x < parentCanvas.pixelRect.xMax)
        //    return;
        //else
        //    _rectTrans.position = startPosition;
    }

    // click event
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (uManager == null)
            uManager = parentCanvas.GetComponent<UntangleMapMaker>();

        if (uManager.isSelected == true)
            uManager.LinkNodes(_vertexId, uManager.selectedID);
        else {
            uManager.isSelected = true;
            uManager.selectedID = _vertexId;
        }
    }
}
