using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchPosition : ActionNode
{
    // 노드가 가지고 있는 일반적인 변수
    public Vector2 randomRange;

    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    // 정해진 네비게이션 맵 안에서 랜덤으로 위치를 찾는 액션
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
