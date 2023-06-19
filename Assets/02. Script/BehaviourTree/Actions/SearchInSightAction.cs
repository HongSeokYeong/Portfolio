using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchInSightAction : ActionNode
{
    public float detectionRadius;
    public LayerMask detectionLayer;
    public float minDetectionAngle;
    public float maxDetectionAngle;
    public Transform tr;

    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        // ownVector는 나중에 BT의 컨트롤러의 transform에 접근해야한다.
        Collider[] colliders = Physics.OverlapSphere(GetBehaviourTreeController().transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            var player = colliders[i].gameObject;

            if (player != null)
            {
                Vector3 targetDirection = player.transform.position - GetBehaviourTreeController().transform.position;
                var angle = Vector3.Angle(targetDirection, GetBehaviourTreeController().transform.forward);

                if (angle > minDetectionAngle && angle < maxDetectionAngle)
                {
                    blackboard.SetValueAsTransform("CurrentTarget", player.transform);
                    blackboard.SetValueAsBool("HasTarget", true);
                    blackboard.SetValueAsBool("HasPosition", false);
                    GetBehaviourTreeController().animator.SetTrigger("Idle");
                    GetBehaviourTreeController().animator.SetBool("Move", false);
                    GetBehaviourTreeController().agent.isStopped = true;
                    tr = player.transform;
                    return E_State.Success;
                }
            }
        }

        blackboard.SetValueAsTransform("CurrentTarget", null);
        blackboard.SetValueAsBool("HasTarget", false);
        return E_State.Failure;
    }
}
