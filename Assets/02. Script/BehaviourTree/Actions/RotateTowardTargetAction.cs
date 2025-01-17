using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotateTowardTargetAction : ActionNode
{
    public float rotationSpeed;
    // context 변수
    public NavMeshAgent navMeshAgent;
    public Rigidbody rigidbody;

    // 블랙보드키
    public GameObject currentTarget;

    bool isRotating = false;

    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        if (blackboard.GetValueAsTransform("CurrentTarget") == null || isRotating == true)
        {
            return E_State.Failure;
        }

        // rotate manually
        {
            var direction = blackboard.GetValueAsTransform("CurrentTarget").position - GetBehaviourTreeController().transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = GetBehaviourTreeController().transform.forward;
            }

            var targetRotation = Quaternion.LookRotation(direction);
            GetBehaviourTreeController().transform.rotation = Quaternion.Slerp(GetBehaviourTreeController().transform.rotation, targetRotation, rotationSpeed);
        }

        return E_State.Success;
    }
}
