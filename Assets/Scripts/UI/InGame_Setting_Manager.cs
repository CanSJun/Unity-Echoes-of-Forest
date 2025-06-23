using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_Setting_Manager : MonoBehaviour
{
    [SerializeField]GameObject _prevtransform;


    [SerializeField] Text BGM_Text;
    [SerializeField] Text Effect_Text;

    [SerializeField] Text Sensitive_X_Text;
    [SerializeField] Text Sensitive_Y_Text;

    [SerializeField] Slider BGM;
    [SerializeField] Slider Effect;
    [SerializeField] Slider Sensitive_X;
    [SerializeField] Slider Sensitive_Y;


    public void OnExit()
    {
        _prevtransform.SetActive(true);
        gameObject.SetActive(false);
    }


    void Start()
    {
        BGM.onValueChanged.AddListener(BGM_Change);
        Effect.onValueChanged.AddListener(Effect_Change);
        Sensitive_X.onValueChanged.AddListener(Sensitive_X_Change);
        Sensitive_Y.onValueChanged.AddListener(Sensitive_Y_Change);

        BGM_Text.text = Mathf.Floor(MasterControl._Instance.BGM_Volume * 100).ToString();
        BGM.value = MasterControl._Instance.BGM_Volume;
        Effect_Text.text = Mathf.Floor(MasterControl._Instance.Effect_Volume * 100).ToString();
        Effect.value = MasterControl._Instance.Effect_Volume;
        Sensitive_X_Text.text = Mathf.Floor(MasterControl._Instance.Sensitive_X * 100).ToString();
        Sensitive_X.value = MasterControl._Instance.Sensitive_X;
        Sensitive_Y_Text.text = Mathf.Floor(MasterControl._Instance.Sensitive_Y * 100).ToString();
        Sensitive_Y.value = MasterControl._Instance.Sensitive_Y;
    }


    public void BGM_Change(float x)
    {
        MasterControl._Instance.BGM_Volume = x;
        BGM_Text.text = Mathf.Floor(x * 100).ToString();
    }

    public void Effect_Change(float x)
    {
        MasterControl._Instance.Effect_Volume = x;
        Effect_Text.text = Mathf.Floor(x * 100).ToString();
    }
    public void Sensitive_X_Change(float x)
    {
        MasterControl._Instance.Sensitive_X = x;
        Sensitive_X_Text.text = Mathf.Floor(x * 100).ToString();
    }

    public void Sensitive_Y_Change(float x)
    {
        MasterControl._Instance.Sensitive_Y = x;
        Sensitive_Y_Text.text = Mathf.Floor(x * 100).ToString();
    }
}
