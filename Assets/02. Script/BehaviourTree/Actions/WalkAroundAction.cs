using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAroundAction : ActionNode
{
    float verticalMovementValue = 0.0f;
    float horizontalMovementValue = 0.0f;
    float targetHorizontalValue = 0.0f;
    float targetVerticalValue = 0.0f;

    float currentValue = 0.0f;

    float lerpTime;

    public override void NodeStart()
    {
        GetBehaviourTreeController().agent.updatePosition = false;

        var rand = Random.Range(0, 1.0f);

        GetBehaviourTreeController().animator.SetBool("Move", true);

        if (rand < 0.5f)
        {
            var attackOnTarget = blackboard.GetValueAsBool("AttackOnTarget");
            blackboard.SetValueAsBool("AttackOnTarget", !attackOnTarget);
            targetHorizontalValue = horizontalMovementValue;
            targetVerticalValue = verticalMovementValue;
        }

        lerpTime = 0.0f;
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        if (blackboard.GetValueAsBool("AttackOnTarget"))
        {
            lerpTime += Time.deltaTime * 2.0f;
            if (lerpTime <= 1.0f)
            {
                horizontalMovementValue = Mathf.Lerp(targetHorizontalValue, 0.0f, lerpTime);
                verticalMovementValue = Mathf.Lerp(targetVerticalValue, 1.0f, lerpTime);

                GetBehaviourTreeController().animator.SetFloat("Horizontal", horizontalMovementValue);
                GetBehaviourTreeController().animator.SetFloat("Vertical", verticalMovementValue);

                return E_State.Running;
            }

            horizontalMovementValue = 0.0f;
            verticalMovementValue = 1.0f;

            GetBehaviourTreeController().animator.SetFloat("Horizontal", horizontalMovementValue);
            GetBehaviourTreeController().animator.SetFloat("Vertical", verticalMovementValue);

            return E_State.Success;
        }

        if (state != E_State.Running)
        {
            targetHorizontalValue = Random.Range(0, 2);

            if (targetHorizontalValue == 0)
            {
                targetHorizontalValue = 1.0f;
            }
            else if (targetHorizontalValue == 1)
            {
                targetHorizontalValue = -1.0f;
            }
        }

        lerpTime += Time.deltaTime * 2.0f;
        if (lerpTime <= 1)
        {
            currentValue = Mathf.Lerp(horizontalMovementValue, targetHorizontalValue, lerpTime);
            targetVerticalValue = Mathf.Lerp(verticalMovementValue, 0.0f, lerpTime);

            GetBehaviourTreeController().animator.SetFloat("Vertical", targetVerticalValue);
            GetBehaviourTreeController().animator.SetFloat("Horizontal", currentValue);

            return E_State.Running;
        }

        horizontalMovementValue = targetHorizontalValue;
        verticalMovementValue = 0.0f;

        GetBehaviourTreeController().animator.SetFloat("Horizontal", horizontalMovementValue);
        GetBehaviourTreeController().animator.SetFloat("Vertical", verticalMovementValue);

        return E_State.Success;
    }
}