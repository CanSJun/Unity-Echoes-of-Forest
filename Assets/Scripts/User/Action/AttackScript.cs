using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField] private GameObject Effect;
    private UserControl Player;
    void Sound()
    {
        switch (Player._CurrentWeapon)
        {
            case UserControl.Weapon.Hands:
                SoundManager._instance.PlaySound(SoundManager.SOUNDTYPE.TargetHit);
                break;
            default:
                SoundManager._instance.PlaySound(SoundManager.SOUNDTYPE.One_Hand_Axe);
                break;
        }
    }

    void OnTriggerEnter(Collider target)
    {
        if (Player == null) Player = UserControl._instance;

        if ((target.CompareTag("Boss")  || target.CompareTag("Monster")) && (Player._Action.GetBool("IsAttack") || Player._skilltype == 1))
        {
            MonsterControl mob = target.GetComponent<MonsterControl>();
            if (mob._isDead) return;
            //자는 적 공격 시 잠 깨우기
            if (mob == true && mob._CurrentState == MonsterControl.State.Sleep) mob._CurrentState = MonsterControl.State.None;

            Vector3 cal = target.ClosestPoint(transform.position) - target.transform.position;
            GameObject effect = Instantiate(Effect, cal, Quaternion.identity);
            effect.transform.SetParent(target.transform, false);

            Sound();
            mob.CheckHitted(Player._damage);

        }
            
        


    }
}
