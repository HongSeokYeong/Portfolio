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

        // 거리가 0~1이면 0, 1
        // 거리가 1~2이면 2, 3
        // 거리가 2~3이면 4, 5
        //Debug.Log("거리 ==> " + distanceFromTarget);
        // 3.3 점프어택 파워어택2
        // 2.3 콤보어택
        // 1.5 노말어택1 2
        // 1.7 파워어택
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
