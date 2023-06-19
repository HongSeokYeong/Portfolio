using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 플레이어를 타겟 후 움직이는 노드
public class MoveToTarget : ActionNode
{
    public float distanceFromTarget;

    // Context로 묶을 변수들
    public Animator animator;
    public NavMeshAgent navMeshAgent;

    // 블랙보드 키로 써야한다
    public GameObject currentTarget;
    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        // ownVector는 나중에 BT의 컨트롤러의 transform에 접근해야한다.
        Transform ownerTransform = default;

        var targetDirection = currentTarget.transform.position - ownerTransform.position;
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, ownerTransform.transform.position);
        var angle = Vector3.Angle(targetDirection, ownerTransform.forward);

        if (distanceFromTarget > navMeshAgent.stoppingDistance)
        {
            animator.SetFloat("MoveBlend", 1, 0.1f, Time.deltaTime);
        }

        return E_State.Success;
    }
}
