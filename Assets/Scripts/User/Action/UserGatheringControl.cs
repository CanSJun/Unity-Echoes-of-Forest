using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGatheringControl : StateMachineBehaviour
{
    Transform _trans;
    UserControl _player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _trans = animator.GetComponent<Transform>();
        _player = animator.GetComponent<UserControl>();

        
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       _player._CurrentState = UserControl.State.Idle;
    }

}
