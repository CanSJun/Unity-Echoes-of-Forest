using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPMP_Control : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] Image _HPMP;
    [SerializeField] Image _Surface;
    [SerializeField] float pos = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeValue(float value)
    {
        if (value <= 0) return; // ���࿡ ��ġ�� 0�̸� return!
        _HPMP.fillAmount -= value;

        _Surface.fillAmount = _HPMP.fillAmount + pos; // surface ó��
    }
    public void iniChangeValue(float value)
    {
        _HPMP.fillAmount = value;

        _Surface.fillAmount = _HPMP.fillAmount + pos; // surface ó��
    }

    public void ChangeValueUp(float value)
    {
        _HPMP.fillAmount += value;
        _Surface.fillAmount = _HPMP.fillAmount + pos; // surface ó��
    }
}
