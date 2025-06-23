using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static ItemControl;

public enum Skill_Type
{
    None,
    blade,
    target,
    Field,
    Buff
}

public enum Effect_Type
{
    None,
    Lightning,
    Slow,
    Damage_UP,
    Def_Up
}


public class SkillControl : MonoBehaviour
{

    static SkillControl _my;
    public static SkillControl _instance { get { return _my; } }

    [Header("정보")]
    public List<Skill> skills;

    private void Awake()
    {
        _my = this;

        for (int i = 0; i < skills.Count; i++)
        {

            InformationUpdate(i);
        }
    }






    void Start()
    {

    }

    void InformationUpdate(int index)
    {

        skill_data_class _skillDB = MasterControl._Instance.GETSKILLDB();
        skills[index].skill_name = _skillDB.item[index].skill_name;
        skills[index]._skill_type = (Skill_Type)_skillDB.item[index].skill_type;
        skills[index].skill_icon = SkillData._instance.ICONS[_skillDB.item[index].skill_icon];

        skills[index].skill_duration = _skillDB.item[index].skill_duration;
        skills[index].skill_cool_time = _skillDB.item[index].skill_cool_time;
        skills[index]._effect_type = (Effect_Type)_skillDB.item[index].skill_effect;
        skills[index].skill_mana = _skillDB.item[index].skill_mana;
        skills[index].least_level = _skillDB.item[index].least_level;
        skills[index].max_level = _skillDB.item[index].max_level;
        skills[index].description = _skillDB.item[index].description;
        skills[index].sound = _skillDB.item[index].sound;
        skills[index].isLearned = false;

    }
    public Skill GetSkill(string skill_name)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skill_name.Equals(skills[i].skill_name))
            {
                return skills[i];
            }
        }
      return null;
    }


    public bool UpdateSkill(string skill_name, out string level, bool update = false)
    {
        level = "0/0";

        for(int i  =0; i < skills.Count; i++)
        {

            if (skill_name.Equals(skills[i].skill_name))
            {
                if (skills[i].isLearned == true)
                {
                    level = $"{skills[i].current_level}/{skills[i].max_level}";
                    if(update) skills[i].current_level++;
                    return true;
                }
                else if (update) // 이 경우는 배우지 않은 스킬일때
                {
                    skills[i].isLearned = true;
                    skills[i].current_level = 1;
                    level = $"{skills[i].current_level}/{skills[i].max_level}";
                    return true;
                }
                break;
            }
        }
        return false; // 스킬이 없음!
    }


    public bool GetSkillLevel(string skill_name, out int CurrentLevel, out int MaxLevel)
    {
        CurrentLevel = 0;
        MaxLevel = 0;
        for (int i = 0; i < skills.Count; i++)
        {
            if (skill_name.Equals(skills[i].skill_name))
            {
                CurrentLevel = skills[i].current_level;
                MaxLevel = skills[i].max_level;
                return true;
            }
        }
        return false; // 스킬이 없음!
    }
}

[Serializable]
public class Skill
{
    public string skill_name;
    public Sprite skill_icon;
    public float skill_duration;
    public int skill_cool_time;
    public int skill_mana;
    public int least_level;
    public int max_level;
    public int current_level;
    public string description;
    public Effect_Type _effect_type;
    public Skill_Type _skill_type;
    public bool isLearned;
    public int sound;
}