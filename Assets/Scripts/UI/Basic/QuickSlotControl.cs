using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class Save_Load_Quick
{
    public List<Save_Load_Quick_Slot> Quick;
}


public class QuickSlotControl : MonoBehaviour
{
    static QuickSlotControl _my;
    public static QuickSlotControl _instance { get { return _my; } }

    private void Awake()
    {
        _my = this;

    }
    [SerializeField] private List<QuickSlots> _QuickSlots = new List<QuickSlots>();

    [SerializeField] public HPMP_Control _MPBAR;



    public void Init()
    {
        GameData data = MasterControl._Instance.Data;
        if (data != null)
        {

                if (data.QUICKSLOTS != null)
                {
                QuickSlotControl slot = QuickSlotControl.LoadSlot(data.QUICKSLOTS);

                foreach (var t in slot._QuickSlots)
                {
                    Quick_Slots_manager._instance.CheckSlot(true);
                    if (string.IsNullOrEmpty(t._name)) continue;
                    switch (t.control_number)
                    {
                        case 1:
                            UserControl._instance._inventory.SetActive(true);
                            SlotControl[] slots = GameObject.FindObjectsOfType<SlotControl>();
                            foreach (var x in slots)
                            {
                                if (x._items.Count > 0)
                                {
                                    GameObject target = GameObject.Find(t.slot_number);
                                    if (target != null)
                                    {
                                        QuickSlotAdd(x, target.GetComponent<QuickSlots>());
                                    }
                                }
                            }
                            UserControl._instance._inventory.SetActive(false);

                            break;
                        case 2:


                            Skill skill = SkillControl._instance.GetSkill(t._name);


                            if (skill != null && skill.current_level > 0)
                            {
                                UserControl._instance._skillbook.SetActive(true);
                                GameObject skill_obj = GameObject.Find(t._name);
                                if (skill_obj != null)
                                {
                                    Skillslots skillSlot = skill_obj.GetComponent<Skillslots>();
                                    if (skillSlot != null)
                                    {
                                        GameObject target = GameObject.Find(t.slot_number);
                                        if (target != null)
                                        {
                                            QuickSlotAdd(skillSlot, target.GetComponent<QuickSlots>());
                                        }
                                    }
                                }
                                UserControl._instance._skillbook.SetActive(false);
                            }


                            break;
                    }
                }
                Quick_Slots_manager._instance.CheckSlot(false);
            }
        }
    }



    public float _SlotSize = 100f;
    public Skill_Manager _skillManager;
    public Dictionary<KeyCode, int> SlotsKey = new Dictionary<KeyCode, int>();


    public QuickSlots GetQuickSlot(int index) => _QuickSlots[index].GetComponent<QuickSlots>();


    void Start()
    {
        _skillManager = Skill_Manager._instance;

        SlotsKey.Add(KeyCode.Alpha1, 0);
        SlotsKey.Add(KeyCode.Alpha2, 1);
        SlotsKey.Add(KeyCode.Alpha3, 2);
        SlotsKey.Add(KeyCode.Alpha4, 3);
        SlotsKey.Add(KeyCode.Alpha5, 4);
        SlotsKey.Add(KeyCode.Alpha6, 5);
        SlotsKey.Add(KeyCode.Alpha7, 6);
        SlotsKey.Add(KeyCode.Alpha8, 7);
        SlotsKey.Add(KeyCode.Alpha9, 8);


        Invoke("Init", 0.1f);
    }



    public Save_Load_Quick SaveSlot()
    {

        return new Save_Load_Quick
        {
            Quick = _QuickSlots.Select(item => item.Save_Item()).ToList(),
        };
    }


