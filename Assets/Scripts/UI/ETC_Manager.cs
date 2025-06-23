using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETC_Manager : MonoBehaviour
{


    int Mob_Count = 1;

    private void Start()
    {
        Invoke("Init", 0.1f);
    }
    void Init()
    {
        GameData data = MasterControl._Instance.Data;
        if(data != null)
        {
            GameObject[] Mob = GameObject.FindGameObjectsWithTag("Monster");
            foreach(var x in Mob)
            {

                if (Mob_Count > data.BEAR)
                {
                    Destroy(x);
                    continue;
                }
                Mob_Count++;
            }

            if (data.BOSS == 0)
            {
                GameObject Boss = GameObject.FindGameObjectWithTag("Boss");
                Destroy(Boss);
            }
        }
    }

}
