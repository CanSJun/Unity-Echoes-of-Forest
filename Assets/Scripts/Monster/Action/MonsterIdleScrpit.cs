using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterIdleScrpit : StateMachineBehaviour
{
    Transform _trans;
    MonsterControl _monster;
  
    NavMeshAgent _nav;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        _trans = animator.GetComponent<Transform>();
        _monster = animator.GetComponent<MonsterControl>();
        _monster._CurrentState = MonsterControl.State.None;
        _nav = animator.GetComponent<NavMeshAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (_monster._isDead || _monster._CurrentState == MonsterControl.State.Skill) return;
  
        _nav.velocity = Vector3.zero;
        if (Vector3.Distance(_trans.position, _monster._player.transform.position) < _nav.stoppingDistance)
        {
            if(_monster.QuestNum == 2) _monster.Boss_Skill_Delay();
            if (_monster.attackdelay <= 0)
            {

                    if (_monster._CurrentState != MonsterControl.State.Skill)
                    {
                        _monster._CurrentState = MonsterControl.State.Attack;
                        animator.SetFloat("AttackType", Random.Range(0, 3));
                        animator.SetTrigger("Attack");
                        _monster.attackdelay = _monster._attackspeed;
                        _monster.Attack();
                        
                    }
                
            }

        }
        else
        {
            
            if (Vector3.Distance(_trans.position, _monster._player.transform.position) < _monster._distance)
            {

                
                _monster._CurrentState = MonsterControl.State.None;
                animator.SetBool("IsWalk", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


}
