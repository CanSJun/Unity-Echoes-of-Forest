using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Buff : MonoBehaviour
{

    [SerializeField] private Image _Image;
    [SerializeField] private Image _duration_image;



    private UserControl Player;
    private int _value;
    private Effect_Type _type;
    public void Init(Sprite sprite, float duration, Effect_Type type, int value)
    {
        _Image.sprite = sprite;
        _type = type;
        Player = UserControl._instance;
        _value = value;
        UpdateBuff(true);
        StartCoroutine(Duration(duration));
    }

    private void UpdateBuff(bool check)
    {
        switch (_type)
        {
            case Effect_Type.Damage_UP:
                    if (check) Player._damage += _value;
                    else Player._damage -= _value;
                break;
            case Effect_Type.Def_Up:
                    if (check) Player._def += _value;
                    else Player._def -= _value;
                break;
        }
    }

    private IEnumerator Duration(float _duration)
    {
        float now_time = 0f;
        while (now_time < _duration)
        {
            _duration_image.fillAmount = now_time / _duration;
            now_time += Time.deltaTime;
            yield return null;
        }
        UpdateBuff(false);
        Destroy(gameObject);
    }
}
