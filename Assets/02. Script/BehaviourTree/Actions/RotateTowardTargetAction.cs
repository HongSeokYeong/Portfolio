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
        // rotate with pathfinding(navmesh)
        {
            //var relativeDirection = GetBehaviourTreeController().transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
            //Vector3 targetVelocity = rigidbody.velocity;

            //navMeshAgent.enabled = true;
            //navMeshAgent.SetDestination(currentTarget.transform.position);
            //rigidbody.velocity = targetVelocity;
            //GetBehaviourTreeController().transform.rotation = Quaternion.Slerp(GetBehaviourTreeController().transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
        }

        // 턴 애니메이션 추가
        //Vector3 targetDirection = blackboard.GetValueAsTransform("currentTarget").position - GetBehaviourTreeController().transform.position;
        //float viewableAngle = Vector3.SignedAngle(targetDirection, GetBehaviourTreeController().transform.forward, Vector3.up);

        //if (viewableAngle >= 100 && viewableAngle <= 180)
        //{
        //    // 루트 모션으로 뒤로돌기턴 애니메이션 재생
        //    // animator.applyRootMotion = true;
        //    return E_State.Success;
        //}
        //else if (viewableAngle <= -101 && viewableAngle >= -180)
        //{
        //    // 루트 모션으로 뒤로돌기턴 애니메이션 재생

        //}
        //else if (viewableAngle <= -45 && viewableAngle >= -100)
        //{
        //    // 루트 모션으로 오른쪽턴 애니메이션 재생
        //    GetBehaviourTreeController().animator.applyRootMotion = true;
        //    GetBehaviourTreeController().animator.SetTrigger("GreatSwordRightTurn");
        //    isRotating = true;
        //    return E_State.Success;
        //}
        //else if (viewableAngle >= 45 && viewableAngle <= 100)
        //{
        //    // 루트 모션으로 왼쪽 턴 애니메이션 재생
        //    GetBehaviourTreeController().animator.applyRootMotion = true;
        //    GetBehaviourTreeController().animator.SetTrigger("GreatSwordLeftTurn");
        //    isRotating = true;
        //    return E_State.Success;
        //}

        return E_State.Success;
    }
}