    public static QuickSlotControl LoadSlot(Save_Load_Quick data)
    {
        var slot = new QuickSlotControl
        {
            _MPBAR = QuickSlotControl._instance._MPBAR,
            _SlotSize = QuickSlotControl._instance._SlotSize,
            _skillManager = QuickSlotControl._instance._skillManager,
        };
        slot.SlotsKey.Add(KeyCode.Alpha1, 0);
        slot.SlotsKey.Add(KeyCode.Alpha2, 1);
        slot.SlotsKey.Add(KeyCode.Alpha3, 2);
        slot.SlotsKey.Add(KeyCode.Alpha4, 3);
        slot.SlotsKey.Add(KeyCode.Alpha5, 4);
        slot.SlotsKey.Add(KeyCode.Alpha6, 5);
        slot.SlotsKey.Add(KeyCode.Alpha7, 6);
        slot.SlotsKey.Add(KeyCode.Alpha8, 7);
        slot.SlotsKey.Add(KeyCode.Alpha9, 8);
        foreach (var itemData in data.Quick)
        {


            slot._QuickSlots.Add(QuickSlots.Load_Item(itemData));
        }


        return slot;
    }

    bool Checking(QuickSlots skill)
    {
        if (UserControl._instance._nowMP < skill.mana) return false;
        _MPBAR.ChangeValue(skill.mana/ UserControl._instance._MP);

        UserControl._instance._nowMP -= skill.mana;
        return true;
    }
    void Using(int index)
    {


        if (_QuickSlots[index]._CurrentSlotType == QuickSlots.Type.None || UserControl._instance._areaskill || UserControl._instance._isinventory) return;
        if (_QuickSlots[index]._current_cool_time > 0) { LogSystem._instance.UpdateLog("사용을 하실 수 없습니다."); return; }
       
        UserControl my = UserControl._instance;
        switch (_QuickSlots[index]._CurrentSlotType)
        {
            case QuickSlots.Type.item:
                if (_QuickSlots[index].control is SlotControl slot)
                {
                    slot.QuickUsing();
                    if(slot._items.Count == 0)
                    {
                        DeleteSlot(_QuickSlots[index]);
                    }
                }
                break;
            case QuickSlots.Type.skill:
                if (my._skilltype > 0) return;
                if (Checking(_QuickSlots[index]) == false) { LogSystem._instance.UpdateLog("마력이 부족합니다."); return; }
                if (_QuickSlots[index]._skilltype == 4) _skillManager.Using_BuffSkill(_QuickSlots[index]); // type 4는 Buff Skill
                else if (_QuickSlots[index]._skilltype == 3) _skillManager.UsingAreaSkill(_QuickSlots[index]); // type 3은 Area Skill
                else _skillManager.UsingSkill(_QuickSlots[index]); // type 1은 즉시 시전 스킬, type2는 targeting skill
                break;
        }

    }

