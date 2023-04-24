using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : ActionNode
{
    public override void NodeStart()
    {
        context.animator.SetTrigger("Idle");
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        return E_State.Success;
    }
}
