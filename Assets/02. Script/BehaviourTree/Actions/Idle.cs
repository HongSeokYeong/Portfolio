using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle", menuName = "BehaviourTree/Idle")]
public class Idle : ActionNode
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