    void Update()
    {
        //내가 아무 키나 누르면 그 키를 찾아간다 나중에 키코드로 맵핑 해야할듯;;
        if (Input.anyKeyDown)
        {
            foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(k))
                {
                    if (SlotsKey.TryGetValue(k, out int index))
                    {
                        Using(index);
                        break;
                    }
                }
            }
        }
    }


    public QuickSlots Find_ClosedQuickSlot(Vector3 pos)
    {
        float Closed = 10000f;
        int index = -1;
        for (int i = 0; i < _QuickSlots.Count; i++)
        {
            Vector2 NowSlotPos = _QuickSlots[i].transform.GetChild(1).position;
            float diff = Vector2.Distance(NowSlotPos, pos);
            if (diff < Closed) { Closed = diff; index = i; }
        }
        if (Closed > _SlotSize) return null;
        return _QuickSlots[index].GetComponent<QuickSlots>();

    }

    void DeleteSlot(QuickSlots slot)
    {
        slot._name = null;
        slot._CurrentSlotType = QuickSlots.Type.None;
        slot.mana = 0;
        slot._cool_time = 0;
        slot._duration = 0;
        slot._skilltype = 0;
        slot._icon.sprite = null;
        slot._icon.gameObject.SetActive(false);
        slot.SetUsing(false);
        slot.control = false;
        
    }
    int CheckQuickSlot(Skill skill)
    {
        for (int i = 0; i < _QuickSlots.Count; i++)
        {
            if (_QuickSlots[i].GetUsing() == true && _QuickSlots[i]._name.Equals(skill.skill_name))
            {
                if (_QuickSlots[i]._current_cool_time != 0)  return 0; 
                DeleteSlot(_QuickSlots[i]);
                break;
            }
        }
        return 1;
    }

    public void QuickSlotAdd(object control, QuickSlots QuickSlot)
    {
        
        if (control is Skillslots skill_slot){
           
            Skill skill = SkillControl._instance.GetSkill(skill_slot.name);
            if(skill == null)
            {
                Debug.Log("QuickSlotControl.cs - QUickSlotAdd Error - skill is null");
                return;
            }
            
            int check = CheckQuickSlot(skill); //Find 동일한 것 찾아서 지워주기\
            if (check == 0) return; // int형으로 받은 이유는 내가 만약에 스킬을 사용하고 있으면 새롭게 추가하지 못하게 하기위해서
            QuickSlot._name = skill.skill_name;
            QuickSlot._CurrentSlotType = QuickSlots.Type.skill;
            QuickSlot.mana = skill.skill_mana;
            QuickSlot._cool_time = skill.skill_cool_time;
            QuickSlot._duration = skill.skill_duration;
            QuickSlot._skilltype = (float)(int)skill._skill_type;
            QuickSlot._icon.sprite = skill.skill_icon;
            QuickSlot.control = control;
            QuickSlot.SetUsing(true);
            QuickSlot._icon.gameObject.SetActive(true);
            QuickSlot.control_number = 2;
            QuickSlot.slot_number = QuickSlot.transform.name;

        }
        else if(control is SlotControl item_slot){
            if(item_slot._items.Peek()._isStackable == false) return;
            QuickSlot._name = item_slot._items.Peek()._name;
            QuickSlot._CurrentSlotType = QuickSlots.Type.item;
            QuickSlot.mana = item_slot._items.Peek()._value;
            QuickSlot._cool_time = 0;
            QuickSlot._duration = 0;
            QuickSlot._skilltype = 0;
            QuickSlot._icon.sprite = item_slot._items.Peek()._Icon;
            QuickSlot.control = control;
            QuickSlot.SetUsing(true);
            QuickSlot._icon.gameObject.SetActive(true);
            QuickSlot.control_number = 1;
            QuickSlot.slot_number = QuickSlot.transform.name;
        }
        else
        {
            Debug.Log("QuickSlotControl.cs - QuickSlotAdd Error - not found object");
        }
    }

    public void Swap(QuickSlots Current, QuickSlots Target)
    {
        if (Target.GetUsing() == false)
        {
            QuickSlotAdd(Current.control, Target);
            DeleteSlot(Current);
        }
        else
        {
            object tempControl = Current.control;
            string tempName = Current._name;
            float tempSkillType = Current._skilltype;
            float tempMana = Current.mana;
            float tempCoolTime = Current._cool_time;
            float tempDuration = Current._duration;
            string tempnumber = Current.slot_number;
            int tempnumber2 = Current.control_number;
            Sprite tempIcon = Current._icon.sprite;
            QuickSlots.Type tempType = Current._CurrentSlotType;

            Current._name = Target._name;
            Current._skilltype = Target._skilltype;
            Current.mana = Target.mana;
            Current._CurrentSlotType = Target._CurrentSlotType;
            Current._cool_time = Target._cool_time;
            Current._duration = Target._duration;
            Current._icon.sprite = Target._icon.sprite;
            Current.control = Target.control;
            Current.slot_number = Target.slot_number;
            Current.control_number = Target.control_number;

            Target._name = tempName;
            Target._skilltype = tempSkillType;
            Target.mana = tempMana;
            Target._CurrentSlotType = tempType;
            Target._cool_time = tempCoolTime;
            Target._duration = tempDuration;
            Target._icon.sprite = tempIcon;
            Target.control = tempControl;
            Target.slot_number = tempnumber;
            Target.control_number = tempnumber2;
        }
    }
}
