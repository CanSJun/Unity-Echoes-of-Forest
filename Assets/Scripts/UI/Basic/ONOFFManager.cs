using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ONOFFManager : MonoBehaviour
{
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject Skill;
    [SerializeField] GameObject NPCSHOP;
    [SerializeField] GameObject STATUS;



    void ONOFF(bool b)
    {
        if (!b)
        {
            Inventory.gameObject.SetActive(false);
            Skill.gameObject.SetActive(false);
            NPCSHOP.gameObject.SetActive(false);
            STATUS.gameObject.SetActive(false);
        }
        else
        {
            Inventory.gameObject.SetActive(true);
            Skill.gameObject.SetActive(true);
            NPCSHOP.gameObject.SetActive(true);
            STATUS.gameObject.SetActive(true);
        }
    }
    void Start()
    {
        ONOFF(true);
        ONOFF(false);
    }
}
