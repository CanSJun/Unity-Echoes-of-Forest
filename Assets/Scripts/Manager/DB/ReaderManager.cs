
using GoogleSheetsToUnity;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using JetBrains.Annotations;
using System.Data;
using System.Linq;

[Serializable]
public struct Bundle_Item_Data
{
    public int icon_number;
    public string item_name;
    public int use_sound;
    public int value;
    public int type; // 0 ü�� , 1 ���� , 2 SP
    public int gold;
    [TextArea] public string descrption;

    public Bundle_Item_Data(int icon, string item_name, int use_sound, int value, int type, string descrption, int gold)
    {
        this.icon_number = icon;
        this.item_name = item_name;
        this.use_sound = use_sound;
        this.value = value;
        this.type = type;
        this.descrption = descrption;
        this.gold = gold;
    }
}


[Serializable]
public struct Equip_Item_Data
{
    public int icon_number;
    public string item_name;
    public int use_sound;
    public int hp;
    public int mp;
    public int damage;
    public int other_sound;
    public int wear_image;
    public int def;
    public int type;
    public int gold;
    [TextArea] public string descrption;
    public float attack_speed;
    public int slot_type;
    public Equip_Item_Data(int icon, string item_name, int use_sound, int hp, int mp, int damage, int other_sound, int wear_image, int def, int type, string descrption, int gold, float attack_speed, int slot_type)
    {
        this.icon_number = icon;
        this.item_name = item_name;
        this.use_sound = use_sound;
        this.hp = hp;
        this.mp = mp;
        this.damage = damage;
        this.other_sound = other_sound;
        this.wear_image = wear_image;
        this.def = def;
        this.type = type;
        this.descrption = descrption;
        this.gold = gold;
        this.attack_speed = attack_speed;
        this.slot_type = slot_type;
    }
}



[Serializable]
public struct Skill_Data
{
    public string skill_name;
    public int skill_icon;
    public int skill_type;
    public float skill_duration;
    public int skill_cool_time;
    public int skill_effect;
    public int skill_mana;
    public int least_level;
    public int max_level;
    public int sound;
    [TextArea] public string description;
    

    public Skill_Data(string skill_name, int skill_icon, int skill_type, float skill_duration, int skill_cool_time, int skill_effect, int skill_mana, int least_level, int max_level, string description, int sound)
    {
        this.skill_name = skill_name;
        this.skill_icon = skill_icon;
        this.skill_type = skill_type;
        this.skill_duration = skill_duration;
        this.skill_cool_time = skill_cool_time;
        this.skill_effect = skill_effect;
        this.skill_mana = skill_mana;
        this.least_level = least_level;
        this.max_level = max_level;
        this.description = description;
        this.sound = sound;
    }
}
[CreateAssetMenu(fileName = "Bundle_item_Reader", menuName = "DB Reader/BundleItem", order = 0)]
public class Bundle_item_data_Class : DB_BaseData
{
    [SerializeField] public List<Bundle_Item_Data> item = new List<Bundle_Item_Data>();
    internal void UpdateStats(List<GSTU_Cell> list)
    {
        int icon_number = 0;
        string item_name = null;
        int use_sound = 0;
        int value = 0;
        int type = 0;
        string descriptioin = "";
        int gold = 0;
        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "������ ��ȣ": icon_number = int.Parse(list[i].value); break;
                case "�̸�": item_name = list[i].value; break;
                case "��� ���� ��ȣ": use_sound = int.Parse(list[i].value); break;
                case "�߰� ��ġ": value = int.Parse(list[i].value); break;
                case "Ÿ��": type = int.Parse(list[i].value); break;
                case "����": descriptioin = list[i].value; break;
                case "����": gold =  int.Parse(list[i].value); break;
            }

        }
        item.Add(new Bundle_Item_Data(icon_number, item_name, use_sound, value, type, descriptioin, gold));
    }
}


[CreateAssetMenu(fileName = "Equip_item_Reader", menuName = "DB Reader/EquipItem", order = 0)]
public class Equip_item_data_Class : DB_BaseData
{
    [SerializeField] public List<Equip_Item_Data> item = new List<Equip_Item_Data>();
    internal void UpdateStats(List<GSTU_Cell> list)
    {
        int icon_number = 0;
         string item_name = null;
         int use_sound = 0;
        int hp = 0;
        int mp = 0;
        int damage = 0;
         int other_sound = 0;
         int wear_image = 0;
        int def = 0;
        int type = 0;
        string description = "";
        int gold = 0;
        float attack_speed = 0f;
        int slot_type = -1;
        for (int i = 0; i < list.Count; i++)
        {
            
            switch (list[i].columnId)
            {
                case "������ ��ȣ": icon_number = int.Parse(list[i].value); break;
                case "�̸�": item_name = list[i].value; break;
                case "��� ���� ��ȣ": use_sound = int.Parse(list[i].value); break;
                case "�߰� ü��": hp = int.Parse(list[i].value); break;
                case "�߰� ����": mp = int.Parse(list[i].value); break;
                case "������": damage = int.Parse(list[i].value); break;
                case "���� ����": other_sound = int.Parse(list[i].value); break;
                case "���� �̹��� ��ȣ": wear_image = int.Parse(list[i].value); break;
                case "����": def = int.Parse(list[i].value); break;
                case "Ÿ��": type = int.Parse(list[i].value); break;
                case "����": description = list[i].value; break;
                case "����": gold = int.Parse(list[i].value); break;
                case "���ݼӵ�": attack_speed = float.Parse(list[i].value); break;
                case "���� Ÿ��": slot_type = int.Parse(list[i].value); break;
            }

        }
        item.Add(new Equip_Item_Data(icon_number, item_name, use_sound, hp, mp ,damage, other_sound, wear_image,def,type, description, gold, attack_speed, slot_type));
    }
}

