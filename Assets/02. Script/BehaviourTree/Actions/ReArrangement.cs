using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
public class ReArrangement : ActionNode
{
    public override void NodeStart()
    {
        GetBehaviourTreeController().animator.SetFloat("Vertical", -1.0f);
        GetBehaviourTreeController().animator.SetFloat("Horizontal", 0);
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        var myTransform = GetBehaviourTreeController().transform;

        var enemyTransform = blackboard.GetValueAsTransform("CurrentTarget");

        var direction = enemyTransform.position - GetBehaviourTreeController().transform.position;
        direction.y = 0;
        direction.Normalize();

        if (direction == Vector3.zero)
        {
            direction = GetBehaviourTreeController().transform.forward;
        }

        var targetRotation = Quaternion.LookRotation(direction);
        GetBehaviourTreeController().transform.rotation = Quaternion.Slerp(GetBehaviourTreeController().transform.rotation, targetRotation, 10.0f);

        var distanceFromTarget = Vector3.Distance(enemyTransform.position, myTransform.position);

        if (distanceFromTarget <= 2.0f)
        {
            return E_State.Running;
        }
        else
        {
            return E_State.Success;
        }
    }
}
