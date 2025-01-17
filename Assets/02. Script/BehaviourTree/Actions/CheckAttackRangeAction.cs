using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttackRangeAction : ActionNode
{
    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        var myTransform = GetBehaviourTreeController().transform;

        var enemyTransform = blackboard.GetValueAsTransform("CurrentTarget");

        Vector3 targetDirection = myTransform.position - enemyTransform.position;
        float viewableAngle = Vector3.Angle(targetDirection, myTransform.forward);

        var distanceFromTarget = Vector3.Distance(enemyTransform.position, myTransform.position);

        if (distanceFromTarget <= 1.5f)
        {
            blackboard.SetValueAsInt("AttackNumber", Random.Range(0, 2));
        }
        else if(distanceFromTarget <= 1.7f)
        {
            blackboard.SetValueAsInt("AttackNumber", 2);
        }
        else if(distanceFromTarget <= 2.3f)
        {
            blackboard.SetValueAsInt("AttackNumber", 3);
        }
        else if(distanceFromTarget <= 3.3f)
        {
            blackboard.SetValueAsInt("AttackNumber", 4);
        }
        else if(distanceFromTarget <= 3.7f)
        {
            blackboard.SetValueAsInt("AttackNumber", 5);
        }
        else
        {
            return E_State.Failure;
        }

        return E_State.Success;
    }
}
