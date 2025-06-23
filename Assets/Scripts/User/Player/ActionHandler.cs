using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using static UserControl;

public class ActionHandler : MonoBehaviour
{
    [SerializeField] GameObject description;
    public void Handle_Gather(ref State currentState, Animator action, ItemControl item)
    {
        if (item != null && currentState != State.Gathering)
        {
            currentState = State.Gathering;
            action.SetTrigger("Gather");
        }
    }
    public void Handle_Enter(KeyCode key, ref bool check, GameObject UI)
    {
        if (Input.GetKeyDown(key) && !check)Open(ref check, UI);
        else if (Input.GetKeyDown(key) && check) Close(ref check, UI);
    }
    public void Handle_Escape(ref bool inventorycheck, ref bool statuscheck, ref bool skillCheck, GameObject inventory, GameObject status, GameObject skill)
    {
            if(description.gameObject.activeSelf == true) description.gameObject.SetActive(false);
            if (inventorycheck)Close(ref inventorycheck, inventory); // 인벤토리창
            else if (statuscheck)  Close(ref statuscheck, status); // 상태창
            else if (skillCheck) Close(ref skillCheck, skill); // 스킬창
    }
    public void Open(ref bool check, GameObject UI)
    {
        check = true;
        UI.SetActive(true);
        UI.transform.SetAsLastSibling();
    }

    public void Close(ref bool check, GameObject UI)
    {
        if (description.gameObject.activeSelf == true) description.gameObject.SetActive(false);
        check = false;
        UI.SetActive(false);
    }

    public void Handle_Attack(bool isAttack, bool isInventoryOpen, bool isSkillBookOpen, bool isStatusOpen, ref int skillType, Weapon currentWeapon, Animator action, Transform transform)
    {

        if (!isAttack && !isInventoryOpen && !isSkillBookOpen && !isStatusOpen && skillType == 0)
        {

            if (skillType == 2) skillType = 0;
            action.SetTrigger("Attack");
            switch (currentWeapon)
            {
                case Weapon.Hands:
                    SoundManager._instance.PlaySound(Random.Range(0, 2) == 0 ? SoundManager.SOUNDTYPE.Attack : SoundManager.SOUNDTYPE.Attack2);
                    break;

                default:
                    SoundManager._instance.PrivatePlaySound(UserControl._instance.AttackSound , transform);
                    break;
            }

            StartCoroutine(AttackTime());
        }
    }

    IEnumerator AttackTime()
    {
        
        yield return new WaitForSecondsRealtime(UserControl._instance._attackSpeed);
        UserControl._instance._isAttack = false;

    }
}
