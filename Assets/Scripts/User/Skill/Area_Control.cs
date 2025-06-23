using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Area_Control : MonoBehaviour
{

    private float _delay = 0.3f;
    private float _current_delay = 0.3f;
    private float _damage = 0f;

    public void Init(float duration, float damage, int index) {


        _damage = damage;

        Invoke("Destroy", duration);
        SoundManager._instance.PrivatePlaySound(SkillData._instance.Sound[index], transform, 0.5f);
    }

    void Update()
    {
        if (_current_delay <= _delay) _current_delay += Time.deltaTime;
    }

     void OnTriggerEnter(Collider target)
    {
        if ((target.CompareTag("Boss")  || target.CompareTag("Monster")) && _current_delay >= _delay)
        {
            Attack(target);
        }
    }
    void OnTriggerStay(Collider target)
    {
        if ((target.CompareTag("Boss") || target.CompareTag("Monster")) && _current_delay >= _delay)
        {
            Attack(target);
        }
    }

    void Attack(Collider target)
    {
        _current_delay = 0f;
        //자는 적 공격 시 잠 깨우기
        if (target.GetComponent<MonsterControl>()._CurrentState == MonsterControl.State.Sleep) target.GetComponent<MonsterControl>()._CurrentState = MonsterControl.State.None;
        target.GetComponent<MonsterControl>().CheckHitted(_damage);

    }
    void Destroy() =>  Destroy(gameObject);
    
    
}
