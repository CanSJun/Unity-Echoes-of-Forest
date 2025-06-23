using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpControl : MonoBehaviour
{
    static ExpControl _my;
    public static ExpControl _instance { get { return _my; } }
    private void Awake()
    {
        _my = this;
    }
    public Text _text;
    public Image _Fill;


    public void GetExp(float Value)
    {
        UserControl._instance._EXP += Value;
        _Fill.fillAmount = UserControl._instance._EXP / 100f;
        while (UserControl._instance._EXP >= 100)
        {
            UserControl._instance._EXP -= 100;
            UserControl._instance._level += 1;
            UserControl._instance._SP += 1;
            _Fill.fillAmount = 0;
            SkillUI._instance.UpdateSp();
        }
        _text.text = $"Level: {UserControl._instance._level} EXP: {UserControl._instance._EXP}/100";



    }
}
