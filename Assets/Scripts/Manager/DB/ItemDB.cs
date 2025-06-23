using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    static ItemDB _my;
    public static ItemDB _instance { get { return _my; } }
    [Header("Prefeb")]
    [SerializeField] private Bundle_item_data_Class _BundleData;
    [SerializeField] private Equip_item_data_Class _EquipData;
    void Awake() {

        _my = this; }

    





    private Dictionary<string, ItemControl> _dictionary = new Dictionary<string, ItemControl>();


    public Bundle_item_data_Class Get_Bundle_DB() => _BundleData;
    public Equip_item_data_Class Get_Equip_DB() => _EquipData;



    void Start()
    {

        Setting();
    }


    public ItemControl GetItem(string item_name)
    {
        if (_dictionary.TryGetValue(item_name, out ItemControl item)) return item;
        else return null;
    }
    
    void Setting()
    {

        foreach (var bundleItem in _BundleData.item)
        {
           
            ItemControl item = gameObject.AddComponent<ItemControl>();
            item._Icon = ItemData._instance.ICONS[bundleItem.icon_number];
            item._item_type = (item_type)bundleItem.type;
            item._prefebnum = bundleItem.icon_number;
            item._name = bundleItem.item_name;
            if (ItemData._instance.AUDIO.Count > 0) { 
                item._UseSound = ItemData._instance.AUDIO[bundleItem.use_sound];
                item.use_sound_number = bundleItem.use_sound;
            }
            item._value = bundleItem.value;
            item._description = bundleItem.descrption;
            item._gold = bundleItem.gold;
            item._isStackable = true;
            item.icon_number = bundleItem.icon_number;
           
            _dictionary.Add(item._name, item);
        }

        foreach (var equipItem in _EquipData.item)
        {

            ItemControl item = gameObject.AddComponent<ItemControl>();
            item._Icon = ItemData._instance.ICONS[equipItem.icon_number];
            item._prefebnum = equipItem.icon_number;
            item._name = equipItem.item_name;
            if (ItemData._instance.AUDIO.Count > 0) { 
                item._UseSound = ItemData._instance.AUDIO[equipItem.use_sound];
                item.use_sound_number = equipItem.use_sound;
            }
            item._WearImage = ItemData._instance.Wear[equipItem.wear_image];
            item._isStackable = false;
            item.damage = equipItem.damage;
            item.hp = equipItem.hp;
            item.mp = equipItem.mp;
            if (ItemData._instance.EQUIPAUDIO.Count > 0) { 
                item._EquipSound = ItemData._instance.EQUIPAUDIO[equipItem.other_sound];
                item.equip_sound_number = equipItem.other_sound;
            }
            item.def = equipItem.def;
            item._description = equipItem.descrption;
            item._gold = equipItem.gold;
            item.attack_speed = equipItem.attack_speed;
            item._item_type = (item_type)equipItem.type;
            item.icon_number = equipItem.icon_number;
            item.slot_number = equipItem.slot_type;
            item.type_number = equipItem.type;
            _dictionary.Add(item._name, item);
        }
    }


}