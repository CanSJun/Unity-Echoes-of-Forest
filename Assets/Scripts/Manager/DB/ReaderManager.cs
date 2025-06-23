
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
    public int type; // 0 체력 , 1 마나 , 2 SP
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
                case "아이콘 번호": icon_number = int.Parse(list[i].value); break;
                case "이름": item_name = list[i].value; break;
                case "사용 음향 번호": use_sound = int.Parse(list[i].value); break;
                case "추가 수치": value = int.Parse(list[i].value); break;
                case "타입": type = int.Parse(list[i].value); break;
                case "설명": descriptioin = list[i].value; break;
                case "가격": gold =  int.Parse(list[i].value); break;
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
                case "아이콘 번호": icon_number = int.Parse(list[i].value); break;
                case "이름": item_name = list[i].value; break;
                case "사용 음향 번호": use_sound = int.Parse(list[i].value); break;
                case "추가 체력": hp = int.Parse(list[i].value); break;
                case "추가 마력": mp = int.Parse(list[i].value); break;
                case "데미지": damage = int.Parse(list[i].value); break;
                case "음향 정보": other_sound = int.Parse(list[i].value); break;
                case "착용 이미지 번호": wear_image = int.Parse(list[i].value); break;
                case "방어력": def = int.Parse(list[i].value); break;
                case "타입": type = int.Parse(list[i].value); break;
                case "설명": description = list[i].value; break;
                case "가격": gold = int.Parse(list[i].value); break;
                case "공격속도": attack_speed = float.Parse(list[i].value); break;
                case "슬롯 타입": slot_type = int.Parse(list[i].value); break;
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
                case "스킬 이름": skill_name = list[i].value; break;
                case "아이콘 번호": skill_icon = int.Parse(list[i].value); break;
                case "스킬 타입": skill_type = int.Parse(list[i].value); break;
                case "지속 시간": skill_duration = float.Parse(list[i].value); break;
                case "쿨타임": skill_cool_time = int.Parse(list[i].value); break;
                case "효과 번호": skill_effect = int.Parse(list[i].value); break;
                case "마나": skill_mana = int.Parse(list[i].value); break;
                case "필요 레벨": least_level = int.Parse(list[i].value); break;
                case "최대 레벨": max_level = int.Parse(list[i].value); break;
                case "설명": description = list[i].value; break;
                case "사운드": sound = int.Parse(list[i].value); break;


            }
        }
        item.Add(new Skill_Data(skill_name, skill_icon, skill_type, skill_duration, skill_cool_time, skill_effect, skill_mana, least_level, max_level, description, sound));
    }
}


#if UNITY_EDITOR
public abstract class BaseDataEditor<T> : Editor where T : DB_BaseData
{
    //상속받은 애들만 접근 할 수 있게 Protected로 진행! private를 하면 data에 접근이 불가능
    //public으로 하면 모든 클래스에서 data 접근이 가능해지기 때문에 의도치 않은 오류 생길 수 있음 

    // abstract를 해준 이유는 parameter와 Clear 구현이 없기 때문!
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
        if (GUILayout.Button("불러오기"))
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
        EditorUtility.SetDirty(target); // 데이터 저장 이렇게 안하면 씬 이동때 마다 사라짐!
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
        EditorUtility.SetDirty(target); // 데이터 저장 이렇게 안하면 씬 이동때 마다 사라짐!
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
        EditorUtility.SetDirty(target); // 데이터 저장 이렇게 안하면 씬 이동때 마다 사라짐!
    }
    protected override void ClearData() => data.item.Clear();

}
#endif