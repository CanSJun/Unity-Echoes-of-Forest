using Google.GData.Spreadsheets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EquipControl;


public struct EquipSlot
{
    public _slot_type Type;
    public item_type _item_type;
    public string _name;
    public bool _isStackable;
    public Sprite _Icon;
    public GameObject _WearImage;
    public int _value;
    public int _prefebnum;
    public AudioClip _UseSound;
    public int _damage;
    public int _def;
    public int _hp;
    public int _mp;
    public AudioClip _EquipSound;


    public EquipSlot(_slot_type type, item_type item_type, string name, bool Stack, Sprite Icon, GameObject WearImage, int value, int prefeb, AudioClip Use, int damage, int def, int hp, int mp, AudioClip Equip)
    {
        Type = type;
        _item_type = item_type;
        _name = name;
        _isStackable = Stack;
        _Icon = Icon;
        _WearImage = WearImage;
        _value = value;
        _prefebnum = prefeb;
        _UseSound = Use;
        _damage = damage;
        _def = def;
        _hp = hp;
        _mp = mp;
        _EquipSound = Equip;




    }
}

[Serializable]
public class Save_Load_Equip
{
    public int Helmet;
    public int Shoes;
    public int Armer;
    public int Weapon;
    public int Shield;
    public bool _helemt_using = false;
    public bool _shoes_using = false;
    public bool _armer_using = false;
    public bool _weapon_using = false;
    public bool _shield_using = false;
    public Save_Load_Item shelmet;
    public Save_Load_Item sshoes;
    public Save_Load_Item sarmer;
    public Save_Load_Item sweapon;
    public Save_Load_Item sshield;
}

public class EquipControl : MonoBehaviour
{
    static EquipControl _my;
    public static EquipControl _instance { get { return _my; } }
    private void Awake()
    {
        Invoke("Init", 0.1f);
        _my = this;
    }
    [Header("SLOT 정보")]
    public Image Helmet;
    public Image Shoes;
    public Image Armer;
    public Image Weapon;
    public Image Shield;

    public Information_Manager Helmet_;
    public Information_Manager Shoes_;
    public Information_Manager Armer_;
    public Information_Manager Weapon_;
    public Information_Manager Shield_;
    public Sprite _default; // 빈칸 아이콘
    private int _damage;
    private int _level;
    private int _ac;
    private int _sp;
    [Header("텍스트")]
    [SerializeField] TextMeshProUGUI _damage_text;
    [SerializeField] TextMeshProUGUI _ac_text;
    [SerializeField] TextMeshProUGUI _level_text;
    [SerializeField] TextMeshProUGUI _sp_text;

    public bool _helemt_using = false;
    public bool _shoes_using = false;
    public bool _armer_using = false;
    public bool _weapon_using = false;
    public bool _shield_using = false;

    private UserControl Player;

    public ItemControl shelmet;
    public ItemControl sshoes;
    public ItemControl sarmer;
    public ItemControl sweapon;
    public ItemControl sshield;



    public enum _slot_type
    {
        Helmet,
        Shoes,
        Armer,
        Weapon,
        Shield
    }


    private List<EquipSlot> equipSlots = new List<EquipSlot> {

        new EquipSlot(_slot_type.Helmet, item_type.Helmet, null, false, null, null, 0, 0, null, 0, 0, 0, 0, null),
        new EquipSlot(_slot_type.Weapon, item_type.Sword, null, false, null, null, 0, 0, null, 0, 0, 0, 0, null),
        new EquipSlot(_slot_type.Shoes, item_type.Shoes, null, false, null, null, 0, 0, null, 0, 0, 0, 0, null),
        new EquipSlot(_slot_type.Shield, item_type.Shield, null, false, null, null, 0, 0, null, 0, 0, 0, 0, null),
        new EquipSlot(_slot_type.Armer, item_type.Armer, null, false, null, null, 0, 0, null, 0, 0, 0, 0, null)
    };


