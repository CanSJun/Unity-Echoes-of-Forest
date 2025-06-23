using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EventSystemControl : MonoBehaviour
{
    private Text _child;

    private Transform target;
    public void Init(Transform obj, string test, string key_text)
    {
        _child = GetComponentInChildren<Text>();
        if (obj == null || (test == null || key_text == null) || _child == null) { Destroy(gameObject); return; }
        target = obj;
        UpdatePoint();
        Chnage_Text(test, key_text);


    }

    void UpdatePoint()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvas_rect = canvas.GetComponent<RectTransform>();

        Vector2 ScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas_rect, ScreenPos, canvas.worldCamera, out Vector2 localPoint);

        RectTransform rect = GetComponent<RectTransform>();
        rect.localPosition = localPoint;

    }
    void Update() => UpdatePoint();


    public void Chnage_Text(string test, string key_text) => _child.text = test + " <color=#FFD500>"+ key_text + " </color>(을/를) 누르세요";
    

    public void OnOff(bool b) => _child.gameObject.SetActive(b);
}
