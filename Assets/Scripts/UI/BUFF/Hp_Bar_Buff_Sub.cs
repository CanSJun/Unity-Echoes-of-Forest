using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;


public enum Debuff_Type
{
    Electric_Shock,
    Blooding,
}

public class Hp_Bar_Buff_Sub : MonoBehaviour
{
    [SerializeField] Image _duration_image;
    [SerializeField] Image _icon;

    private MonsterControl _Owner;
    private Debuff_Type _type;

    private float _delay = 0.3f;
    private float _currentdelay = 0f;

    public Debuff_Type GetBuffType() => _type;
    private Coroutine _currentCoroutine;
    public void Duplication(float duration)
    {
        StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(Duration(duration));
    }
    public void StartBuff(float duration, MonsterControl Monster, Debuff_Type type, Sprite icon)
    {
        _Owner = Monster;
        _type = type;
        _icon.sprite = icon;
        if (this.gameObject.activeSelf == true)
        {
            _currentCoroutine = StartCoroutine(Duration(duration));
        }
    }


    private IEnumerator Duration(float _duration)
    {
        float now_time = 0f;
        while (now_time < _duration)
        {
            if (this.gameObject.activeSelf == false) yield break;

                _duration_image.fillAmount = now_time / _duration;
            now_time += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        if(_currentdelay < _delay)
        {
            _currentdelay += Time.deltaTime;
            return;
        }
        else
        {
            _currentdelay = 0f;
        }

        switch (_type)
        {
            case Debuff_Type.Electric_Shock:
                _Owner.CheckHitted(0.1f);
                break;
        }
    }
}
