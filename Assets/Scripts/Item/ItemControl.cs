using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum item_type
{
    HP,
    MP,
    SP,
    One_Hand_Axe,
    Sword,
    Two_Hand_Axe,
    Bow,
    Armer,
    Shield,
    Helmet,
    Gloves,
    Shoes,
    Gold,
    ETC
}

[Serializable]
public class Save_Load_Item
{
    public item_type _item_type;
    public string _name;
    public bool _isStackable = false;
    public Sprite _Icon;
    public GameObject _WearImage;
    public int _value;
    public int _prefebnum;
    public int _gold;
    public string _description;
    public int _UseSound;
    public int damage;
    public int def;
    public int hp;
    public int mp;
    public float attack_speed;
    public int _EquipSound;
    public int icon_number;
    public int slot_type;
    public int type_number;

}
public class ItemControl : MonoBehaviour
{


    [Header("기본 정보")]
    public item_type _item_type;
    public string _name;
    public bool _isStackable = false;
    public Sprite _Icon;
    public GameObject _WearImage;
    public int _value;
    public int _prefebnum;
    public int _gold;

    
    [TextArea]public string _description;

 
    [Header("음향 정보")]
    public AudioClip _UseSound;



    [Header("장착 아이템 정보")]
    public int damage;
    public int def;
    public int hp;
    public int mp;
    public float attack_speed;
    public AudioClip _EquipSound;

    [Header("기타 정보")]
    public int use_sound_number;
    public int equip_sound_number;
    public int icon_number;
    public int slot_number;
    public int type_number;

    public Save_Load_Item Save_Item()
    {
        
        return new Save_Load_Item
        {
            _item_type = (item_type)this.type_number,
            _name = this._name,
            _isStackable = this._isStackable,
            _Icon = this._Icon,
            _WearImage = this._WearImage,
            _value = this._value,
            _prefebnum = this._prefebnum,
            _gold = this._gold,
            _description = this._description,
            _UseSound = this.use_sound_number,
            damage = this.damage,
            def = this.def,
            hp = this.hp,
            mp = this.mp,
            attack_speed = this.attack_speed,
            _EquipSound = this.equip_sound_number,
            icon_number = this.icon_number,
            slot_type = this.slot_number,
            type_number = this.type_number
        };
    }


    public static ItemControl Load_Item(Save_Load_Item data)
    {
        return new ItemControl
        {
            _item_type = (item_type)data.type_number,
            _name = data._name,
            _isStackable = data._isStackable,
            _Icon = ItemData._instance.ICONS[data.icon_number],
            _WearImage = data._WearImage,
            _value = data._value,
            _prefebnum = data._prefebnum,
            _gold = data._gold,
            _description = data._description,
            _UseSound = ItemData._instance.AUDIO[data._UseSound],
            use_sound_number = data._UseSound,
            damage = data.damage,
            def = data.def,
            hp = data.hp,
            mp = data.mp,
            attack_speed = data.attack_speed,
            _EquipSound = ItemData._instance.EQUIPAUDIO[data._EquipSound],
            equip_sound_number = data._EquipSound,
            icon_number = data.icon_number,
            slot_number = data.slot_type,
            type_number = data.type_number

        };
    }
    public void CopyValue(ItemControl source)
    {
        _item_type = source._item_type;
        _name = source._name;
        _isStackable = source._isStackable;
        _Icon = source._Icon;
        _WearImage = source._WearImage;
        _value = source._value;
        _prefebnum = source._prefebnum;
        _gold = source._gold;
        _description = source._description;
        _UseSound = source._UseSound;
        damage = source.damage;
        def = source.def;
        hp = source.hp;
        mp = source.mp;
        attack_speed = source.attack_speed;
        _EquipSound = source._EquipSound;
        use_sound_number = source.use_sound_number;
        equip_sound_number = source.equip_sound_number;
        icon_number = source.icon_number;
        slot_number = source.slot_number;
        type_number = source.type_number;
    }
    public void Initialize(string itemName)
    {
        // ItemDB에서 아이템 데이터를 가져와 초기화
        var itemData = ItemDB._instance.GetItem(itemName);

        if (itemData != null)
        {
            _item_type = itemData._item_type;
            _Icon = itemData._Icon;
            _isStackable = itemData._isStackable;
            _value = itemData._value;
            _gold = itemData._gold;
            _description = itemData._description;
            _UseSound = itemData._UseSound;

            
            // 장착 아이템일 경우
            if (itemData._isStackable == false)
            {
                damage = itemData.damage;
                def = itemData.def;
                hp = itemData.hp;
                mp = itemData.mp;
                _WearImage = itemData._WearImage;
                _EquipSound = itemData._EquipSound;
            }
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' 이 DB에 없습니다.");
        }
    }

    public void Using()
    {
        UserControl player = UserControl._instance;

        switch (_item_type)
        {
            case item_type.HP:
                player._nowHP += _value;
                player._HPBAR.GetComponent<HPMP_Control>().ChangeValueUp(_value);
            break;
            case item_type.MP:
                player._nowMP += _value;
                player._MPBAR.GetComponent<HPMP_Control>().ChangeValueUp(_value);
                break;
            case item_type.SP:
                player._SP += _value;
                break;
            case item_type.Gold:

            break;
            case item_type.ETC:

            break;
            default: // 나머지는 전부 다 장착 아이템
                EquipControl._instance.Equip_on(this);
                break;
        }
    }


    void Start()
    {
        if (!string.IsNullOrEmpty(_name))
        {
            Initialize(_name);
        }
    }



}
