using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Hp_Bar_Buff : MonoBehaviour
{
    [SerializeField] Transform _parentTransform;
    [SerializeField] GameObject _prefab;
    [SerializeField] GameObject _Boss_prefab;


    bool CheckDuplication(Debuff_Type type, float duration)
    {
        foreach(Transform Child in _parentTransform)
        {
            if(Child.GetComponent<Hp_Bar_Buff_Sub>() != null)
            {
                if(type == Child.GetComponent<Hp_Bar_Buff_Sub>().GetBuffType())
                {
                    Child.GetComponent<Hp_Bar_Buff_Sub>().Duplication(duration);
                    return false;
                }
            }
        }
        return true;
    }
    public void Init(float duration, MonsterControl Monster, Debuff_Type type, Sprite icon, bool check)
    {
        if (CheckDuplication(type, duration))
        {
            GameObject obj = Instantiate(check ? _Boss_prefab : _prefab);
            obj.transform.SetParent(_parentTransform, false);
            
            obj.GetComponent<Hp_Bar_Buff_Sub>().StartBuff(duration, Monster, type, icon);
        }
    }

}
