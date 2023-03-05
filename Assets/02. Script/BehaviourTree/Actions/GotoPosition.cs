using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GotoPosition", menuName = "BehaviourTree/ActionNode/GotoPosition")]
public class GotoPosition : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    public override void NodeStart()
    {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.destPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;

        context.animator.SetTrigger("Run");
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        if (context.agent.pathPending)
        {
            return E_State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            return E_State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return E_State.Failure;
        }

        return E_State.Running;
    }
}
