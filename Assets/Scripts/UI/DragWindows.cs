using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindows : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] RectTransform _windows;
 
    private Vector2 _CurrentPosition;
    public void OnDrag(PointerEventData eventData)
    {
        _windows.position = eventData.position + _CurrentPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
       _CurrentPosition = (Vector2)_windows.position - eventData.position; // object 위치에서 현재 위치를 빼줘야지 내가 클릭한 위치!
        gameObject.transform.SetAsLastSibling(); // 맨 앞으로!! 
    }

}
