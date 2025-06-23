using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Manager : MonoBehaviour
{
    [Header("�Ǹ� ������ ���")]
    [SerializeField] string[] items;
    

    
    private void Start()
    {
        FirstShopSetting();
    }

    void FirstShopSetting()
    {
        foreach(var item in items)
        {
 
            ItemControl newitem = ItemDB._instance.GetItem(item);
            if(newitem != null)
            {
                NPC_Shop_Control._instance.Create(newitem);
            }
        }

    }

}
