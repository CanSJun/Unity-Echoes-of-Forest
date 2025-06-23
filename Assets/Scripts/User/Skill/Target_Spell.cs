using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Target_Spell : MonoBehaviour
{


    private float damage = 0f;
    private float _CurrentTime;
    private float _MaxTime; // 살아있을 정도
    private GameObject _monster;


    private int _index;
    private Skill _skill;
    public void Spell(GameObject target, float value, float duration, int index, Skill skill)
    {

        if (target == null)
        {
            target = Search_Target();
            if (target == null) return;
        }
        damage = value;
        _monster = target;
        _MaxTime = duration;
        _index = index;
        _skill = skill;
    }

    GameObject Search_Target()
    {
        float _minDistance = 100f;
        int _Imask = 1 << LayerMask.NameToLayer("Monster");
        Collider[] monsters = Physics.OverlapSphere(transform.position, 10f, _Imask);
        GameObject target = null;
        foreach (var  monster in monsters)
        {
            if (monster.GetComponent<MonsterControl>()._isDead) continue;
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < _minDistance) { _minDistance = distance; target = monster.gameObject; }
        }
        return target;
    }

    void Update()
    {
        if (_CurrentTime < _MaxTime) _CurrentTime += Time.deltaTime;
        else Destroy(gameObject);
        if (_monster == null ||  _monster.GetComponent<MonsterControl>()._isDead)
        {
            _monster = Search_Target();
            if (_monster == null) // 만약에 _monster가 없을 경우 새로운 몹을 찾아 이동하지만 주변에 몹이 없을 경우 소멸
            {
                Destroy(gameObject); return;
            }

        }
        transform.LookAt(_monster.transform); // 해당 방향을 바라보게
        transform.position = Vector3.MoveTowards(transform.position, _monster.transform.position, 3f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag("Boss") || target.CompareTag("Monster"))
        {
            bool check = false;
            if (target.CompareTag("Boss")) check = true;


            if (target.GetComponent<MonsterControl>()._CurrentState == MonsterControl.State.Sleep) target.GetComponent<MonsterControl>()._CurrentState = MonsterControl.State.None;
            target.GetComponent<MonsterControl>().CheckHitted(damage);

            switch (_skill._skill_type)
            {
                case Skill_Type.target:
                    target.GetComponentInChildren<Hp_Bar_Buff>().Init(_skill.current_level * 3, target.GetComponent<MonsterControl>(), Debuff_Type.Electric_Shock, _skill.skill_icon, check);
                break;
            }
            SoundManager._instance.PrivatePlaySound(SkillData._instance.Sound[_index], transform);
            Destroy(gameObject);
        }
        
    }
}