[CreateAssetMenu(fileName = "skill_data", menuName = "DB Reader/skill_data", order = 0)]
public class skill_data_class : DB_BaseData
{
    [SerializeField] public List<Skill_Data> item = new List<Skill_Data>();
    internal void UpdateStats(List<GSTU_Cell> list)
    {
         string skill_name = null;
         int skill_icon = 0 ;
         int skill_type = 0;
        float skill_duration = 0;
         int skill_cool_time = 0;
         int skill_effect = 0;
         int skill_mana = 0;
         int least_level = 0;
         int max_level = 0;
         int sound = 0;
        string description = null;
        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "��ų �̸�": skill_name = list[i].value; break;
                case "������ ��ȣ": skill_icon = int.Parse(list[i].value); break;
                case "��ų Ÿ��": skill_type = int.Parse(list[i].value); break;
                case "���� �ð�": skill_duration = float.Parse(list[i].value); break;
                case "��Ÿ��": skill_cool_time = int.Parse(list[i].value); break;
                case "ȿ�� ��ȣ": skill_effect = int.Parse(list[i].value); break;
                case "����": skill_mana = int.Parse(list[i].value); break;
                case "�ʿ� ����": least_level = int.Parse(list[i].value); break;
                case "�ִ� ����": max_level = int.Parse(list[i].value); break;
                case "����": description = list[i].value; break;
                case "����": sound = int.Parse(list[i].value); break;


            }
        }
        item.Add(new Skill_Data(skill_name, skill_icon, skill_type, skill_duration, skill_cool_time, skill_effect, skill_mana, least_level, max_level, description, sound));
    }
}


#if UNITY_EDITOR
public abstract class BaseDataEditor<T> : Editor where T : DB_BaseData
{
    //��ӹ��� �ֵ鸸 ���� �� �� �ְ� Protected�� ����! private�� �ϸ� data�� ������ �Ұ���
    //public���� �ϸ� ��� Ŭ�������� data ������ ���������� ������ �ǵ�ġ ���� ���� ���� �� ���� 

    // abstract�� ���� ������ parameter�� Clear ������ ���� ����!
    protected T data;
    protected void Read(UnityAction<GstuSpreadSheet> callback, bool MergedCell = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(data.url, data.SheetName), callback, MergedCell);
    }

    protected abstract void Parameter(GstuSpreadSheet x);
    protected abstract void ClearData();

    protected void OnEnable() => data = (T)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("�ҷ�����"))
        {
            Read(Parameter);
            ClearData();
        }
    }
    
}
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(Equip_item_data_Class))]
public class EquipReader : BaseDataEditor<Equip_item_data_Class>
{
    protected override void Parameter(GstuSpreadSheet x)
    {

        int i = data.Start;
        while (x.rows.ContainsKey(i) && x.rows[i] != null)
        {
            data.UpdateStats(x.rows[i]);
            i++;
        }
        EditorUtility.SetDirty(target); // ������ ���� �̷��� ���ϸ� �� �̵��� ���� �����!
    }
    protected override void ClearData() => data.item.Clear();
}
#endif
#if UNITY_EDITOR
[CustomEditor(typeof(Bundle_item_data_Class))]
public class BundleReader : BaseDataEditor<Bundle_item_data_Class>
{
    protected override void Parameter(GstuSpreadSheet x)
    {
        int i = data.Start;
        while (x.rows.ContainsKey(i) && x.rows[i] != null)
        {
            data.UpdateStats(x.rows[i]);
            i++;
        }
        EditorUtility.SetDirty(target); // ������ ���� �̷��� ���ϸ� �� �̵��� ���� �����!
    }
    protected override void ClearData() => data.item.Clear();
}
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(skill_data_class))]
public class SkillReader : BaseDataEditor<skill_data_class>
{
    protected override void Parameter(GstuSpreadSheet x)
    {
        int i = data.Start;
        while (x.rows.ContainsKey(i) && x.rows[i] != null)
        {
            data.UpdateStats(x.rows[i]);
            i++;
        }
        EditorUtility.SetDirty(target); // ������ ���� �̷��� ���ϸ� �� �̵��� ���� �����!
    }
    protected override void ClearData() => data.item.Clear();

}
#endif