    void updatetext()
    {
        _damage = UserControl._instance._damage;
        _damage_text.text = "Damage : " + _damage;
        _level = UserControl._instance._level;
        _level_text.text = "Level : " + _level;
        _ac = UserControl._instance._def;
        _ac_text.text = "AC : " + _ac;
        _sp = UserControl._instance._SP;
        _sp_text.text = "SP : " + _sp;
    }
    public void UpdatgeInformation(_slot_type type, ItemControl item)
    {
        updatetext();

        switch (type)
        {
            case _slot_type.Helmet:
                if (_helemt_using) CheckInit(type);

                Helmet_._isUsing = true;
                Helmet_._name = item._name;
                Helmet_._description = item._description;
                
                _helemt_using = true;
                Helmet.sprite = item._Icon;
                shelmet = item;
                break;
            case _slot_type.Shoes:
                if (_shoes_using) CheckInit(type);
                _shoes_using = true;


                Shoes.sprite = item._Icon;
                Shoes_._isUsing = true;
                Shoes_._name = item._name;
                Shoes_._description = item._description;

                sshoes = item;
                break;
            case _slot_type.Armer:
                if (_armer_using) CheckInit(type);
                _armer_using = true;
                Armer.sprite = item._Icon;
                Armer_._isUsing = true;

                Armer_._name = item._name;
                Armer_._description = item._description;
                sarmer = item;
                break;

            case _slot_type.Weapon:
                if (_weapon_using) CheckInit(type);
                _weapon_using = true;
                Weapon.sprite = item._Icon;
                Weapon_._isUsing = true;
                Weapon_._name = item._name;
                Weapon_._description = item._description;
                sweapon = item;
                break;
            case _slot_type.Shield:
                if (_shield_using) CheckInit(type);
                _shield_using = true;
                Shield.sprite = item._Icon;
                Shield_._isUsing = true;
                Shield_._name = item._name;
                Shield_._description = item._description;

                sshield = item;
                break;
        }
        UpdateEquipSlot(type, item._item_type, item._name, item._isStackable, item._Icon, item._WearImage, item._value, item._prefebnum, item._UseSound, item.damage, item.def, item.hp, item.mp, item._EquipSound);
    }


    public Save_Load_Equip SaveEquip()
    {


        return new Save_Load_Equip
        {
        _helemt_using = this._helemt_using,
         _shoes_using = this._shoes_using,
         _armer_using = this._armer_using,
         _weapon_using = this._weapon_using,
         _shield_using = this._shield_using,
        shelmet = shelmet == null ? null : this.shelmet.Save_Item(),
            sshoes = sshoes == null ? null : this.sshoes.Save_Item(),
            sarmer = sarmer == null ? null : this.sarmer.Save_Item(),
            sweapon = sweapon == null ? null : this.sweapon.Save_Item(),
            sshield = sshield == null ? null : this.sshield.Save_Item()
        };
    }



    public void Init()
    {
        GameData data = MasterControl._Instance.Data;
        if (data != null)
        {
            if(data.WEARITEM != null)
            {
                _default = ItemData._instance.DEFAULT;


                if (data.WEARITEM.shelmet != null) Equip_on(ItemControl.Load_Item(data.WEARITEM.shelmet));
                if (data.WEARITEM.sshield != null) Equip_on(ItemControl.Load_Item(data.WEARITEM.sshield));
                if (data.WEARITEM.sarmer != null) Equip_on(ItemControl.Load_Item(data.WEARITEM.sarmer));
                if (data.WEARITEM.sshoes != null) Equip_on(ItemControl.Load_Item(data.WEARITEM.sshoes));
                if (data.WEARITEM.sweapon != null) Equip_on(ItemControl.Load_Item(data.WEARITEM.sweapon));
            }
        }
    }


    void Update_UserStat(ItemControl item)
    {
        if (item._isStackable) return;

            
            Player._damage += item.damage;
            Player._HP += item.hp;
            Player._MP += item.mp;
            Player._def += item.def;
    }
    void UNUpdate_UserStat(EquipSlot equip)
    {
        Player._damage -= equip._damage;
        Player._HP -= equip._hp;
        Player._MP -= equip._mp;
        Player._def -= equip._def;

        updatetext();
    }


