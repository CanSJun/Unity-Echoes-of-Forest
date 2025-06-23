using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    Transform _trans;
    UserControl _player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _trans = animator.GetComponent<Transform>();
        _player = animator.GetComponent<UserControl>();


        //ATTACK OPTION
        // 1 ��ġ
        // 2 ���ڵ� ����, �ܰ�
        // 3 ���ڵ� ����
        // 4 Ȱ
        // 5 ����
        // 6 �ֹ�
        // 7 ��� ȸ������

        animator.SetFloat("AttackType", (int)UserControl._instance._CurrentWeapon + 1);
        
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player._isAttack == false)
        {
            animator.SetBool("IsAttack", true);
            _player._isAttack = true;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsAttack", false);
        _player._CurrentState = UserControl.State.Idle;
    }


}
