using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class Skill_Manager : MonoBehaviour
{

    static Skill_Manager _my;
    public static Skill_Manager _instance { get { return _my; } }
    [SerializeField] private GameObject _Casting;
    [SerializeField] private GameObject[] _Pre_Prefab;
    public Image _Fill;
    public Text _Text;

    private UserControl player;
    private void Awake()
    {
        _my = this;
    }
    private void Start()
    {
        player = UserControl._instance;
    }


    public void UsingSkill(QuickSlots slot)
    {
        player._Action.SetBool("Skill", true);
        player._Action.SetFloat("SkillType", slot._skilltype);
        player._skilltype = (int)slot._skilltype;
        _Text.text = slot._name;
        StartCoroutine(DurationTime(slot));
        StartCoroutine(Cooling(slot));
    }
    public void UsingAreaSkill(QuickSlots slot)
    {
        player._areaskill = true;
        player._areaOrigin = slot;
        CreateAreaPrefab(_Pre_Prefab[(int)slot._skilltype]);
    }
    public void UsingAreaSkill_Start(QuickSlots slot, Transform Pos)
    {
       
        GameObject obj =  Instantiate(SkillData._instance.PreFab[(int)slot._skilltype], Pos.position, Quaternion.identity);
        Skill skill = SkillControl._instance.GetSkill(slot._name);
        obj.GetComponentInChildren<Area_Control>().Init(slot._duration, skill.current_level, skill.sound);
        StartCoroutine(Cooling(slot));
    }
    void CreateAreaPrefab(GameObject prefab)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane Ground = new Plane(Vector3.up, Vector3.zero);
        if (Ground.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            player._areaprefab = Instantiate(prefab, worldPosition, Quaternion.identity);
        }

    }

    public void Using_BuffSkill(QuickSlots slot)
    {
        player._Action.SetBool("Skill", true);
        player._Action.SetFloat("SkillType", slot._skilltype);
        player._skilltype = (int)slot._skilltype;
        _Text.text = slot._name;
        Skill skill = SkillControl._instance.GetSkill(slot._name);
        Buff_Manager._instance.Create(slot._icon.sprite, slot._duration, skill.sound, skill._effect_type, skill.current_level * 2);
        StartCoroutine(Cooling(slot));
    }

    IEnumerator Cooling(QuickSlots slot)
    {
        slot._CooltimeImage.SetActive(true);
        slot._coolText.gameObject.SetActive(true);
        slot._Cool.fillAmount = 1;
        while (slot._current_cool_time < slot._cool_time)
        {
            slot._current_cool_time += Time.deltaTime;
            slot._Cool.fillAmount = 1 - slot._current_cool_time / slot._cool_time;

            int remain = Mathf.FloorToInt(slot._cool_time - slot._current_cool_time);
            slot._coolText.text = remain.ToString();
            yield return null;
        }
        slot._CooltimeImage.SetActive(false);
        slot._coolText.gameObject.SetActive(false);
        slot._current_cool_time = 0;
    }
    IEnumerator DurationTime(QuickSlots slot)
    {

        slot._UsingTime = 0f;

        _Casting.SetActive(true);
        while (slot._UsingTime < slot._duration)
        {
            slot._UsingTime += Time.deltaTime;
            _Fill.fillAmount = slot._UsingTime / slot._duration;
            yield return null;
        }

        _Casting.SetActive(false);
        player._Action.SetBool("Skill", false);
        switch (player._skilltype)
        {
            case 1:
                player._Action.SetBool("IsAttack", false);
                break;
            case 2:
                GameObject spell = Instantiate(SkillData._instance.PreFab[ (int)slot._skilltype], player._Action.GetBoneTransform(HumanBodyBones.UpperChest).position, transform.rotation);
                Skill current = SkillControl._instance.GetSkill(slot._name);

                spell.GetComponent<Target_Spell>().Spell(null,current.current_level * 3, current.current_level * 3, current.sound, current);


                break;
        }
        yield return new WaitForSecondsRealtime(0.3f);
        player._Action.Play("Idle,Walk"); // 상태를 강제로 바꿔준다.
        player._skilltype = 0;
        yield return null;

    }
}
