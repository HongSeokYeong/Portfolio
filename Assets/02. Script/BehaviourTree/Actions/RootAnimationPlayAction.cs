using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAnimationPlayAction : ActionNode
{
    // 애니메이션 재생할 파라미터 이름
    public string parameterKey;
    private Animator animator;

    public override void NodeStart()
    {
        if (animator == null)
        {
            animator = GetBehaviourTreeController().animator;
        }
        animator.SetTrigger(parameterKey);
        animator.applyRootMotion = true;
    }

    public override void NodeStop()
    {
        animator.applyRootMotion = false;
    }

    public override E_State NodeUpdate()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(parameterKey))
        {
            return E_State.Running;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f)
        {
            return E_State.Running;
        }

        return E_State.Success;
    }
}
