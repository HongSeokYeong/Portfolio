using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 액션은 이동 액션 후에 공격 거리(범위) 안에 들어오면 실행되는 액션
public class AttackAction : ActionNode
{
    public bool rootAnimationEnd;
    public override void NodeStart()
    {
        GetBehaviourTreeController().animator.SetInteger("AttackNumber", blackboard.GetValueAsInt("AttackNumber"));
        rootAnimationEnd = false;
    }

    public override void NodeStop()
    {
        GetBehaviourTreeController().animator.SetBool("Attack", false);
    }

    public override E_State NodeUpdate()
    {
        GetBehaviourTreeController().animator.SetBool("Attack", true);

        if (!rootAnimationEnd)
        {
            if (GetBehaviourTreeController().animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && GetBehaviourTreeController().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                rootAnimationEnd = true;
            }

            return E_State.Running;
        }

        return E_State.Success;
    }
} 
