using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quick_Slots_manager : MonoBehaviour
{
    static Quick_Slots_manager _my;
    public static Quick_Slots_manager _instance { get { return _my; } }

    private void Awake()
    {
        _my = this;

    }

    public GameObject[] Slots;

    public void CheckSlot(bool b)
    {
        if (b == true)
        {
            for (int i = 0; i < Slots.Length; i++)Slots[i].SetActive(true);
        }
        else
        {
            for (int i = 0; i < Slots.Length; i++) {

                if (Slots[i].GetComponent<Image>().sprite == null)
                    {
                        Slots[i].SetActive(false);

                    }
                
            
            }
        }
    }


}
