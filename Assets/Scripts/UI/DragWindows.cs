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
       _CurrentPosition = (Vector2)_windows.position - eventData.position; // object ��ġ���� ���� ��ġ�� ������� ���� Ŭ���� ��ġ!
        gameObject.transform.SetAsLastSibling(); // �� ������!! 
    }

}
