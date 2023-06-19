using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.AI;

public class GotoPosition : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    public NavMeshAgent agent;

    public override void NodeStart()
    {
        var animator = GetBehaviourTreeController().animator;
        if (agent == null)
        {
            agent = GetBehaviourTreeController().agent;
        }

        agent.stoppingDistance = stoppingDistance;
        agent.speed = speed;
        agent.updateRotation = updateRotation;
        agent.acceleration = acceleration;
        agent.isStopped = false;

        animator.SetBool("Move", true);
        GetBehaviourTreeController().animator.SetFloat("Vertical", 1.0f);
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        //if (agent.pathPending)
        //{
        //    return E_State.Running;
        //}

        if (agent.remainingDistance < tolerance)
        {
            blackboard.SetValueAsBool("HasPosition", false);
            return E_State.Success;
        }

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            return E_State.Failure;
        }

        return E_State.Success;
    }
}
