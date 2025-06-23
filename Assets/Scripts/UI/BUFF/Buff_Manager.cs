using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Buff_Manager : MonoBehaviour
{
    static Buff_Manager _my;
    public static Buff_Manager _instance { get { return _my; } }

    [SerializeField] Transform _parentTransform;
    [SerializeField] GameObject _prefab;
    private void Awake()
    {
        _my = this;
    }
    public void Create(Sprite icon, float duration, int index, Effect_Type type, int value)
    {
        GameObject obj = Instantiate(_prefab);
        obj.transform.SetParent(_parentTransform);
        obj.GetComponent<Buff>().Init(icon, duration, type, value);
        SoundManager._instance.PrivatePlaySound(SkillData._instance.Sound[index], UserControl._instance.transform, 0.5f);
    }
}
