using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchPosition : ActionNode
{
    // 노드가 가지고 있는 일반적인 변수
    public Vector2 randomRange;

    // 노드가 가지고 있는 블랙보드 변수
    public BlackBoardKeyType_Vector3 vector3Pos;
    public BlackBoardKeyType_Bool a;
    public BlackBoardKeyType_Class b;
    public BlackBoardKeyType_Enum c;
    public BlackBoardKeyType_Float d;
    public BlackBoardKeyType_Int e;
    public BlackBoardKeyType_Object f;
    public BlackBoardKeyType_Rotation h;
    public BlackBoardKeyType_String j;
    public BlackBoardKeyType_Transform i;

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
        //blackboard.destPosition = pos;

        NavMeshHit hit;

        //if(NavMesh.SamplePosition(blackboard.destPosition, out hit, 1.0f, NavMesh.AllAreas))
        //{
        //    return E_State.Success;
        //}

        return E_State.Running;
    }
}