    public void Equip_on(ItemControl item)
    {
        if (item._isStackable) return;
        Player = UserControl._instance;
        Update_UserStat(item);
        switch (item._item_type)
        {

            case item_type.Helmet:
                UpdatgeInformation(_slot_type.Helmet, item);
                break;

            case item_type.Shoes:
                UpdatgeInformation(_slot_type.Shoes, item);
                break;
            case item_type.Armer:
                UpdatgeInformation(_slot_type.Armer, item);
                break;
            case item_type.Shield:
                UpdatgeInformation(_slot_type.Shield, item);
                Instantiate(item._WearImage, Player._LeftHand);
                Player._nowShield = item;
                Player._isShield_Using = true;
                break;

            case item_type.One_Hand_Axe:
                UpdatgeInformation(_slot_type.Weapon, item);
                Instantiate(item._WearImage, Player._RightHand.transform);
                Player._CurrentWeapon =  UserControl.Weapon.One_Hand_Axe;
                
                Player._attackSpeed = item.attack_speed;
                Player.AttackSound = item._UseSound;
                break;
            case item_type.Sword:

                UpdatgeInformation(_slot_type.Weapon, item);
                Instantiate(item._WearImage, Player._HandsArea.transform);
                Player._CurrentWeapon = UserControl.Weapon.Sword;
                
                Player._attackSpeed = item.attack_speed;
                Player.AttackSound = item._UseSound;
                break;
        }

        
        SoundManager._instance.PrivatePlaySound(item._EquipSound, Player.transform);
    }



    private void UpdateEquipSlot(_slot_type type, item_type item_type, string name, bool Stack, Sprite Icon, GameObject WearImage, int value, int prefeb, AudioClip Use, int damage, int def, int hp, int mp, AudioClip Equip)
    {
        int index = equipSlots.FindIndex(slot => slot.Type == type);
        if(index >= 0)
        {
            equipSlots[index] = new EquipSlot(type, item_type, name, Stack, Icon, WearImage, value, prefeb, Use, damage, def, hp, mp, Equip);
        }
    }



    void CheckInit(_slot_type type)
    {
        if(InventorySystem.instance._MaxSlots <= 0) { LogSystem._instance.UpdateLog("인벤토리가 부족합니다."); return; }


        int index = equipSlots.FindIndex(slot => slot.Type == type);
        if(index >= 0)
        {
            UNUpdate_UserStat(equipSlots[index]);

            ItemControl deleteItem = ItemDB._instance.GetItem(equipSlots[index]._name);
            InventorySystem.instance.Item_add(deleteItem);
        }
        switch (type)
        {
            case _slot_type.Weapon:
                _weapon_using = false;
                Weapon.sprite = _default;
                Weapon_._isUsing = false;
                Player._CurrentWeapon = UserControl.Weapon.Hands;
                if(Player._RightHand.transform.childCount > 0) Destroy(Player._RightHand.transform.GetChild(0).gameObject);
                else if(Player._HandsArea.transform.childCount > 0) Destroy(Player._HandsArea.transform.GetChild(0).gameObject);

                sweapon = null;
                break;
            case _slot_type.Helmet:
                _helemt_using = false;
                Helmet.sprite = _default;
                Helmet_._isUsing = false;
                shelmet = null;
                break;
            case _slot_type.Armer:
                _armer_using = false;
                Armer.sprite = _default;
                Armer_._isUsing = false;
                sarmer = null;
                break;
            case _slot_type.Shield:
                _shield_using = false;
                Shield.sprite = _default;
                Shield_._isUsing = false;
                Player._nowShield = null;
                Destroy(Player._LeftHand.transform.GetChild(0).gameObject);
                Player._isShield_Using = false;
                sshield = null;
                break;
            case _slot_type.Shoes:
                _shoes_using = false;
                Shoes.sprite = _default;
                Shoes_._isUsing = false;
                sshoes = null;
                break;

        }
    }



    void Onslot(_slot_type type, BaseEventData eventData, bool b)
    {
        if (!b || (eventData as PointerEventData).button != PointerEventData.InputButton.Right) return;
        CheckInit(type);
    }
    public void OnHelmet(BaseEventData eventData) => Onslot(_slot_type.Helmet, eventData, _helemt_using);
    public void OnArmer(BaseEventData eventData) => Onslot(_slot_type.Armer, eventData, _armer_using);
    public void OnShield(BaseEventData eventData) => Onslot(_slot_type.Shield, eventData, _shield_using);
    public void OnShoes(BaseEventData eventData) => Onslot(_slot_type.Shoes, eventData, _shoes_using);
    public void OnWeapon(BaseEventData eventData) => Onslot(_slot_type.Weapon, eventData, _weapon_using);

}
