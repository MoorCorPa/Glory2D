using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
/*    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!stateInfo.IsName("idle"))
        {
            CombatManager.instance.canReceiveInput = true;
        }
        if (animator.GetInteger("attackState") == 0)
        {
            CombatManager.instance.canReceiveInput = true;
        }
    }*/

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if (PlayerController.instance.isAttacking)
        // {
        //     var atkAnim = "normal_punch";
        //     if (stateInfo.IsName("translation1"))
        //     {
        //         atkAnim = "normal_back";
        //     }
        //     else if (stateInfo.IsName("translation2"))
        //     {
        //         atkAnim = "normal_shock";
        //     }
        //
        //     PlayerController.instance.animator.Play(atkAnim);
        // }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //PlayerController.instance.isAttacking = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
