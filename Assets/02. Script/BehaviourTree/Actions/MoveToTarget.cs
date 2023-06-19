using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// �÷��̾ Ÿ�� �� �����̴� ���
public class MoveToTarget : ActionNode
{
    public float distanceFromTarget;

    // Context�� ���� ������
    public Animator animator;
    public NavMeshAgent navMeshAgent;

    // ������ Ű�� ����Ѵ�
    public GameObject currentTarget;
    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        // ownVector�� ���߿� BT�� ��Ʈ�ѷ��� transform�� �����ؾ��Ѵ�.
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
