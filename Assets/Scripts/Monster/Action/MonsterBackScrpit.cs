using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBackScrpit : StateMachineBehaviour
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
        if(_monster._isDead || _monster._CurrentState == MonsterControl.State.Skill) return;
        if (Vector3.Distance(_trans.position, _monster._BackPoint) < _nav.stoppingDistance || Vector3.Distance(_trans.position, _monster._player.transform.position) < _monster._distance)
        {

            animator.SetBool("IsBack", false);
        }
        else
        {

            _trans.LookAt(_monster._BackPoint); // 해당 방향을 바라보게
            //_trans.position = Vector3.MoveTowards(_trans.position, _monster._BackPoint, _nav.speed * Time.deltaTime);
            _nav.SetDestination(_monster._BackPoint);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
