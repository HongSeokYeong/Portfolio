using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchPosition : ActionNode
{
    // ��尡 ������ �ִ� �Ϲ����� ����
    public Vector2 randomRange;

    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    // ������ �׺���̼� �� �ȿ��� �������� ��ġ�� ã�� �׼�
    public override E_State NodeUpdate()
    {
        if (!blackboard.GetValueAsBool("HasPosition"))
        {
            Vector3 pos = new Vector3();
            pos.x = Random.Range(-randomRange.x, randomRange.x);
            pos.z = Random.Range(-randomRange.y, randomRange.y);
            GetBehaviourTreeController().agent.destination = pos;
            blackboard.SetValueAsBool("HasPosition", true);
            GetBehaviourTreeController().animator.SetTrigger("Walk");
        }

        return E_State.Success;
    }
}
