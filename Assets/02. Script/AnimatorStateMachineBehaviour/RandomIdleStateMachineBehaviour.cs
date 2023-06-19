using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleStateMachineBehaviour : StateMachineBehaviour
{
    public int numberOfState;
    public float randomNormalizedTime;

    public int hashRandomIdle = Animator.StringToHash("RandomIdle");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Vertical", 0.0f);
        animator.SetFloat("Horizontal", 0.0f);
        randomNormalizedTime = Random.Range(0.0f, 4.0f);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
        {
            animator.SetInteger(hashRandomIdle, -1);
        }

        if (stateInfo.normalizedTime > randomNormalizedTime && !animator.IsInTransition(0))
        {
            animator.SetInteger(hashRandomIdle, Random.Range(0, numberOfState));
        }
    }
}
