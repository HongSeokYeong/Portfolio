using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GotoPosition", menuName = "BehaviourTree/GotoPosition")]
public class GotoPosition : ActionNode
{
    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        return E_State.Success;
    }
}
