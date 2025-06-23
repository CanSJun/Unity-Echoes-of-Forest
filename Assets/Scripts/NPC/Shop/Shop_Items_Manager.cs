using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Items_Manager : MonoBehaviour
{
    [SerializeField] Text _nameText;
    [SerializeField] Text _bodyText;
    [SerializeField] Text _goldText;
    [SerializeField] Image _icon;
    [SerializeField] AudioClip _sound;
    
    private UserControl Player;
    private int _gold;
    private ItemControl _item;
    private void Start() => Player = UserControl._instance;
    


    public void Create_Shop_Item(ItemControl item)
    {
        _icon.sprite = item._Icon;
        _nameText.text = item._name;
        _bodyText.text = item._description;
        _goldText.text = string.Format("{0} Gold", item._gold);
        _gold = item._gold;
        _item = item;
    }

    public void Buy()
    {
        if (Player._money >= _gold)
        {
            Player._money -= _gold;
            if (InventorySystem.instance._MaxSlots <= 0)
            {
                LogSystem._instance.UpdateLog("인벤토리가 부족합니다.");
                return;
            }
            LogSystem._instance.UpdateLog($"{_item._name}을 구매 하셨습니다.");
            InventorySystem.instance.Item_add(_item);
            NPC_Shop_Control._instance.UpdateState(Player._money);

            SoundManager._instance.PrivatePlaySound(_sound, Player.transform,  0.3f);

        }
        else
        {
            LogSystem._instance.UpdateLog("금전이 부족합니다.");
        }
    }
}
