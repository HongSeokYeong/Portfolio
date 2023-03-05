using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "SearchPosition", menuName = "BehaviourTree/ActionNode/SearchPosition")]
public class SearchPosition : ActionNode
{
    public Vector2 randomRange;
    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        Vector3 pos = new Vector3();
        pos.x = Random.Range(-randomRange.x, randomRange.x);
        pos.z = Random.Range(-randomRange.y, randomRange.y);
        blackboard.destPosition = pos;

        NavMeshHit hit;

        if(NavMesh.SamplePosition(blackboard.destPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            return E_State.Success;
        }

        return E_State.Running;
    }
}
