
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


public class GameData
{
    //�⺻����
    public float LocationX;
    public float LocationY;
    public float LocationZ;
    public float RotationX;
    public float RotationY;
    public float RotationZ;
    public float RotationW;
    public float HP;
    public float MAXHP;
    public float MP;
    public float MAXMP;
    public float EXP;
    public int DEF;
    public int MONEY;
    public int SP;
    public int Level;
    public int[] QUESTS;

    //�κ��丮
    public Save_Load_Slot[] SLOTS;
    //��ų
    public int[] SKILL;
    // 0 : ���ڵ�
    // 1 : ���� ���
    // 2 : ��ȿ
    // 3 : ȸ������
    // 4 : ����Ʈ��

    //���� ������
    public Save_Load_Equip WEARITEM;

    // ������
    public Save_Load_Quick QUICKSLOTS;

    // ���� ���� ����
    public int BEAR;
    public int BOSS;


    public GameData(Vector3 Location, Quaternion Rotation,float MaxHP, float MaxMP,int level, float HP, float MP, float EXP, int DEF, int MONEY, int SP, int[] Quest, Save_Load_Slot[] slots, int[] skills, Save_Load_Equip Wear, Save_Load_Quick Quick, int Bear, int Boss)
    {
        this.LocationX = Location.x;
        this.LocationY = Location.y;
        this.LocationZ = Location.z;

        this.RotationX = Rotation.x;
        this.RotationY = Rotation.y;
        this.RotationZ = Rotation.z;
        this.RotationW = Rotation.w;
        this.Level = level;
        this.HP = HP;
        this.MAXHP = MaxHP;
        this.MP = MP;
        this.MAXMP = MaxMP;
        this.EXP = EXP;
        this.DEF = DEF;
        this.MONEY = MONEY;
        this.SP = SP;
        this.QUESTS = Quest;

        this.SLOTS = slots;



        this.SKILL = skills;
        this.WEARITEM = Wear;
        this.QUICKSLOTS = Quick;
        this.BEAR = Bear;
        this.BOSS = Boss;


    }
    public override string ToString()
    {
        string quests = QUESTS != null ? string.Join(", ", QUESTS) : "None";

        string slots = "None";
        if (SLOTS != null)
        {
            List<string> slotStrings = new List<string>();
            foreach (var slot in SLOTS)
            {
                slotStrings.Add(slot != null ? slot.ToString() : "Empty");
            }
            slots = string.Join(", ", slotStrings);
        }

        string skills = SKILL != null ? string.Join(", ", SKILL) : "None";

        string wearItems = "None";

        string wearItem = WEARITEM != null ? string.Join(", ", WEARITEM) : "None";


        string quickSlots = QUICKSLOTS != null ? string.Join(", ", QUICKSLOTS) : "None";

        return $"Location: ({LocationX}, {LocationY}, {LocationZ})\n" +
               $"Rotation: ({RotationX}, {RotationY}, {RotationZ}, {RotationW})\n" +
               $"HP: {HP}, MP: {MP}, EXP: {EXP}, DEF: {DEF}, MONEY: {MONEY}, SP: {SP}\n" +
               $"Quests: [{quests}]\n" +
               $"Slots: [{slots}]\n" +
               $"Skills: [{skills}]\n" +
               $"Wear Items: [{wearItems}]\n" +
               $"Quick Slots: [{quickSlots}]\n" +
               $"Bear: {BEAR}, Boss: {BOSS}";
    }



}
public class Save_Manager : MonoBehaviour
{


    GameData Setting_Data()
    {
        GameData NewData = new GameData(new Vector3(0,0,0), new Quaternion(0,0,0,0),0,0, 0, 0, 0, 0, 0, 0, 0, new int[3], new Save_Load_Slot[36], new int[5], new Save_Load_Equip(), new Save_Load_Quick(), 0, 0);
        UserControl Player = UserControl._instance;
        NewData.LocationX = Player.transform.position.x;
        NewData.LocationY = Player.transform.position.y;
        NewData.LocationZ = Player.transform.position.z;
        NewData.RotationX = Player.transform.rotation.x;
        NewData.RotationY = Player.transform.rotation.y;
        NewData.RotationZ = Player.transform.rotation.z;
        NewData.RotationW = Player.transform.rotation.w;
        NewData.MAXHP = Player._HP;
        NewData.MAXMP = Player._MP;
        NewData.HP = Player._nowHP;
        NewData.MP = Player._nowMP;
        NewData.EXP = Player._EXP;
        NewData.Level = Player._level;
        NewData.DEF = Player._def;
        NewData.MONEY = Player._money;
        NewData.SP = Player._SP;
        NewData.QUESTS = new int[3];
        NewData.SLOTS = new Save_Load_Slot[36];
        for (int i = 0; i < Player.Quest.Length; i++)  NewData.QUESTS[i] = Player.Quest[i];
        for (int i = 0; i < 36; i++) {
            SlotControl slot = InventorySystem.instance.GetSlot(i);
            if (slot  == null) continue;
            NewData.SLOTS[i] = slot.SaveSlot();

        }


        NewData.SKILL = new int[5];
        int current_level = 0;
        int max_level = 0;
        if (SkillControl._instance.GetSkillLevel("���ڵ�", out current_level, out max_level) == true) NewData.SKILL[0] = current_level;
        if (SkillControl._instance.GetSkillLevel("���� ���", out current_level, out max_level) == true) NewData.SKILL[1] = current_level;
        if (SkillControl._instance.GetSkillLevel("��ȿ", out current_level, out max_level) == true) NewData.SKILL[2] = current_level;
        if (SkillControl._instance.GetSkillLevel("ȸ������", out current_level, out max_level) == true) NewData.SKILL[3] = current_level;
        if (SkillControl._instance.GetSkillLevel("����Ʈ��", out current_level, out max_level) == true) NewData.SKILL[4] = current_level;

        NewData.WEARITEM = EquipControl._instance.SaveEquip();

        NewData.QUICKSLOTS = QuickSlotControl._instance.SaveSlot();

        NewData.BEAR = GameObject.FindGameObjectsWithTag("Monster").Length;
        NewData.BOSS = GameObject.FindGameObjectsWithTag("Boss").Length;

        return NewData;
    }

    public void SaveGame()
    {
        GameData data = Setting_Data();
        string json = JsonUtility.ToJson(data);
        
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        string EncodedJson = System.Convert.ToBase64String(bytes);
        File.WriteAllText(Application.dataPath + "/Save.json", EncodedJson);
    }
}
