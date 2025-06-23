using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalUiControl : MonoBehaviour
{
    static TotalUiControl _my;
    public static TotalUiControl _instance { get { return _my; } }

    [Header("UI ǥ�� Object")]
    [Space(10)]
    [Header("���ϴ� key�� �������� Prefab")]
    [SerializeField] private  GameObject _EventSystem;


    private void Awake()
    {
        _my = this;
    }
    public enum Type
    {
        EventSystem = 0,
        ItemDescription,
        SkillDescription
    }

    public GameObject Create(Type type)
    {
        GameObject obj = null;
        switch (type)
        {
            case Type.EventSystem:
                obj = Instantiate(_EventSystem, transform);
            break;
        }
        return obj;
    }
}
