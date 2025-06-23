using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopTextControl : MonoBehaviour
{
    
    [Header("Á¤º¸")]
    [SerializeField] private TextMeshProUGUI _text;
    public float _speed = 50f;
    public float _duration = 1f;


    private RectTransform _Rect;
    private Color _oriColor;
    private Vector2 _oriScale;

    public void Change_Text(string text) {
        _Rect = transform as RectTransform;
        _oriColor = _text.color;
        _oriScale = _Rect.localScale;
        _text.text = text;
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        float current_time = 0f;

        Vector2 _Ori = _Rect.anchoredPosition;

        yield return new WaitForSeconds(0.5f);
        while(current_time < _duration)
        {
            _Rect.anchoredPosition = _Ori + Vector2.up * (_speed * current_time);
            float a = Mathf.Lerp(1, 0 , current_time/ _duration);
            _text.color = new Color(_oriColor.r, _oriColor.g, _oriColor.b, a);
            _Rect.localScale = _oriScale * a;

            
            
            current_time += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
