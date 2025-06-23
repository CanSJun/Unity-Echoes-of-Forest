using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{

    static SkillUI _my;
    public static SkillUI _instance { get { return _my; } }
    [SerializeField] private List<GameObject>_Slots = new List<GameObject>();
    [SerializeField] private Text _SP;

    private void Awake()
    {
        _my = this;
    }
    private SkillControl skillcontrol;

    private bool[] _Checked;

    private string leveltext;
    void Start()
    {
        skillcontrol = SkillControl._instance;
        _Checked = new bool[_Slots.Count]; // ���� ��ŭ �Ҵ�!
        UiUpdate();
        UpdateSp();
    }


     void UiUpdate()
    {
        for(int i = 0; i < _Slots.Count; i++) {
            if (_Checked[i]) continue; // ���࿡ �ѹ� Ȱ��ȭ �� ���� üũ �Ͽ����� �ٽô� �Ⱥҷ�����.
            if (skillcontrol.UpdateSkill(_Slots[i].name, out leveltext))
            {
                //Debug.Log($"{_Slots[i].name} ��ų�� ��� ����");
                _Checked[i] = true;
                Finding<Image>(_Slots[i], "Icon").color = Color.white;
                Finding<Text>(_Slots[i], "Text").text = leveltext;
            }
        }
    }


    //���� ���ϴ� ������Ʈ ã��
    private T Finding<T> (GameObject parent, string name) where T : Component
    {
        Transform find = parent.transform.Find(name);
        if(find != null) return find.GetComponent<T>();
        return null;
    }

    public void UpdateSp() => _SP.text = $"SP : {UserControl._instance._SP}";


}
