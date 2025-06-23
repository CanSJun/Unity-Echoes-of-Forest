using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPSCRIPT : MonoBehaviour
{
    [Header("구성요소")]
    public TextMeshProUGUI _text;
    public TextMeshProUGUI _info_text;
    public Image HP;
    public float _MAX_HP = 100;
    public float _NOW_HP = 0;
    void Start()
    {
        _text.text = _NOW_HP.ToString() + "/" + _MAX_HP.ToString();
    }
    void Update()
    {

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

    }


    public void ChangeHpText(float value)
    {
        HP.fillAmount = (value / _MAX_HP);
        value = (Mathf.FloorToInt(value));
        _text.text = value.ToString() +"/"+ _MAX_HP.ToString();
        
    }
    public void ChangeInfoText(string txt)
    {
        _info_text.text = txt;
    }
}
